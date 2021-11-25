using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    [Table("htmlpages")]
    public class HTMLPage
    {
        public int id { get; set; }
        public string fullhtml { get; set; }

        public string title { get; set; }

        public string text { get; set; }

        public string url { get; set; }

        public string datetime { get; set; }

        public string domain { set; get; }
    }
}