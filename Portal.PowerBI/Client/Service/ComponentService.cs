using Portal.PowerBI.Client;
using Portal.PowerBI.Server.Data.Context;
using Portal.PowerBI.Server.Data.Model;
using Portal.PowerBI.Shared.Model;
using Portal.PowerBI.Shared.ResponseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Portal.PowerBI.Server.Services
{
    public class ComponentServiceasd : IComponentServiceasd
    {
        private readonly HttpClient client;

        public ComponentServiceasd(HttpClient _client)
        {
            client = _client;
        }


        async Task<Components> IComponentServiceasd.GetComponent(string Id)
        {
            var httpreq = await client.PostAsJsonAsync("api/Component/Component",Id);

            var res = await httpreq.Content.ReadFromJsonAsync<Components>();

            return res;
        }

    }
}
