using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp_usrmsi_mgmtapi_dotnet.Models
{
    public class ResourceViewModelSet
    {
        [JsonProperty("value")]
        public List<ResourceViewModel> Values { get; set; }
    }
}
