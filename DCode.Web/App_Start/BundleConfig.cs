using System.Web;
using System.Web.Optimization;

namespace DCode.Web
{
    public class BundleConfig
    {
        public const string JsJqueryBundle = "~/Scripts/jquery";
        //public const string JsJqueryValidationBundle = "~/Scripts/jqueryval";
        //public const string JsModernizrBundle = "~/Scripts/modernizr";
        //public const string JsBootstrapBundle = "~/Scripts/bootstrap";
        public const string JsAngularBundle = "~/Scripts/angular";
        public const string JsAngularAssetsBundle = "~/Scripts/angular-assets";

        public const string CssMainBundle = "~/Content";

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(JsJqueryBundle).Include(
                        "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle(JsJqueryValidationBundle).Include(
            //            "~/Scripts/jquery.validate*",
            //            "~/Scripts/validation-config.js",
            //            "~/Scripts/additional-methods.js"));

            //bundles.Add(new ScriptBundle(JsModernizrBundle).Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle(JsBootstrapBundle).Include(
            //            "~/Scripts/bootstrap.js",
            //            "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle(JsAngularBundle).Include(
                        "~/Scripts/AngularJs/angular.js",
                        "~/Scripts/AngularJs/angular-animate.js",
                //"~/Scripts/angular-messages.min.js",
                        "~/Scripts/AngularJs/angular-touch.js",
                //TODO: the ui-bootstrap-tpls script definitely overrides and replaces the ui-bootstrap0.14.3 script. Only include one version in the bundle
                //"~/Scripts/ui-bootstrap-0.14.3.min.js",
                //"~/Scripts/ui-bootstrap-tpls.js",
                //"~/Scripts/ui-grid.js",
                        "~/Scripts/AngularJs/angular-route.js",
                        "~/Scripts/AngularJs/ng-infinite-scroll.js",
                        "~/Scripts/AngularJs/angucomplete.js",
                        "~/Scripts/AngularJs/ui-router.min.js",
                       "~/Scripts/AngularJs/ngAutocomplete.js",
                       "~/Scripts/AngularJs/angucomplete-alt.js",
                        "~/Scripts/AngularJs/loading-bar.js",
                        "~/Scripts/AngularJs/ui-bootstrap-tpls-1.3.3.js"
                       // "~/Scripts/AngularJs/bootstrap-datepicker.js"
                       ));

            bundles.Add(new ScriptBundle(JsAngularAssetsBundle).IncludeDirectory(
                        "~/Scripts/App_JS", "*.js", true));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new StyleBundle(CssMainBundle).Include(
                        "~/Content/all.css",
                        "~/Content/angucomplete.css",
                        "~/Content/angucomplete-alt.css",
                        "~/Content/loading-bar"
                        //"~/Content/datepicker.css"
                        ));
        }
    }
}
