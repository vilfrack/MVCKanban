using MVCKanban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKanban.Utilitarios
{
    public class Utilitarios
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool CantidadArchivos(int RequerimientoID)
        {

            int CantidadArchivo = db.Files.Where(w => w.RequerimientoID == RequerimientoID).Count();
            if (CantidadArchivo >= 3)
            {
                return true;
            }
            return false;
        }
        public List<Departamento> GetDepartamento()
        {
            var departamento = db.Departamento.ToList();
            return departamento;
        }
        /**/
        public void SaveUploadedFile(HttpPostedFileBase[] file, int id, string ruta,string ruta_virtual)
        {
            foreach (HttpPostedFileBase Archivo in file)
            {
                if (Archivo != null)
                {
                    if (Archivo != null && Archivo.ContentLength > 0)
                    {
                        bool isExists = System.IO.Directory.Exists(ruta);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(ruta);

                        var path = string.Format("{0}\\{1}", ruta, Archivo.FileName);


                        Files modelFiles = new Files
                        {
                            nombre = Archivo.FileName,
                            ruta = ruta,// + "/" + Archivo.FileName,
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
    }

    public class GetErrors : Controller
    {
        public Dictionary<string, object> GetErrorsFromModelState(ModelStateDictionary mState)
        {
            //explicar el errors
            var errors = new Dictionary<string, object>();
            foreach (var key in mState.Keys)
            {
                // Only send the errors to the client.
                if (mState[key].Errors.Count > 0)
                {
                    errors[key] = mState[key].Errors;
                }
                else
                {
                    errors[key] = "true";
                }
            }
            return errors;
        }
    }
}