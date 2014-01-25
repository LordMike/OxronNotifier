using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OxronNotifier.Model;
using OxronNotifier.OxronApi;
using OxronNotifier.Utility;

namespace OxronNotifier
{
    public partial class ConfigureForm : Form
    {
        private Configuration _configuration;
        private BindingList<ServerConfiguration> _configList;

        public ConfigureForm(Configuration configuration)
        {
            InitializeComponent();

            _configuration = configuration;

            _configList = new BindingList<ServerConfiguration>(configuration.Servers);
            cmbConfigs.DataSource = _configList;

            cmbConfigs.DisplayMember = "DisplayString";

            if (!_configuration.Servers.Any())
            {
                AddNewServer();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Display(ServerConfiguration configuration)
        {
            cmdTest.Enabled = false;

            txtApiKey.ResetBindings();
            txtServerKey.ResetBindings();
            txtServer.ResetBindings();
            txtUsername.ResetBindings();

            if (configuration != null)
            {
                cmdTest.Enabled = true;

                txtApiKey.BindText(configuration, serverConfiguration => serverConfiguration.ApiKeyString);
                txtServerKey.BindText(configuration, serverConfiguration => serverConfiguration.ServerKeyString);
                txtServer.BindText(configuration, serverConfiguration => serverConfiguration.ServerAddress);
                txtUsername.BindText(configuration, serverConfiguration => serverConfiguration.Username);
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            AddNewServer();
        }

        private void AddNewServer()
        {
            ServerConfiguration newConfig = new ServerConfiguration();
            newConfig.Username = "(username)";
            newConfig.ApiKey = Guid.Empty;
            newConfig.ServerAddress = "(server)";

            _configList.Add(newConfig);

            Display(newConfig);
        }

        private void cmbConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServerConfiguration configuration = (ServerConfiguration)cmbConfigs.SelectedItem;

            Display(configuration);
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            ServerConfiguration configuration = (ServerConfiguration)cmbConfigs.SelectedItem;

            DialogResult result = MessageBox.Show("Are you sure you want to delete " + configuration.DisplayString + "?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _configList.Remove(configuration);
                Display(_configList.FirstOrDefault());
            }
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            Enabled = false;
            try
            {
                ServerConfiguration serverConfiguration = (ServerConfiguration)cmbConfigs.SelectedItem;
                ApiProxy proxy = new ApiProxy(serverConfiguration);

                proxy.GetTowns();

                // Success
                MessageBox.Show("The provided settings were validated!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to validate settings, please double-check." + Environment.NewLine + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Enabled = true;
            }
        }
    }
}