using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using TOPSS.Utility;
using TransportationService.Utility;

namespace TransportationService.Controllers
{
   public abstract class TransportationBaseController : Controller
   {
      protected SessionManager sessionManager;
      protected override void OnActionExecuting(ActionExecutingContext ctx)
      {
         base.OnActionExecuting(ctx);
         sessionManager = new SessionManager(this.HttpContext.Session);
         if (sessionManager.User == null)
         {
            //if the user was loaded from a auth cookie, grab the user name from it and load the user details, and place it in the session variable
            var identity = HttpContext.User.Identity as System.Security.Principal.IIdentity;
            if (identity != null)
            {
               System.Web.Security.FormsIdentity fi = identity as System.Web.Security.FormsIdentity;
               if (fi != null)
               {
                  DatabaseInterface db = new DatabaseInterface();
                  var user = db.GetUserById(new ObjectId(fi.Name));
                  if (user != null)
                  {
                     sessionManager.User = user;
                  }
               }
            }
         }
      }
      protected string RenderPartialViewToString()
      {
         return RenderPartialViewToString(null, null);
      }

      protected string RenderPartialViewToString(string viewName)
      {
         return RenderPartialViewToString(viewName, null);
      }

      protected string RenderPartialViewToString(object model)
      {
         return RenderPartialViewToString(null, model);
      }

      protected string RenderPartialViewToString(string viewName, object model)
      {
         if (string.IsNullOrEmpty(viewName))
            viewName = ControllerContext.RouteData.GetRequiredString("action");

         ViewData.Model = model;

         using (StringWriter sw = new StringWriter())
         {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
            ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
            viewResult.View.Render(viewContext, sw);

            return sw.GetStringBuilder().ToString();
         }
      }
   }
}
