using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using OxronNotifier.Annotations;

namespace OxronNotifier.Model
{
    public class ServerConfiguration : INotifyPropertyChanged
    {
        private string _serverAddress;
        private string _username;
        private Guid _apiKey;
        private Guid _serverKey;

        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                if (value == _serverAddress)
                    return;

                _serverAddress = value;
                OnPropertyChanged();
            }
        }

        public Guid ServerKey
        {
            get { return _serverKey; }
            set
            {
                if (value == _serverKey)
                    return;

                _serverKey = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (value == _username)
                    return;

                _username = value;
                OnPropertyChanged();
            }
        }

        public Guid ApiKey
        {
            get { return _apiKey; }
            set
            {
                if (value == _apiKey)
                    return;

                _apiKey = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public string ApiKeyString
        {
            get { return ApiKey.ToString(); }
            set
            {
                Guid guid;
                if (Guid.TryParse(value, out guid))
                    ApiKey = guid;
            }
        }

        [XmlIgnore]
        public string ServerKeyString
        {
            get { return ServerKey.ToString(); }
            set
            {
                Guid guid;
                if (Guid.TryParse(value, out guid))
                    ServerKey = guid;
            }
        }

        [XmlIgnore]
        public string DisplayString
        {
            get { return ServerAddress + " " + Username; }
        }

        public ServerConfiguration()
        {
        }

        public ServerConfiguration(ServerConfiguration other)
        {
            Clone(other);
        }

        public void Clone(ServerConfiguration other)
        {
            if (other == null)
                return;

            ServerAddress = other.ServerAddress;
            ServerKey = other.ServerKey;
            Username = other.Username;
            ApiKey = other.ApiKey;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}