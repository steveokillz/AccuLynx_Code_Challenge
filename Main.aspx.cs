﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccuLynx_Code_Challenge
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userMachine = HttpContext.Current.Server.MachineName;
            Label1.Text = "Welcome User: " + userMachine;
        }
    }
}