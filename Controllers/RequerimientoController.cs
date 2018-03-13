using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCKanban.Utilitarios;
using System.IO;
using MVCKanban.ViewModel;
using Models;

namespace MVCKanban.Controllers
{
    //[Authorize]
    public class RequerimientoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserIdentity user = new UserIdentity();
        private Utilitarios.Utilitarios utl = new Utilitarios.Utilitarios();
        private Utilitarios.GetErrors getError = new GetErrors();
        // GET: Requerimiento
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            return PartialView();
        }
        public ActionResult Create()
        {
            ViewBag.Depatarmento = new SelectList(utl.GetDepartamento(), "IDDepartamento", "departamento");
            return PartialView();
        }
        [HttpPost]
        public JsonResult Create(Requerimiento requerimiento, HttpPostedFileBase[] file)
        {

            try
            {
                bool bsuccess = false;
                int id = 0;
                if (ModelState.IsValid)
                {
                    MaestroTaskStatus maestroTaskStatus = new MaestroTaskStatus();
                    //  tasks.ruta_foto = CrearDirectorio(IDRuta);
                    requerimiento.FechaCreacion = DateTime.Now.Date;
                    //tasks.FechaFinalizacion = DateTime.Now.Date;
                    requerimiento.StatusIDActual = 1;
                    requerimiento.UsuarioID = user.GetIdUser();


                    db.Requerimiento.Add(requerimiento);
                    db.SaveChanges();
                    var getLast = db.Requerimiento.OrderByDescending(u => u.RequerimientoID).FirstOrDefault();
                    id = getLast.RequerimientoID;

                    // SE SALVA EN EL HISTORICO DE REQUERIMIENTOS
                    maestroTaskStatus.StatusID = 1;
                    maestroTaskStatus.RequerimientoID = id;
                    maestroTaskStatus.Fecha = DateTime.Now.Date;
                    db.MaestroTaskStatus.Add(maestroTaskStatus);
                    db.SaveChanges();


                    bsuccess = true;
                    bool boolArchivos = utl.CantidadArchivos(id);
                    if (boolArchivos == true)
                    {
                        return Json(new { success = false, cantidad = boolArchivos, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
                    }
                    string ruta = Path.Combine(Server.MapPath("~/Images/Requerimientos/" + id));
                    string ruta_virtual = "~/Images/Requerimientos/" + id;
                    utl.SaveUploadedFile(file,id, ruta, ruta_virtual);

                }
                return Json(new { success = bsuccess, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewTaskFile TaskFiles = new ViewTaskFile();
            var vtask = db.Requerimiento.ToList();
            var vFile = db.Files.ToList();

            var subquery = db.Requerimiento.Where(sq => sq.RequerimientoID == id).SingleOrDefault();
            var subqueryFile = db.Files.Where(qf => qf.RequerimientoID == id).ToList();
            TaskFiles.RequerimientoID = id;
            TaskFiles.Titulo = subquery.Titulo;
            TaskFiles.Descripcion = subquery.Descripcion;
            TaskFiles.ruta_virtual = new List<string>();
            TaskFiles.IDFiles = new List<int>();
            TaskFiles.IDDepartamento = subquery.IDDepartamento;
            foreach (var item in subqueryFile)
            {
                TaskFiles.ruta_virtual.Add(item.ruta_virtual);
                TaskFiles.IDFiles.Add(item.IDFiles);
            }
            ViewBag.Depatarmento = new SelectList(utl.GetDepartamento(), "IDDepartamento", "departamento");
            return PartialView(TaskFiles);
        }
        [HttpPost]
        public JsonResult Edit(Requerimiento requerimiento, HttpPostedFileBase[] FileEdit)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    var result = db.Requerimiento.Find(requerimiento.RequerimientoID);
                    result.Titulo = requerimiento.Titulo;
                    result.Descripcion = requerimiento.Descripcion;
                    result.IDDepartamento = requerimiento.IDDepartamento;
                    db.SaveChanges();
                    bsuccess = true;
                    if (FileEdit.Count() > 0)
                    {
                        bool boolArchivos = utl.CantidadArchivos(requerimiento.RequerimientoID);
                        if (boolArchivos == true)
                        {
                            return Json(new { success = false, cantidad = boolArchivos, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
                        }

                        string ruta = Path.Combine(Server.MapPath("~/Images/Requerimientos/" + requerimiento.RequerimientoID));
                        string ruta_virtual = "~/Images/Requerimientos/" + requerimiento.RequerimientoID;
                        utl.SaveUploadedFile(FileEdit, requerimiento.RequerimientoID, ruta, ruta_virtual);
                    }
                }
                return Json(new { success = bsuccess, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Errors = getError.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }
        private void SaveUploadedFile(HttpPostedFileBase[] file, int id)
        {
            foreach (HttpPostedFileBase Archivo in file)
            {
                if (Archivo != null)
                {
                    if (Archivo != null && Archivo.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("~/Images/" + id));
                        string ruta = Path.Combine(Server.MapPath("~/Images/" + id));
                        string ruta_virtual = "~/Images/" + id;
                        string pathString = ruta;
                        var fileName1 = Path.GetFileName(Archivo.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, Archivo.FileName);


                        Files modelFiles = new Files
                        {
                            nombre = Archivo.FileName,
                            ruta = pathString,
                            RequerimientoID = id,
                            ruta_virtual = ruta_virtual + "/" + Archivo.FileName
                        };

                        db.Files.Add(modelFiles);
                        db.SaveChanges();
                        Archivo.SaveAs(path);
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                bool bsuccess = true;
                Files deleteFiles = db.Files.Where(d => d.RequerimientoID == id).SingleOrDefault();
                if (deleteFiles != null)
                {
                    Directory.Delete(deleteFiles.ruta, true);
                    db.Files.Remove(deleteFiles);

                }
                MaestroTaskStatus maestro = db.MaestroTaskStatus.Where(d => d.RequerimientoID == id).SingleOrDefault();
                Requerimiento requermiento = db.Requerimiento.Where(r => r.RequerimientoID == id).Single();

                db.MaestroTaskStatus.Remove(maestro);
                db.Requerimiento.Remove(requermiento);
                db.SaveChanges();
                return Json(new { success = bsuccess });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public ActionResult DeleteImage(int id)
        {
            /* http://plugins.krajee.com/file-input-ajax-demo/3 */
            try
            {
                bool bsuccess = false;
                Files deleteFiles = db.Files.Where(d => d.IDFiles == id).SingleOrDefault();
                if (deleteFiles != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(deleteFiles.ruta_virtual)))
                    {
                        System.IO.File.Delete(Server.MapPath(deleteFiles.ruta_virtual));//investigar  Server.MapPath
                        Files registro = db.Files.Where(r => r.IDFiles == id).Single();

                        db.Files.Remove(registro);
                        db.SaveChanges();
                        bsuccess = true;

                    }
                }

                return Json(new { success = bsuccess });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        public JsonResult get()
        {

            // ES NECESARIO PONER EXACTAMENTE LOS CAMPOS A EXTRAER PORQUE SI NO DA ERROR
            var tabla = db.Requerimiento.ToList();
            var status = db.Status.ToList();
            var jsonData = new
            {
                data = (from query in db.Requerimiento
                        join sta in db.Status on query.StatusIDActual equals sta.StatusID
                        join dep in db.Departamento on query.IDDepartamento equals dep.IDDepartamento
                        select new
                        {
                            Titulo = query.Titulo,
                            FechaCreacion = query.FechaCreacion,
                            Status = sta.nombre,
                            RequerimientoID = query.RequerimientoID,
                            Departamento = dep.departamento
                        }).ToList()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
