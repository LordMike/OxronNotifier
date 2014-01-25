using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OxronNotifier.Model;
using OxronNotifier.OxronApi;

namespace OxronNotifier
{
    public class ApiProxy
    {
        private ServerConfiguration _serverConfiguration;

        public ApiProxy(ServerConfiguration serverConfiguration)
        {
            _serverConfiguration = serverConfiguration;
        }

        public List<OxronApiTown> GetTowns()
        {
            OxronApiSoapClient client = CreateClient();

            // Contact
            OxronApiResultTown towns = client.GetTownList(_serverConfiguration.ServerKey, _serverConfiguration.ApiKey, _serverConfiguration.Username);

            if (!towns.Validation.IsSuccessful)
                throw new Exception("Unable to get towns list: " + towns.Validation.ErrorMessage);

            return towns.Towns;
        }

        public List<OxronApiEvent> GetTownInfo(Guid townId)
        {
            OxronApiSoapClient client = CreateClient();

            // Contact
            OxronApiResultEvent events = client.GetTownActionList(_serverConfiguration.ServerKey, _serverConfiguration.ApiKey, _serverConfiguration.Username, townId);

            if (!events.Validation.IsSuccessful)
                throw new Exception("Unable to get towns list: " + events.Validation.ErrorMessage);

            return events.Events;
        }

        private OxronApiSoapClient CreateClient()
        {
            OxronApiSoapClient client = new OxronApiSoapClient();
            client.Endpoint.Address = new EndpointAddress(new Uri(new Uri((_serverConfiguration.ServerAddress.Contains("://") ? "" : "http://") + _serverConfiguration.ServerAddress), "/OxronApi.asmx"));
            
            return client;
        }
    }
}
