using System;
using System.Net;

namespace MyApp.Class
{
    public static class Extensioan
    {
        public static bool ExitURL(this string URI)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(URI);
                request.Method = "HEAD";

                var response = (HttpWebResponse)request.GetResponse();

                return response.StatusCode == HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}