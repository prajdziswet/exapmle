using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    [Table("HtmlPages")]
    public class HTMLPage
    {
        public int ID { get; set; }
        public string FullHTML { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string URL { get; set; }

        public string dateTime { get; set; }

        public string Domain { set; get; }
    }
}