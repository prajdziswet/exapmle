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
using MyApp.Models;
using Newtonsoft.Json;
using Pullenti.Morph;
using Pullenti.Ner;
using Pullenti.Ner.Titlepage;
using ServiceStack;

namespace MyApp.Class
{
    public class WorkWeb
    {
        public Uri URI { get; }

        public WorkWeb(string link)
        {
            if (!link.ExitURL()) throw new ArgumentException();
            URI = new Uri(link);
        }

        public async Task<string> HTML_Page(Uri URItemp)
        {
            var pageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());
            var crawledPage = await pageRequester.MakeRequestAsync(URItemp);
            return crawledPage.Content.Text;
        }


        private async Task<List<Uri>> GetLinks()
        {
            var pageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());
            var crawledPage = await pageRequester.MakeRequestAsync(URI);
            HyperLinkParser hyperLinkParser = new AngleSharpHyperlinkParser();
            return hyperLinkParser.GetLinks(crawledPage).Where(x=>x.RawHrefValue.Contains(URI.DnsSafeHost)&&x.RawHrefText.Trim().Split(' ').Length>3).Select(x=>x.HrefValue).ToList();
        }

        public void Analiz()
        {
            List<Uri> listUri = GetLinks().Result;
            foreach (Uri element in listUri)
            {
                ParseAddClass(element);
            }
        }

        private void ParseAddClass(Uri uri)
        {
            string HtmlCode= HTML_Page(uri).Result;
            //решил регуляркой, хоть все равно коряво
            MatchCollection temp = Regex.Matches(HtmlCode, @"(?s)<h\d>(.+?)</h\d>\s+<div.+?>.+?<p>.+?</div>");
            if (temp.Count == 1)
            {
                int posStart, posEnd;
                posEnd = HtmlCode.IndexOf(temp[0].Value);
                posStart = HtmlCode.LastIndexOf("<div", posEnd);
                string str = HtmlCode.Substring(posStart, posEnd + temp[0].Value.Length - posStart);
                str=Regex.Replace(str, "< script.+?/ script >", String.Empty);
                string title = temp[0].Groups[1].Value;
                string strFirst =
                    str.Substring(0, str.Substring(0, str.IndexOf("<p>")).LastIndexOf("<div"));
                string strEnd = WithoutHtmlTag(str.Replace(strFirst, String.Empty));
                strFirst = Regex.Replace(strFirst, @"(?s)<h\d>.+?</h\d>", String.Empty);
                strFirst = WithoutHtmlTag(strFirst);
                string time = ReturnPullenity(strFirst).Result.Entities.Where(x=>x.TypeName=="DATE").Max(x=>x.ToString().Length).ToString();
                using (ApplicationContext db = new ApplicationContext())
                {
                    HTMLPage tempclass = new HTMLPage();
                    tempclass.FullHTML = HtmlCode;
                    tempclass.Text = strEnd;
                    tempclass.Title = title;
                    tempclass.dateTime = time;
                    tempclass.URL = uri.ToString();
                    if (db.HtmlPages.Count()==0||!db.HtmlPages.Any(x => x.URL != uri.ToString()))
                    {
                        db.Add(tempclass);
                        db.SaveChangesAsync();
                    }
                }

            }
            //Console.WriteLine(temp.Count);
            //Console.WriteLine(temp[0].Value);

        }

        //public void pro()
        //{
        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        Console.WriteLine(db.HtmlPages.Count());
        //    }
        //}

        public string WithoutHtmlTag(string text)
        {
            return Regex.Replace(Regex.Replace(text, @"<(.|\n)*?>", string.Empty),@"\s+"," ");
        }

        public async Task<AnalysisResult> ReturnPullenity(string text)
        {
            Pullenti.Sdk.InitializeAll();
            // создаём экземпляр процессора со стандартными анализаторами
            Processor processor = ProcessorService.CreateProcessor();
            //processor.Analyzers
            SourceOfAnalysis source = new SourceOfAnalysis(text);
            // запускаем на тексте text
            AnalysisResult result = processor.Process(source);
           
            //processor.ProcessNext(result);
            //получили выделенные сущности
            //foreach (Referent entity in result.Entities)
            //{
            //    Console.WriteLine("____________________________________________");
            //    Console.WriteLine(entity.ToString());
            //    //Console.WriteLine(entity.InstanceOf.Caption);
            //    Console.WriteLine(entity.TypeName);
            //    Console.WriteLine(entity.Serialize());
            //    //var temp=entity.Slots;
            //    //foreach (Slot element in temp)
            //    //{
            //    //    string jsonString = JsonConvert.SerializeObject(element);
            //    //    Console.WriteLine(jsonString);
            //    //}
            //}

            return result;
        }



    }
}
