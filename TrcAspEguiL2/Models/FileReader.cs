namespace TrcAspEguiL2.Models;

public  class FileReader
{
    public int Month { get; set; }

    public FileReader(int month)
    {
        this.Month = month;
    }
    public  IDictionary<string, int> GetMonthProjectTime()
    {
        var resultDict = new Dictionary<string, int>();
        
        for (var i = 1; i <= 31; i++)
        {
            if (i < 10)
            {
                // get user json string  for today data
                var filePath = $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-0" + i}-{this.Month}-2021.json";
                if (!File.Exists(filePath))
                {
                    continue;
                }

                var jsonString = System.IO.File.ReadAllText(filePath);

                var jsonStringActivities = System.IO.File.ReadAllText($"JsonFiles/Activities.json");
                // create new placeholder for the user activities
                var entriesPlaceHolder = EntriesRepository.GetEntriesObjectFromJson(jsonString);
                var resultDict1 = new Dictionary<string, int>(entriesPlaceHolder.GetUserTotalTimeOnEachProject());
                foreach (var item in resultDict1)
                {
                    if (!resultDict.ContainsKey(item.Key))
                    {
                        resultDict.Add(item.Key, item.Value);
                    }
                    else
                    {
                        resultDict[item.Key] = item.Value;
                    }
                    
                }
            }
            else
            {
                // get user json string  for today data
                var filePath = $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + i}-{this.Month}-2021.json";
                if (!File.Exists(filePath))
                {
                    continue;
                }

                var jsonString = System.IO.File.ReadAllText(filePath);
                var jsonStringActivities = System.IO.File.ReadAllText($"JsonFiles/Activities.json");
                // create new placeholder for the user activities
                var entriesPlaceHolder = EntriesRepository.GetEntriesObjectFromJson(jsonString);
                var resultDict2 = new Dictionary<string, int>(entriesPlaceHolder.GetUserTotalTimeOnEachProject());
                foreach (var item in resultDict2)
                {
                    if (!resultDict.ContainsKey(item.Key))
                    {
                        resultDict.Add(item.Key, item.Value);
                    }
                    else
                    {
                        resultDict[item.Key] = item.Value;
                    }
                    
                }
            }
        }

        return resultDict;

    }
}