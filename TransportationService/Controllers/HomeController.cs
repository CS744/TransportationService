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
         User user = new User()
         {
            Email = "weisse.simon@gmail.com",
            Id = ObjectId.GenerateNewId(),
            Password = "Soupy",
            Username = "Simon"
         };
         DatabaseInterface db = new DatabaseInterface();
         db.SaveUser(user);

         return View();
      }

      public ActionResult LogIn(string username, string password)
      {

         DatabaseInterface db = new DatabaseInterface();
         User user = db.getUser(username,password);

         var model = new OutputViewModel()
         {
            Username = user.Username
         };

         return Json(new
         {
            user = JsonUtility.ToUserJson(user),
            html = RenderPartialViewToString("ExampleView", model)
         });
      }
   }
}
