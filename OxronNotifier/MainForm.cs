using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxronNotifier.Model;
using OxronNotifier.OxronApi;
using OxronNotifier.Utility;

namespace OxronNotifier
{
    public partial class MainForm : Form
    {
        private const int MaxHistory = 100;

        private BindingList<ServerMonitor> _monitors;
        private BindingList<EventHistory> _history;
        private FileInfo _settingsFile;
        private Configuration _configuration;
        private MenuItem _menuConfigure;
        private MenuItem _menuLastChecked;
        private MenuItem _menuLastMessage;
        private MenuItem _menuExit;
        private TaskScheduler _uiScheduler;
        private List<Form> _subForms;

        public MainForm()
        {
            InitializeComponent();

            _subForms = new List<Form>();
            _history = new BindingList<EventHistory>();
            _monitors = new BindingList<ServerMonitor>();

            _settingsFile = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OxronNotifier", "config.xml"));
            _configuration = new Configuration();

            if (!_settingsFile.Directory.Exists)
                _settingsFile.Directory.Create();

            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            try
            {
                if (_settingsFile.Exists)
                    _configuration.Load(_settingsFile.FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the settings file 'config.xml'." + Environment.NewLine + ex.Message + Environment.NewLine + _settingsFile.FullName, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Initialize();

            if (!_configuration.Servers.Any())
                ShowConfigureWindow();
        }

        private void CancelAllMonitors(bool waitForExit)
        {
            _monitors.ForEach(s => s.Stop(waitForExit));
            _monitors.Clear();
        }

        private void Initialize()
        {
            notify.ContextMenu = BuildContextMenu();

            CancelAllMonitors(true);

            // Build monitors
            foreach (ServerConfiguration serverConfiguration in _configuration.Servers)
            {
                ServerMonitor monitor = new ServerMonitor(serverConfiguration);
                monitor.OnTaskFinished += MonitorOnOnTaskFinished;
                monitor.OnTaskDiscovered += MonitorOnOnTaskDiscovered;
                monitor.OnStateChanged += state => MonitorOnOnStateChanged(monitor);
                //monitor.PropertyChanged += MonitorOnPropertyChanged;

                _monitors.Add(monitor);
            }

            // Start monitors
            _monitors.ForEach(s => s.StartAsync());
        }

        //private void MonitorOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        //{
        //    ServerMonitor monitor = sender as ServerMonitor;
        //    if (monitor == null)
        //        return;

        //    _menuLastChecked.Text = monitor.LastRequest.ToString();
        //    _menuLastChecked.Visible = true;

        //    _menuLastMessage.Text = "Tracking " + _monitors.Count + " towns";
        //    _menuLastMessage.Visible = true;
        //}

        private void MonitorOnOnStateChanged(ServerMonitor monitor)
        {
            switch (monitor.CurrentState)
            {
                case ServerMonitorState.None:
                    throw new ArgumentOutOfRangeException("serverMonitorState");
                case ServerMonitorState.Running:
                    Notify("Monitoring", "Monitoring of " + monitor.ServerConfiguration.DisplayString + " has started");
                    break;
                case ServerMonitorState.Canceled:
                    Notify("Monitoring", "Monitoring of " + monitor.ServerConfiguration.DisplayString + " has been canceled");
                    break;
                case ServerMonitorState.Crashed:
                    Notify("Monitoring", "Monitoring of " + monitor.ServerConfiguration.DisplayString + " has crashed!");
                    SoundHelper.PlayNotification();
                    break;
                case ServerMonitorState.Finished:
                    Notify("Monitoring", "Monitoring of " + monitor.ServerConfiguration.DisplayString + " has stopped");
                    SoundHelper.PlayNotification();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("serverMonitorState");
            }
        }

        private void MonitorOnOnTaskFinished(TownConfiguration townConfiguration, TownActionTypes townAction, string tooltip)
        {
            Notify("Task Finished - " + townConfiguration.Name, tooltip);
            SoundHelper.PlayNotification();

            // History
            AddHistory(townConfiguration, EventHistory.EventHistoryType.TaskFinished, townAction);
        }

        private void MonitorOnOnTaskDiscovered(TownConfiguration townConfiguration, TownActionTypes townAction)
        {
            Notify("Task Discovered - " + townConfiguration.Name, townAction.ToString());
            SoundHelper.PlayNotification();

            // History
            AddHistory(townConfiguration, EventHistory.EventHistoryType.TaskDiscovered, townAction);
        }

        private void AddHistory(TownConfiguration townConfiguration, EventHistory.EventHistoryType type, TownActionTypes actionType)
        {
            Task.Factory.StartNew(() =>
            {
                _history.Add(new EventHistory(type, townConfiguration, actionType));

                while (_history.Count > MaxHistory)
                    _history.RemoveAt(0);
            }, CancellationToken.None, TaskCreationOptions.None, _uiScheduler);
        }

        private ContextMenu BuildContextMenu()
        {
            _menuLastMessage = new MenuItem();
            _menuLastMessage.Text = "LastMessage";
            _menuLastMessage.Visible = false;

            _menuLastChecked = new MenuItem();
            _menuLastChecked.Text = "LastCheck";
            _menuLastChecked.Visible = false;

            _menuConfigure = new MenuItem();
            _menuConfigure.Text = "Configure";
            _menuConfigure.Click += ConfigureClick;

            _menuExit = new MenuItem();
            _menuExit.Text = "Exit";
            _menuExit.Click += ExitClick;

            return new ContextMenu(new[] { _menuLastChecked, _menuLastMessage, _menuConfigure, _menuExit });
        }

        private void ConfigureClick(object sender, EventArgs eventArgs)
        {
            ShowConfigureWindow();
        }

        private void ShowConfigureWindow()
        {
            Configuration editableConfiguration = new Configuration(_configuration);

            ConfigureForm configure = new ConfigureForm(editableConfiguration);
            TrackForm(configure);
            DialogResult result = configure.ShowDialog();

            if (result == DialogResult.OK)
            {
                _configuration.Clone(editableConfiguration);

                try
                {
                    _configuration.Save(_settingsFile.FullName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unable to save the settings file 'config.xml'." + Environment.NewLine + ex.Message +
                        Environment.NewLine + _settingsFile.FullName, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Reload
                Initialize();
            }
        }

        private void ExitClick(object sender, EventArgs eventArgs)
        {
            _subForms.ForEach(s => s.Close());

            CancelAllMonitors(true);
            Application.Exit();
        }

        private void Notify(string title, string text, int time = 2000)
        {
            notify.BalloonTipTitle = title;
            notify.BalloonTipText = text;
            notify.ShowBalloonTip(time);
        }

        private void TrackForm(Form form)
        {
            _subForms.RemoveAll(s => s.IsDisposed);
            _subForms.Add(form);
        }
    }
}
