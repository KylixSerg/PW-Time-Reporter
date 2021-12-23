using Newtonsoft.Json;

namespace TrcAspEguiL2.Models;

public class Activity
{
    [JsonProperty("code")] public string Code { get; set; }
    [JsonProperty("manager")] public string Manager { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("budget")] public string Budget { get; set; }
    

   
}