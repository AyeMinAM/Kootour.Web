﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;

namespace MVCSite.Web.ViewModels
{

    public class AdminUserAccountModel : Layout
    {
        public IEnumerable<User> users { get; set; }
    } 
}