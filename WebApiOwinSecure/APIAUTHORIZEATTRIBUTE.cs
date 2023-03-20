﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiOwinSecure
{
    //Kimlik bilgileri dogrulanmamis ise hata yaniti dondurmek icin
    public class APIAUTHORIZEATTRIBUTE : System.Web.Http.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}