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
         Bus bus = new Bus()
         {
            Capacity = 5,
            Id = ObjectId.GenerateNewId(),
            LiscensePlate = "ABC 123",
         };
         DatabaseInterface db = new DatabaseInterface();
         db.SaveBus(bus);

         return View(bus);
      }

      public ActionResult RegisterUser(string username, string password)
      {
         User user = new User()
         {
            Id = ObjectId.GenerateNewId(),
            Username = username,
            Password = password,
            Email = "emailAddressNotImplemented@gmail.com",
            Type = UserType.Driver
         };
         //will save user.
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
