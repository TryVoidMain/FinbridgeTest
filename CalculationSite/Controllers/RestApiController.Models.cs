using CalculationSite.Types.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CalculationSite.Controllers
{
    public partial class RestApiController : ControllerBase
    {
        public class ApiError
        {
            [JsonProperty(nameof(ErrorMessage))]
            public string ErrorMessage { get; set; }
        }

        public class ApiCalculateRequest : ICalculateValues
        {
            [JsonProperty(nameof(Values))]
            public int[] Values { get; set; }
        }

        public class ApiCalculateResponse 
        {
            [JsonProperty(nameof(Result))]
            public int Result { get; set; }
        }
    }
}
