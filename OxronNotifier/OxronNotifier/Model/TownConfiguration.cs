using System;
using System.ComponentModel;
using System.Xml.Serialization;
using OxronNotifier.Annotations;

namespace OxronNotifier.Model
{
    public class TownConfiguration : INotifyPropertyChanged
    {
        private string _name;
        private int _locationX;
        private int _locationY;
        private bool _isResearchTown;
        private Guid _townId;

        [ReadOnly(true)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [ReadOnly(true)]
        public Guid TownId
        {
            get { return _townId; }
            set
            {
                if (value == _townId)
                    return;

                _townId = value;
                OnPropertyChanged("TownId");
            }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        public int LocationX
        {
            get { return _locationX; }
            set
            {
                if (value == _locationX)
                    return;

                _locationX = value;
                OnPropertyChanged("LocationX");
            }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        public int LocationY
        {
            get { return _locationY; }
            set
            {
                if (value == _locationY)
                    return;

                _locationY = value;
                OnPropertyChanged("LocationY");
            }
        }

        public bool IsResearchTown
        {
            get { return _isResearchTown; }
            set
            {
                if (value.Equals(_isResearchTown))
                    return;

                _isResearchTown = value;
                OnPropertyChanged("IsResearchTown");
            }
        }

        [XmlIgnore]
        [ReadOnly(true)]
        public string LocationString { get { return LocationX + ", " + LocationY; } }

        public TownConfiguration()
        {
        }

        public TownConfiguration(TownConfiguration other)
        {
            Clone(other);
        }

        public void Clone(TownConfiguration other)
        {
            if (other == null)
                return;

            TownId = other.TownId;
            Name = other.Name;
            LocationX = other.LocationX;
            LocationY = other.LocationY;
            IsResearchTown = other.IsResearchTown;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}