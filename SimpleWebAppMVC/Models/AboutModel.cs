using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Models
{
    /**
     * About Model
     */
    public class AboutModel
    {
        public string AppName   { get; set; }
        public string Copyright { get; set; }
        public string Url       { get; set; }
        public string Version   { get; set; }
    }
}
