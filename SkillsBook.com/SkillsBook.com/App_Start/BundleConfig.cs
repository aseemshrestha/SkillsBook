using System.Web;
using System.Web.Optimization;

namespace SkillsBook.com
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.js",
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));



            bundles.Add(new ScriptBundle("~/bundles/essentials").Include(
                         "~/js/modernizr.custom.js",
                         "~/assets/bootstrap/js/bootstrap.min.js"
                     
                     ));

            bundles.Add(new ScriptBundle("~/bundles/assets").Include(
                        "~/assets/ui-kit/js/jquery.powerful-placeholder.min.js",
                        "~/assets/ui-kit/js/cusel.min.js",
                        "~/assets/sky-forms/js/jquery.form.min.js",
                        "~/assets/sky-forms/js/jquery.modal.js",
                         "~/assets/hover-dropdown/bootstrap-hover-dropdown.min.js",
                         "~/assets/page-scroller/jquery.ui.totop.min.js",
                         "~/assets/mixitup/jquery.mixitup.init.js",
                         "~/assets/waypoints/waypoints.min.js",
                         "~/assets/mixitup/jquery.mixitup.js",
                         "~/assets/milestone-counter/jquery.countTo.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/optional").Include(
                       "~/assets/responsive-mobile-nav/js/jquery.dlmenu.js",
                       "~/assets/responsive-mobile-nav/js/jquery.dlmenu.autofill.js"
                ));

        }
    }
}