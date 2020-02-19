using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Common
{
    public class CrawlerException : ApplicationException
    {
        public CrawlerException(string message) : base(message) { }
        public CrawlerException(string message, Exception exception) : base(message, exception) { }
    }

    public class GeocoderException : CrawlerException
    {
        public GeocoderException(string message) : base(message) { }
        public GeocoderException(string message, Exception exception) : base(message, exception) { }
    }
}