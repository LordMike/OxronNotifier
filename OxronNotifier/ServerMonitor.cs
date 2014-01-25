using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OxronNotifier.Model;
using OxronNotifier.OxronApi;

namespace OxronNotifier
{
    public enum ServerMonitorState
    {
        None,
        Running,
        Canceled,
        Crashed,
        Finished
    }

    public class ServerMonitor
    {
        private CancellationTokenSource _tokenSource;
        private Task _task;
        private ServerMonitorState _currentState;
        private ApiProxy _proxy;

        private List<TownConfiguration> _towns;
        private Dictionary<Guid, List<OxronApiEvent>> _lastActivities;

        private static TimeSpan _updateTownDefInterval = TimeSpan.FromSeconds(45);
        private static TimeSpan _updateTownMinInterval = TimeSpan.FromSeconds(5);
        private static TimeSpan _updateTownMaxInterval = TimeSpan.FromMinutes(1);
        private static TimeSpan _updateTownListInterval = TimeSpan.FromHours(1);

        public ServerConfiguration ServerConfiguration { get; private set; }

        public DateTime LastRequest { get; private set; }
        public DateTime NextRequest { get; private set; }
        public DateTime NextTownListUpdate { get; private set; }

        public ServerMonitorState CurrentState
        {
            get { return _currentState; }
            private set
            {
                if (value == _currentState)
                    return;

                _currentState = value;
                SendOnStateChanged(value);
            }
        }

        public event Action<TownConfiguration, TownActionTypes, string> OnTaskFinished;
        public event Action<TownConfiguration, TownActionTypes> OnTaskDiscovered;

        public event Action<ServerMonitorState> OnStateChanged;

        public ServerMonitor(ServerConfiguration serverConfiguration)
        {
            _lastActivities = new Dictionary<Guid, List<OxronApiEvent>>();
            ServerConfiguration = serverConfiguration;

            CurrentState = ServerMonitorState.None;

            LastRequest = DateTime.MinValue;
            NextRequest = DateTime.MinValue;
            NextTownListUpdate = DateTime.MinValue;
        }

        public void StartAsync()
        {
            if (_task != null)
                throw new ArgumentException("Already started");

            _tokenSource = new CancellationTokenSource();
            _task = Task.Factory.StartNew(Run, _tokenSource.Token).ContinueWith(FollowUp);

            _proxy = new ApiProxy(ServerConfiguration);

            CurrentState = ServerMonitorState.Running;
        }

        private void FollowUp(Task obj)
        {
            // Check if task crashed or was canceled
            if (obj.IsCanceled)
                CurrentState = ServerMonitorState.Canceled;
            else if (obj.IsFaulted)
                CurrentState = ServerMonitorState.Crashed;
            else
                CurrentState = ServerMonitorState.Finished;
        }

        private void Run()
        {
            while (true)
            {
                _tokenSource.Token.ThrowIfCancellationRequested();

                if (DateTime.UtcNow >= NextTownListUpdate)
                {
                    // Update towns list
                    try
                    {
                        UpdateTowns();
                    }
                    catch (Exception)
                    {
                        // Failed, try again
                        Thread.Sleep(2000);

                        continue;
                    }

                    // Succeeded
                    NextTownListUpdate = DateTime.UtcNow + _updateTownListInterval;
                }

                // Check if we should update the towns
                if (DateTime.UtcNow <= NextRequest)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                TimeSpan nextRequestTime = _updateTownDefInterval;

                foreach (TownConfiguration townConfiguration in _towns)
                {
                    TimeSpan tmpSpan;
                    ProcessTownEvents(townConfiguration, out tmpSpan);

                    if (tmpSpan < nextRequestTime)
                        nextRequestTime = tmpSpan;
                }

                LastRequest = DateTime.UtcNow;
                NextRequest = DateTime.UtcNow + nextRequestTime;
            }
        }

        private void ProcessTownEvents(TownConfiguration town, out TimeSpan nextRequestTime)
        {
            // Fetch
            List<OxronApiEvent> activities;
            try
            {
                activities = _proxy.GetTownInfo(town.TownId);
            }
            catch (Exception ex)
            {
                // TODO: send error to UI
                Debug.WriteLine(ex.Message);

                nextRequestTime = _updateTownMinInterval + _updateTownMinInterval;
                NextRequest = DateTime.UtcNow + nextRequestTime;

                return;
            }

            // Filter out research
            if (!town.IsResearchTown)
                activities.RemoveAll(s => s.TownActionType == TownActionTypes.Research);

            // Process
            bool anythingFinished = false;

            // Detect all old events that dissapeared
            List<OxronApiEvent> newEventsTracking = activities.ToList();
            List<OxronApiEvent> lastActivities;
            if (!_lastActivities.TryGetValue(town.TownId, out lastActivities))
                lastActivities = new List<OxronApiEvent>();

            foreach (OxronApiEvent old in lastActivities)
            {
                OxronApiEvent newCorresponding = activities.SingleOrDefault(s => s.TownActionType == old.TownActionType);

                if (newCorresponding == null)
                {
                    // The last task has finished
                    SendOnTaskFinished(town, old.TownActionType, old.ToolTip);
                    anythingFinished = true;
                }
                else
                {
                    newEventsTracking.Remove(newCorresponding);

                    // Still there
                    if (old.Count > newCorresponding.Count)
                    {
                        // One or more tasks finished
                        SendOnTaskFinished(town, old.TownActionType, old.ToolTip);
                        anythingFinished = true;
                    }
                    else if (old.Count < newCorresponding.Count)
                    {
                        // New task discovered
                        SendOnTaskDiscovered(town, old.TownActionType);
                    }
                }
            }

            // Display brand new events
            foreach (OxronApiEvent oxronApiEvent in newEventsTracking)
            {
                SendOnTaskDiscovered(town, oxronApiEvent.TownActionType);
            }

            // Update
            _lastActivities[town.TownId] = activities;

            // Calculate next queue time
            if (anythingFinished)
            {
                // Queue as soon as possible
                nextRequestTime = TimeSpan.MinValue;
            }
            else if (activities.Count == 0)
            {
                // Queue as late as possible
                nextRequestTime = TimeSpan.MaxValue;
            }
            else
            {
                // Queue to check the next event, when it finished
                int firstEvent = activities.Select(s => s.TimeLeft).OrderBy(s => s).FirstOrDefault();
                nextRequestTime = TimeSpan.FromSeconds(firstEvent + 5);
            }

            // Queue next check
            if (nextRequestTime < _updateTownMinInterval)
                nextRequestTime = _updateTownMinInterval;
            else if (nextRequestTime > _updateTownMaxInterval)
                nextRequestTime = _updateTownMaxInterval;
        }

        private void UpdateTowns()
        {
            List<OxronApiTown> towns = _proxy.GetTowns();

            _towns = new List<TownConfiguration>();
            bool isFirst = true;
            foreach (OxronApiTown town in towns)
            {
                _towns.Add(new TownConfiguration()
                {
                    LocationX = town.X,
                    LocationY = town.Y,
                    TownId = town.TownId,
                    IsResearchTown = isFirst,
                    Name = town.Name
                });

                isFirst = false;
            }
        }

        public void Stop(bool waitForExit)
        {
            _tokenSource.Cancel();

            if (waitForExit)
            {
                try
                {
                    _task.Wait();
                }
                catch (AggregateException ex)
                {
                    // Should be an OperationCanceledException (and only one)
                    if (!(ex.InnerExceptions.Count == 1 && ex.InnerExceptions.First() is OperationCanceledException))
                        throw;
                }
            }
        }

        protected virtual void SendOnTaskFinished(TownConfiguration townConfiguration, TownActionTypes type, string tooltip)
        {
            Action<TownConfiguration, TownActionTypes, string> handler = OnTaskFinished;
            if (handler != null)
                handler(townConfiguration, type, tooltip);
        }

        protected virtual void SendOnStateChanged(ServerMonitorState obj)
        {
            Action<ServerMonitorState> handler = OnStateChanged;
            if (handler != null) handler(obj);
        }

        protected virtual void SendOnTaskDiscovered(TownConfiguration townConfiguration, TownActionTypes arg2)
        {
            Action<TownConfiguration, TownActionTypes> handler = OnTaskDiscovered;
            if (handler != null)
                handler(townConfiguration, arg2);
        }
    }
}