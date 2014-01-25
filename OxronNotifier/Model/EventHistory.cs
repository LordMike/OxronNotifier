using System;
using OxronNotifier.OxronApi;

namespace OxronNotifier.Model
{
    public class EventHistory
    {
        public TownConfiguration  TownConfiguration { get; set; }
        public EventHistoryType Type { get; set; }
        public DateTime EventTime { get; set; }
        public TownActionTypes TownActionType { get; set; }

        public EventHistory(EventHistoryType type, TownConfiguration townConfiguration, TownActionTypes actionType)
        {
            TownConfiguration = townConfiguration;
            Type = type;
            EventTime = DateTime.Now;
            TownActionType = actionType;
        }

        public enum EventHistoryType
        {
            TaskDiscovered,
            TaskFinished
        }
    }
}