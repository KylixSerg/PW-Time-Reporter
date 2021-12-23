using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Newtonsoft.Json;
using TrcAspEguiL2.Models;



namespace TrcAspEguiL2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string userName)
        {
            //save user name throughout the session
            SessionUser.UserName = userName;
            SessionUser.LoginDate = DateTime.Today.Date.ToString("dd-MM-yyyy");
            //create directory for user, function will not create it if it exists
            Directory.CreateDirectory($@"JsonFiles/{SessionUser.UserName}");
            //create file where to hold user entries for today if it does not exist
            var filePath =
                $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + SessionUser.LoginDate}.json";
            SessionUser.TodayDataFilePath = filePath;
            // get all possible codes
            SessionUser.AvailableActivityCodes = ActivitiesRepository
                .GetEntriesObjectFromJson(System.IO.File.ReadAllText("JsonFiles/Activities.json"))
                .GetActivitiesCodes();
            if (!System.IO.File.Exists(filePath))
            {
                var file = System.IO.File.Create(filePath);
                file.Close();
            }


            return View();
        }

        [Route("/Home/MyActivities/{date}")]
        public IActionResult MyActivities(string date)
        {
            ViewBag.date = date;
            var filePath = $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + date}.json";
            if (!System.IO.File.Exists(filePath))
            {
                var file = System.IO.File.Create(filePath);
                file.Close();
            }

            // get all the activities of the user 
            var jsonString = System.IO.File.ReadAllText(filePath);

            var myActivitiesList = EntriesRepository.GetEntriesObjectFromJson(jsonString);
            return View(myActivitiesList);
        }


        [HttpGet]
        public IActionResult AddActivity()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddActivity(Entry userInput)
        {
            // get the date that the user selected to add activity
            var date = userInput.date;
            // create the json for that date if it does not exist
            SessionUser.AddedDataFilePath =
                $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + date.Date.ToString("dd-MM-yyyy")}.json";
            if (!System.IO.File.Exists(SessionUser.AddedDataFilePath))
            {
                var file = System.IO.File.Create(SessionUser.AddedDataFilePath);
                file.Close();
            }

            // get user json string  for today data
            var jsonString = System.IO.File.ReadAllText(SessionUser.AddedDataFilePath);
            // create new placeholder for the user activities
            var entriesPlaceHolder = EntriesRepository.GetEntriesObjectFromJson(jsonString);

            entriesPlaceHolder.AddEntry(userInput);


            // save the data to the json again 
            entriesPlaceHolder.SaveEntriesObjectToJson(SessionUser.AddedDataFilePath);

            // prepare date for redirecting
            var stringifiedDate = date.Date.ToString("dd-MM-yyyy");

            return Redirect($"/Home/MyActivities/{stringifiedDate}");
        }

        [Route("Home/DeleteActivity/{date}/{id}")]
        public IActionResult DeleteActivity(string date, int id)
        {
            // get user json string  for today data
            var jsonString =
                System.IO.File.ReadAllText(
                    $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + date}.json");
            // create new placeholder for the user activities
            var entriesPlaceHolder = EntriesRepository.GetEntriesObjectFromJson(jsonString);
            //delete the activity form the repo, id is safe
            entriesPlaceHolder.RemoveEntry(id);

            // save the data to the json again 
            entriesPlaceHolder.SaveEntriesObjectToJson(
                $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + date}.json");


            return Redirect($"/Home/MyActivities/{date}");
        }

        [Route("Home/ActivityDetails/{date}/{id}")]
        public IActionResult ActivityDetails(string date, int id)
        {
            // get user json string  for today data
            var jsonString =
                System.IO.File.ReadAllText(
                    $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + date}.json");
            var jsonStringActivities = System.IO.File.ReadAllText($"JsonFiles/Activities.json");
            // create new placeholder for the user activities
            var entriesPlaceHolder = EntriesRepository.GetEntriesObjectFromJson(jsonString);
            var activitiesPlaceHolder = ActivitiesRepository.GetEntriesObjectFromJson(jsonStringActivities);
            // get the requested entry
            var requestedEntry = entriesPlaceHolder.GetEntryById(id);
            // get the corresponding code for the entry
            var requestedEntryProject = activitiesPlaceHolder.GetProjectByCode(requestedEntry.code);
            // pass the project details in the view bag
            ViewBag.projectName = requestedEntryProject.Name;
            ViewBag.projectCode = requestedEntryProject.Code;
            ViewBag.projectBudget = requestedEntryProject.Budget;
            ViewBag.projectManager = requestedEntryProject.Manager;


            return View(requestedEntry);
        }

        [HttpGet]
        public IActionResult EditActivity()
        {
            
            return View();
        }

        [HttpPost]
        [Route("/Home/EditActivity/{id:int?}")]
        public IActionResult EditActivity(Entry userInput, int id)
        {
            // get the date that the user selected to add activity
            var date = userInput.date;
            // create the json for that date if it does not exist
            SessionUser.AddedDataFilePath =
                $"JsonFiles/{SessionUser.UserName}/{SessionUser.UserName + "-" + date.Date.ToString("dd-MM-yyyy")}.json";
            if (!System.IO.File.Exists(SessionUser.AddedDataFilePath))
            {
                var file = System.IO.File.Create(SessionUser.AddedDataFilePath);
                file.Close();
            }

            // get user json string  for today data
            var jsonString = System.IO.File.ReadAllText(SessionUser.AddedDataFilePath);
            // create new placeholder for the user activities
            var entriesPlaceHolder = EntriesRepository.GetEntriesObjectFromJson(jsonString);

            entriesPlaceHolder.ReplaceEntryById(id, userInput);


            // save the data to the json again 
            entriesPlaceHolder.SaveEntriesObjectToJson(SessionUser.AddedDataFilePath);

            // prepare date for redirecting
            var stringifiedDate = date.Date.ToString("dd-MM-yyyy");

            return Redirect($"/Home/MyActivities/{stringifiedDate}");
        }

        [Route("Home/MonthlyReport/{id}")]
        public IActionResult MonthlyReport(int id)
        {
            // go through all the files of the month id=month in this case
            
            
            return View(new FileReader(id));
        }


        public IActionResult Logout()
        {
            SessionUser.ResetUser();
            return Redirect("/");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
                {RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}