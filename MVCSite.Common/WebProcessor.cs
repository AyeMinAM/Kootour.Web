using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Windows.Forms;

namespace MVCSite.Common
{
    public class WebProcessor //: IWebProcessor
    {
        private string GeneratedSource { get; set; }
        public string SiteCookie { get; set; }
        private string URL { get; set; }

        public string GetGeneratedHTML(string url)
        {
            URL = url;

            Thread t = new Thread(new ThreadStart(WebBrowserThread));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return GeneratedSource;
        }

        private void WebBrowserThread()
        {
            WebBrowser wb = new WebBrowser();
            wb.Navigate(URL);

            wb.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(
                    wb_DocumentCompleted);
            var startTime = DateTime.UtcNow;
            var runTime = TimeSpan.FromSeconds(1);
            while (wb.ReadyState != WebBrowserReadyState.Complete && runTime.TotalSeconds < 60)
            {
                Application.DoEvents();
                Thread.Sleep(50);
                runTime = DateTime.UtcNow - startTime;
            }
            wb.Stop();
            //Added this line, because the final HTML takes a while to show up
            //            GeneratedSource = wb.Document.Body.InnerHtml;
            //GeneratedSource = wb.Document.Body.Parent.OuterHtml;
            if (wb.Document.Body != null && wb.Document.Body.Parent != null)
                GeneratedSource = wb.Document.Body.Parent.OuterHtml;
            else
                GeneratedSource = string.Empty;
            SiteCookie = wb.Document.Cookie;
            wb.Dispose();
            wb = null;
        }

        private void wb_DocumentCompleted(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;
            GeneratedSource = wb.Document.Body.Parent.OuterHtml;
        }
    }

}