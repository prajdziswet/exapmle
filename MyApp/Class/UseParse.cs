using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyApp.Models;

namespace MyApp.Class
{
    public class UseParse
    {
        private Uri UriDomain;

        public UseParse(Uri uri)
        {
            UriDomain = uri;
        }
        public string WithoutHtmlTag(string text)
        {
            return Regex.Replace(Regex.Replace(text, @"<(.|\n)*?>", string.Empty), @"\s+", " ");
        }

        public HTMLPage CreateEntetyHTMLPage(Uri UriAdress, string HtmlCode)
        {

            string BodyArticle = GetBodyArticle(HtmlCode);
            if (BodyArticle == "") return null;
            HTMLPage pageReturn = new HTMLPage();
            pageReturn.Domain = UriDomain.ToString();
            pageReturn.URL = UriAdress.ToString();
            pageReturn.FullHTML = HtmlCode;
            pageReturn.Title = GetTitle(BodyArticle);
            (string Date, string Aticle) = GetDateandAticle(BodyArticle);
            pageReturn.Text = Aticle;
            pageReturn.dateTime = Date;
       
            return pageReturn;
        }

        private string GetBodyArticle(string HtmlCode)
        {
            //решил регуляркой, хоть все равно коряво
            MatchCollection temp = Regex.Matches(HtmlCode, @"(?s)<h\d>.+?</h\d>\s+<div.+?>.+?<p>.+?</div>");
            if (temp.Count == 1)
            {
                int posStart, posEnd;
                posEnd = HtmlCode.IndexOf(temp[0].Value);
                posStart = HtmlCode.LastIndexOf("<div", posEnd);
                string strReturn = HtmlCode.Substring(posStart, posEnd + temp[0].Value.Length - posStart);
                strReturn = Regex.Replace(strReturn, "<script.+?/script>", String.Empty);
                return strReturn;
            }

            return "";
        }

        private string GetTitle(string BodyArticle)
        {
            MatchCollection temp = Regex.Matches(BodyArticle, @"(?s)<h\d>(.+?)</h\d>\s+");
            if (temp.Count == 0) throw new ArgumentNullException();
            return WithoutHtmlTag(temp[0].Groups[1].Value);
        }

        private (string Date, string Aticle) GetDateandAticle(string BodyArticle)
        {
            Console.WriteLine("___________________________________1___________________________________");
            Console.WriteLine(BodyArticle);
            Console.WriteLine("___________________________________end 1______________________________________");
            string strFirst =
                BodyArticle.Substring(0, BodyArticle.Substring(0, BodyArticle.IndexOf("<p>")).LastIndexOf("<div"));
            string strEnd = Regex.Replace(BodyArticle, "< script.+?/ script >", String.Empty);
            strEnd = WithoutHtmlTag(BodyArticle.Replace(strFirst, String.Empty))??"";
            Console.WriteLine("____________________________________________________________________________");
            Console.WriteLine(strEnd);
            strFirst = Regex.Replace(strFirst, @"(?s)<h\d>.+?</h\d>", String.Empty);
            strFirst = WithoutHtmlTag(strFirst);
            var x=UsePulenity.ReturnPullenity(strFirst).Result.Entities.Where(x => x.TypeName == "DATE").Select(x=>x.ToString()).Max();
            Console.WriteLine(x);

            string time = UsePulenity.ReturnPullenity(strFirst).Result.Entities.Where(x => x.TypeName == "DATE").Max(x => x.ToString().Length).ToString() ?? "";
            return (time, strFirst);
        }
    }
}
