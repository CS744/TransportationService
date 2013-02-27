using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportationService.Utility;
using MongoDB.Bson;
using TransportationService.Models;

namespace TransportationService.Controllers
{
   public class HomeController : TransportationBaseController
   {
      //
      // GET: /Home/

      public ActionResult Index()
      {
         return View();
      }

      public ActionResult LogIn(string username, string password)
      {
         DatabaseInterface db = new DatabaseInterface();
         User user = db.getUser(username, password);
         if (user == null)
         {
            return Json(new
            {
               error = true,
               message = "Invalid username or password"
            });
         }
         OnAuthorization(new AuthorizationContext());
         sessionManager.User = user;
         var model = new OutputViewModel()
         {
            Username = user.Username
         };

         return Json(new
         {
            user = JsonUtility.ToUserJson(user),
            headerText = "Welcome, " + model.Username,
            html = RenderPartialViewToString("AdminView", model)
         });
      }
      public ActionResult Logout()
      {
         sessionManager.User = null;
         return PartialView("Login");
      }
   }
}
