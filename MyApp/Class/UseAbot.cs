using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abot2.Core;
using Abot2.Poco;

namespace MyApp.Class
{
    public class UseAbot
    {
        private Uri URI;

        public UseAbot(Uri uri)
        {
            URI = uri;
        }
        public async Task<string> HTML_Page(Uri URItemp)
        {
            var pageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());
            var crawledPage = await pageRequester.MakeRequestAsync(URItemp);
            return new string(crawledPage.Content.Text);
        }

        public async Task<List<Uri>> GetLinks()
        {
            var pageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());
            var crawledPage = await pageRequester.MakeRequestAsync(URI);
            HyperLinkParser hyperLinkParser = new AngleSharpHyperlinkParser();
            return hyperLinkParser.GetLinks(crawledPage).Where(x => x.RawHrefValue.Contains(URI.DnsSafeHost) && x.RawHrefText.Trim().Split(' ').Length > 3).Select(x => x.HrefValue).ToList();
        }
    }
}
