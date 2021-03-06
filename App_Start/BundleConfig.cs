﻿using System.Web;
using System.Web.Optimization;

namespace MVCKanban
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUIjs").Include(
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/bundles/jqueryUI").Include(
                "~/Content/jquery-ui.css").IncludeDirectory("~/Content", ".css"));

            bundles.Add(new ScriptBundle("~/bundles/slimscroll").Include(
                      "~/Scripts/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                      "~/Scripts/bootstrap-datepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminlte").Include(
          "~/Scripts/adminlte.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapJS").Include(
"~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/bundles/DataPikerCss").Include(
            "~/Content/bootstrap-datepicker.css").IncludeDirectory("~/Content", ".css"));


            bundles.Add(new StyleBundle("~/bundles/KanbanCss").Include(
                "~/Content/kanban.css").IncludeDirectory("~/Content", ".css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/_all-skins.min.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/daterangepicker.css",
                      "~/Content/font-awesome.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/AdminLTE.css"));

            //DATATABLE
            bundles.Add(new ScriptBundle("~/bundles/DatatableJS").Include(
                   "~/Scripts/datatables.min.js"));

            bundles.Add(new StyleBundle("~/Content/DatatableCSS").Include(
                      "~/Content/datatables.min.css"));

            // fileinput
            bundles.Add(new ScriptBundle("~/bundles/FileinputJS").Include(
            "~/Scripts/fileinput.min.js"));

            bundles.Add(new StyleBundle("~/Content/FileinputCSS").Include(
                      "~/Content/fileinput.min.css"));

            //CHART JS
            bundles.Add(new ScriptBundle("~/bundles/Chart").Include(
           "~/Scripts/Chart.min.js").IncludeDirectory("~/Scripts", ".js"));

            /*SE AGREGAN LOS JS DE LOS MODULOS*/
            bundles.Add(new ScriptBundle("~/bundles/scriptRequerimiento").Include(
               "~/Scripts/App/Requerimiento/Requerimiento.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new ScriptBundle("~/bundles/scriptDepartamento").Include(
               "~/Scripts/App/Departamento/Departamento.js").IncludeDirectory("~/Scripts", ".js"));


            bundles.Add(new ScriptBundle("~/bundles/scriptUsuario").Include(
               "~/Scripts/App/Usuario/Usuario.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new ScriptBundle("~/bundles/scriptRoles").Include(
               "~/Scripts/App/Roles/Roles.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new ScriptBundle("~/bundles/scriptModulos").Include(
              "~/Scripts/App/Modulo/Modulo.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new ScriptBundle("~/bundles/DataPikerJs").Include(
             "~/Scripts/bootstrap-datepicker.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new ScriptBundle("~/bundles/Asignar").Include(
             "~/Scripts/App/Asignar/Asignar.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new ScriptBundle("~/bundles/Revision").Include(
            "~/Scripts/App/Revision/Revision.js").IncludeDirectory("~/Scripts", ".js"));

            bundles.Add(new ScriptBundle("~/bundles/Kanban").Include(
            "~/Scripts/App/Kanban/Kanban.js").IncludeDirectory("~/Scripts", ".js"));





        }
    }
}
