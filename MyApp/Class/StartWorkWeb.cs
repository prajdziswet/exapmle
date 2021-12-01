using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Class
{
    public static class StartWorkWeb
    {
        private static object locker = new object();
        private static int allcount;
        private static int actionnumber;
        public static bool startAction = false;

        private static List<Sites> listSites = new List<Sites>();

        public static string Start(string link)
        {
            if (!link.ExitURL()) throw new ArgumentException();

            string returnString = "";
            //useAbot = new UseAbot(link);
            //useParse = new UseParse(link);
            //List<Uri> listUri = useAbot.GetLinks().Result;
            //string HtmlCode = useAbot.HTML_Page(listUri[0]).Result;
            //HTMLPage tempclass = useParse.CreateEntetyHTMLPage(listUri[0], HtmlCode);

            if (startAction == true) returnString = $"Done {actionnumber}/{allcount} in {link}";
            else
            {
                if (listSites.Count == 0)
                {
                    listSites.Add(new Sites(link));
                    StartAnalize(link);
                    returnString = "Wait 5 sec, process is starting..";
                }
                else
                {
                    string dateTime = DateTime.Now.ToString("d");
                    if (!listSites.Any(x => x.uri == link && x.dateTime == dateTime))
                    {
                        listSites.Add(new Sites(link));
                        StartAnalize(link);
                        returnString = "Wait 5 sec, process is starting..";
                    }
                    else
                    {
                        returnString = "Redirect";
                    }
                }
            }

            return returnString;

        }

        public static IReadOnlyList<HTMLPage> ReturnHtmlPages(String domain)
        {
            return (new ApplicationContext()).HtmlPages.AsNoTracking().Where(x=>x.domain==domain).ToList();
        }

        public static IReadOnlyList<string> ReturnGroup()
        {
            return (new ApplicationContext()).HtmlPages.AsNoTracking().GroupBy(x => x.domain).Select(x => x.Key)
                .ToList();
        }

        public static HTMLPage GetHtmlPage(int id)
        {
            return (new ApplicationContext()).HtmlPages.AsNoTracking().FirstOrDefault(x => x.id == id);
        }

        public static void DeleteNoticeDB()
        {
            using (ApplicationContext db =new ApplicationContext())
            {
                var allHtmlPages=db.HtmlPages.ToList();
                db.RemoveRange(allHtmlPages);
                db.SaveChanges();
            }
        }

        private static UseAbot useAbot;
        private static UseParse useParse;
        private static async void StartAnalize(string link)
        {
            startAction = true;
            actionnumber = 0;
            useAbot = new UseAbot(link);
            useParse = new UseParse(link);
            List<Uri> listUri = await useAbot.GetLinks();
            allcount = listUri.Count;
            listUri = GetUriNotDatebase(listUri);
            actionnumber = allcount - listUri.Count;
            var result = Parallel.ForEach<Uri>(listUri, AddClass);
            Console.WriteLine(result.IsCompleted);
            useAbot = null;
            useParse = null;
        }

        private static List<Uri> GetUriNotDatebase(List<Uri> listUri)
        {
            List<Uri> listUriReturn = new List<Uri>();
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (db.HtmlPages?.AsNoTracking().Count() == 0) return listUri;
                    foreach (Uri elementUri in listUri)
                    {
                        if (!db.HtmlPages.Any(x => x.url == elementUri.ToString()))
                            listUriReturn.Add(elementUri);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("______________Exception in GetUriNotDatebase_________");
                Console.WriteLine(e);
                Console.WriteLine("_______________________");
            }

            return listUriReturn;
        }

        private static void AddClass(Uri uri)
       {
            string HtmlCode = useAbot.HTML_Page(uri).Result;

            lock (StartWorkWeb.locker)
                try
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                            HTMLPage tempclass = useParse.CreateEntetyHTMLPage(uri, HtmlCode);
                            if (tempclass != null)
                            {
                                db.Add(tempclass);
                                db.SaveChanges();
                                Console.WriteLine(db.HtmlPages.Count());
                            }


                        StartWorkWeb.actionnumber++;
                        if (StartWorkWeb.actionnumber == StartWorkWeb.allcount) StartWorkWeb.startAction = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("______________Exception_________");
                    Console.WriteLine(e);
                    Console.WriteLine("______________Exception_________");
                    throw;
                }


        }
    }

    public class Sites
    {
        public string uri;
        public string dateTime;

        public Sites(String uri)
        {
            string dateTime = DateTime.Now.ToString("d");
            this.dateTime = dateTime;
            this.uri = uri;
        }
    }
}
