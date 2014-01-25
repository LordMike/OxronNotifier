using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using OxronNotifier.Annotations;
using System.Runtime.CompilerServices;

namespace OxronNotifier.Model
{
    public class Configuration : INotifyPropertyChanged
    {
        private List<ServerConfiguration> _servers;

        public List<ServerConfiguration> Servers
        {
            get { return _servers; }
            set
            {
                if (Equals(value, _servers))
                    return;

                _servers = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Configuration()
        {
            Servers = new List<ServerConfiguration>();
        }

        public Configuration(Configuration other)
        {
            Servers = new List<ServerConfiguration>();
            Clone(other);
        }

        public void Clone(Configuration other)
        {
            if (other == null)
                return;

            Servers = other.Servers.Select(s => new ServerConfiguration(s)).ToList();
        }

        public void Load(string path)
        {
            Configuration configuration;

            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            using (FileStream fs = File.OpenRead(path))
                configuration = serializer.Deserialize(fs) as Configuration;

            Clone(configuration);
        }

        public void Save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            using (FileStream fs = File.Open(path, FileMode.Create))
                serializer.Serialize(fs, this);
        }
    }
}