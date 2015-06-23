using System.Web;
using System.Web.Optimization;

namespace MVCProject
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                "~/Scripts/jquery-2.1.3.js",
                "~/Scripts/jquery.validate.*",
                "~/Scripts/jquery.gallerie.js",
                "~/Scripts/jquery.smint.js",
                "~/Scripts/jquery.wmuSlider.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/easing.js"
                ));
            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/bootstrap.css",
                "~/Content/css/Style.css",
                "~/Content/css/gallerie.css",
                "~/Content/css/gallerie-effects.css"
                ));
        }
    }
}
