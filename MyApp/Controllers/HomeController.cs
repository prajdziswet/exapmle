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

        public IActionResult Group()
        {
            return View("Group");
        }

        public IActionResult ShowGroup(string domain)
        {
            ViewData["domain"] = domain;
            return View();
        }

        public IActionResult ShowNews(int id)
        {
            HTMLPage page = StartWorkWeb.GetHtmlPage(id);
            ViewData["title"] = page.title;
            ViewData["data"] = page.datetime;
            ViewData["text"] = page.text;
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

        public IActionResult Send(string link)
        {
            if (!link.ExitURL())
            {
                ViewData["Message"] = $"Link ({link}) doesn't exist";
                
            }
            else
            {
                ViewData["Message"] = $"Link ({link}) exists";

                String temp=StartWorkWeb.Start(link);
                
                ViewData["Wait"] = temp;

                if ("Redirect" == temp) return Group();
            }
            return View();
        }

        //public async Task<IActionResult> Send()
        //{
        //    ViewData["Message"] = $"Link processed";
        //    ViewData["Wait"] = $"Wait..{WorkWeb.actionnumber}/{WorkWeb.allcount}";
        //    return View();
        //}
    }
}
