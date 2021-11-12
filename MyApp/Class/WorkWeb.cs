using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Abot2;
using Abot2.Poco;
using ServiceStack.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Abot2.Core;
using AngleSharp;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;


namespace MyApp.Class
{
    public class WorkWeb
    {
        private Uri URI { get; }

        private readonly UseAbot useAbot;
        private UseParse useParse;

        public WorkWeb(string link)
        {
            if (!link.ExitURL()) throw new ArgumentException();
            URI = new Uri(link);
            useAbot = new UseAbot(URI);
            useParse = new UseParse(URI);
        }



        public  void Analiz()
        {
            List<Uri> listUri = useAbot.GetLinks().Result;

            foreach (Uri elemUri in listUri)
            {
                AddClass(elemUri);
            }
            //ParallelLoopResult result = Parallel.ForEach<Uri>(listUri, AddClass);
            //Console.WriteLine($"{result.IsCompleted}");
        }

        private  void AddClass(Uri uri)
        {
                string HtmlCode=  useAbot.HTML_Page(uri).Result;

                using (ApplicationContext db = new ApplicationContext())
                {

                    if (db.HtmlPages.AsNoTracking().Count() == 0||
                        !db.HtmlPages.Any(x => x.URL == uri.ToString()))
                    {
                        HTMLPage tempclass = useParse.CreateEntetyHTMLPage(uri, HtmlCode);
                        if (tempclass!=null)
                        {
                            db.Add(tempclass);
                            db.SaveChangesAsync();
                        }
                    }
                }

        }







    }
}
