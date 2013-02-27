using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Hosting;
using System.Xml.Linq;
using System.Configuration;
using System.Security.Cryptography;
using MongoDB.Bson;
using TransportationService.Utility;

namespace TOPSS.Utility
{
   public class SessionManager
   {
      private static class SessionKey
      {
         public static readonly string CurrentUser = "User";
      }

      public SessionManager(HttpSessionStateBase sessionCache)
      {
         _sessionCache = sessionCache;
         sessionCache.Timeout = 60;
      }

      /// <summary>
      /// 
      /// </summary>
      public User User
      {
         get
         {
            var user = (User)_sessionCache[SessionKey.CurrentUser];//_sessionCache.Get<User>(SessionKey.CurrentUser);
            return user;
         }
         set
         {
            _sessionCache[SessionKey.CurrentUser] = value;
         }
      }

      /// <summary>
      /// </summary>
      /// <param name="sessionCache"></param>
      /// <param name="applicationCache"></param>
      /// <param name="topssPath"></param>
      public static void Initialize()
      {
      }

      private HttpSessionStateBase _sessionCache;
   }
}
