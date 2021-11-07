using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyApp.Class;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "don't use";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "My contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Send(string link)
        {
            if (!link.ExitURL())
            {
                ViewData["Message"] = $"Link ({link}) doesn't exist";
                
            }
            else ViewData["Message"] = $"Link ({link}) exists";

            WorkWeb ww = new WorkWeb(link);
            //ww.pro();
            ww.Analiz();
            return View();
        }
    }
}
