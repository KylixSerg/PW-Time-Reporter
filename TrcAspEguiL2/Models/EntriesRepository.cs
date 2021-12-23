using System.Net;
using Newtonsoft.Json;

namespace TrcAspEguiL2.Models;

public class EntriesRepository
{
    [JsonProperty("entries")] public IList<Entry> Entries { get; set; }

    public EntriesRepository()
    {
        this.Entries = new List<Entry>();
    }

    public static EntriesRepository GetEntriesObjectFromJson(string jsonString)
    {
        var dataObject = JsonConvert.DeserializeObject<EntriesRepository>(jsonString);

        if (dataObject is null)
        {
            return new EntriesRepository();
        }

        return dataObject;
    }

    public void SaveEntriesObjectToJson(string filePath)
    {
        var entriesJson = JsonConvert.SerializeObject(this);
        File.WriteAllText(filePath, entriesJson);
    }

    public void AddEntry(Entry entry)
    {
        Entries.Add(entry);
    }

    public void RemoveEntry(int index)
    {
        Entries.RemoveAt(index);
    }

    public Entry GetEntryById(int index)
    {
        return Entries[index];
    }

    public void ReplaceEntryById(int index, Entry newEntry)
    {
        Entries[index] = newEntry;
    }

    public IDictionary<string, int> GetUserTotalTimeOnEachProject()
    {
        var activitiesBreakDown = new Dictionary<string, int>();
        // create dict with all the project associated with user and initialize their time to 0
        foreach (var entry in Entries)
        {
            if (!activitiesBreakDown.ContainsKey(entry.code))
            {
                activitiesBreakDown.Add(entry.code, entry.time);
            }
            // if project is already in dict increment the time 
            else
            {
                activitiesBreakDown[entry.code] += entry.time;
            }
        }

        return activitiesBreakDown;
    }
}