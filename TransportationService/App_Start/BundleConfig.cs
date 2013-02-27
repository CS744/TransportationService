using System.Web;
using System.Web.Optimization;

namespace MvcApplication1
{
   public class BundleConfig
   {
      // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
      public static void RegisterBundles(BundleCollection bundles)
      {
         bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-1.9.0.min.js"
         ));
         bundles.Add(new ScriptBundle("~/bundles/TransportationService").Include(
            "~/Scripts/Logon.js",
            "~/Scripts/notifier.js",
            "~/Scripts/roll_utility.js",
            "~/Scripts/bootstrap.js",
            "~/Scripts/jquery.transit.js",
            "~/Scripts/userManager.js",
            "~/Scripts/AdminActions.js",
            "~/Scripts/routeManager.js"
         ));

         bundles.Add(new StyleBundle("~/Content/css").Include(
            "~/Content/site.css",
            "~/Content/utility.css",
            "~/Content/bootstrap.css",
            "~/Content/font-awesome.css"
         ));
      }
   }
}
