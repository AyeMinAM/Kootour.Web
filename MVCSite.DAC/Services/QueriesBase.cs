using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;

namespace MVCSite.DAC.Services
{
    public class QueriesBase
    {
        protected readonly ISecurity _security;
        protected readonly IWebApplicationContext _webContext;

        public QueriesBase(ISecurity security, IWebApplicationContext webContext)
        {
            _security = security;
            _webContext = webContext;
        }
    }
}
