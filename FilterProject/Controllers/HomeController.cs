using Microsoft.AspNetCore.Mvc;



namespace FilterProject.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Filter(string stringOfNumbers)
    {
        var result = string.Empty;
        var data = new string[]{};
        if (stringOfNumbers.StartsWith("[") && stringOfNumbers.EndsWith("]") && !stringOfNumbers.Contains(" "))
        {
            data = stringOfNumbers.Remove(stringOfNumbers.Length - 1, 1).Remove(0, 1).Trim().Split(',');
            if (Array.TrueForAll(data, IsNumber))
            {
                var dict = from x in data
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new {KeyValue = g.Key, Count = count};
                var listOfNumbers = new List<int>();
                foreach (var value in dict)
                {
                    if (value.Count >= 3)
                    {
                        listOfNumbers.Add(Int32.Parse(value.KeyValue));
                    }
                }

                result = String.Format("[{0}]",string.Join(",", listOfNumbers.OrderByDescending(x => x)));
            }
            else
            {
                result = "Error";
            }
        }
        else
        {
            result = "Error";
        }

        TempData["Result"] = result;
        return View("Index");
    }
    private static bool IsNumber(string KeyValue)
    {
        int s;
        var resultOfParse = int.TryParse(KeyValue, out s);
        return resultOfParse;
    }
}