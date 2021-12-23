namespace TrcAspEguiL2.Models;

public static class SessionUser
{
    /// <summary>
    /// Global variable storing user name.
    /// </summary>
    public static string UserName;

    public static string TodayDataFilePath;

    public static string AddedDataFilePath;

    public static string LoginDate;

    public static List<string> AvailableActivityCodes;

    public static void ResetUser()
    {
        UserName = "";
        TodayDataFilePath = "";
        AvailableActivityCodes = new List<string>();
        LoginDate = "";
    }


}