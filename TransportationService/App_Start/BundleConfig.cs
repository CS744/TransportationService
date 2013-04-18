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
            "~/Scripts/logon.js",
            "~/Scripts/notifier.js",
            "~/Scripts/roll_utility.js",
            "~/Scripts/bootstrap.js",
            "~/Scripts/jquery.transit.js",
            "~/Scripts/userManager.js",
            "~/Scripts/adminActions.js",
            "~/Scripts/addActions.js",
            "~/Scripts/bootstrap-datepicker.js",
            "~/Scripts/employeeManagement.js",
            "~/Scripts/jquery.tablesorter.js",
            "~/Scripts/messageBuilder.js",
            "~/Scripts/modifyActions.js",
            "~/Scripts/viewActions.js",
            "~/Scripts/routeManager.js",
            "~/Scripts/validation.js"
         ));

         bundles.Add(new StyleBundle("~/Content/css").Include(
            "~/Content/site.css",
            "~/Content/utility.css",
            "~/Content/tableStyle.css",
            "~/Content/bootstrap.css",
            "~/Content/datepicker.css",
            "~/Content/font-awesome.css",
            "~/Content/bootstrap-formhelpers.css"
         ));
      }
   }
}
