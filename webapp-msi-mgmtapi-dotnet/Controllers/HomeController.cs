using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using webapp_msi_mgmtapi_dotnet.Models;
using Azure.Identity;
using Azure.Core;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace webapp_msi_mgmtapi_dotnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            // authenticate using managed identity if it is available otherwise use the Azure CLI to auth
            // please refer https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme for more details
            var credential = new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential());

            var _subscriptionId = _configuration["SubscriptionId"];
            var _resourceGroup = _configuration["ResourceGroup"];
            ViewData["ResourceGroup"] = _resourceGroup;

            var url = $"https://management.azure.com/subscriptions/{_subscriptionId}/resourceGroups/{_resourceGroup}/resources?api-version=2020-10-01";
            var accessToken = await credential.GetTokenAsync(new TokenRequestContext(new string[] { "https://management.azure.com/" }));

            string response;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);
                response = await client.GetStringAsync(url);
            }

            var resources = JsonConvert.DeserializeObject<ResourceViewModelSet>(response);
            return View(resources);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
