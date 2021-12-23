using Newtonsoft.Json;

namespace TrcAspEguiL2.Models;

public class Entry
{
    [JsonProperty("date")]
    public DateTime date { get; set; }
    [JsonProperty("code")]
    public string code { get; set; }
    [JsonProperty("time")]
    public int time { get; set; }
    [JsonProperty("description")]
    public string description { get; set; }
    
    public Entry(){}

    public Entry(Entry entry)
    {
        this.date = entry.date;
        this.code = entry.code;
        this.time = entry.time;
        this.description = entry.description;
    }
}