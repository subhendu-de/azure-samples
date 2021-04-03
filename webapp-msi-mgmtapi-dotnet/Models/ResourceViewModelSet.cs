using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webapp_msi_mgmtapi_dotnet.Models
{
    public class ResourceViewModelSet
    {
        [JsonProperty("value")]
        public List<ResourceViewModel> Values { get; set; }
    }
}
