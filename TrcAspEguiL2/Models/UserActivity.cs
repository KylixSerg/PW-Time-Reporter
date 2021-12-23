namespace TrcAspEguiL2.Models;

public class UserActivity
{
    public string Name { get; set; }
    public int TimeSpent { get; set; }
    
    public  UserActivity(){}

    public UserActivity(string name, int timeSpent)
    {
        this.Name = name;
        this.TimeSpent = timeSpent;
    }

    public UserActivity(UserActivity newActivity)
    {
        this.Name = newActivity.Name;
        this.TimeSpent = newActivity.TimeSpent;
    }
    
}