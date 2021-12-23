using System.Collections.Immutable;
using System.Net;
using Newtonsoft.Json;

namespace TrcAspEguiL2.Models;

public class ActivitiesRepository
{
    [JsonProperty("activities")] public IList<Activity> Activities { get; set; }


    public ActivitiesRepository()
    {
        this.Activities = new List<Activity>();
    }
    
    public static ActivitiesRepository GetEntriesObjectFromJson(string jsonString)
    {
        var dataObject = JsonConvert.DeserializeObject<ActivitiesRepository>(jsonString);

        if (dataObject is null)
        {
            return new ActivitiesRepository();
        }

        return dataObject;
    }
    
    public List<string> GetActivitiesCodes()
    {
        var codes = new List<string>();
        foreach (var activity in Activities)
        {
            codes.Add(activity.Code);
        }
        return codes;
    }

    public Activity GetProjectByCode(string code)
    {
        foreach (var activity in Activities)
        {
            if (activity.Code == code)
            {
                return activity;
            }
        }

        return new Activity();
    }

}