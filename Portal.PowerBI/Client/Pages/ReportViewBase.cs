using Microsoft.AspNetCore.Components;
using Portal.PowerBI.Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Portal.PowerBI.Client.Pages
{
    public class ReportViewBase : ComponentBase
    {


        [Inject]
        HttpClient client { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }
        public Components cmp { get; set; } = new Components();


        [Parameter]
        public Guid Id { get; set; }

        protected async override void OnInitialized()
        {
            var httpreq = await client.PostAsJsonAsync("api/Component/ComponentData", Id);

            if (httpreq.IsSuccessStatusCode)
            {
                cmp = await httpreq.Content.ReadFromJsonAsync<Components>();

            }

            //navigationManager.LocationChanged += LocationChanged;
            base.OnInitialized();
        }

        protected async override void OnAfterRender(bool asd)
        {


            var httpreq = await client.PostAsJsonAsync("api/Component/ComponentData", Id);

            if (httpreq.IsSuccessStatusCode)
            {
                cmp = await httpreq.Content.ReadFromJsonAsync<Components>();

            }

        }
    }
}
