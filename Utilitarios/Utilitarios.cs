using Models;
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
    public class FechasDashBoard
    {
        public int year = DateTime.Now.Year;
        public DateTime InicioEnero()
        {
            int month = 01;
            int day = 01;
            return Convert.ToDateTime(year + "/"+month+"/"+day).Date;
        }
        public DateTime FinEnero()
        {
            //return Convert.ToDateTime("2017/01/31").Date;
            int month = 01;
            int day = 31;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioFebrero()
        {
            //return Convert.ToDateTime("2017/02/01").Date;
            int month = 02;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinFebrero()
        {
            //return Convert.ToDateTime("2017/02/28").Date;
            int month = 02;
            int day = 28;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioMarzo()
        {
            //return Convert.ToDateTime("2017/03/01").Date;
            int month = 03;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinMarzo()
        {
            //return Convert.ToDateTime("2017/03/30").Date;
            int month = 03;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioAbril()
        {
            //return Convert.ToDateTime("2017/04/01").Date;
            int month = 04;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinAbril()
        {
            //return Convert.ToDateTime("2017/04/30").Date;
            int month = 04;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }

        public DateTime InicioMayo()
        {
            //return Convert.ToDateTime("2017/05/01").Date;
            int month = 05;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinMayo()
        {
            //return Convert.ToDateTime("2017/05/30").Date;
            int month = 05;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioJunio()
        {
            //return Convert.ToDateTime("2017/06/01").Date;
            int month = 06;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinJunio()
        {
            //return Convert.ToDateTime("2017/06/30").Date;
            int month = 06;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioJulio()
        {
            //return Convert.ToDateTime("2017/07/01").Date;
            int month = 07;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinJulio()
        {
            //return Convert.ToDateTime("2017/07/30").Date;
            int month = 07;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioAgosto()
        {
            //return Convert.ToDateTime("2017/08/01").Date;
            int month = 08;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinAgosto()
        {
            //return Convert.ToDateTime("2017/08/30").Date;
            int month = 08;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioSeptiembre()
        {
            //return Convert.ToDateTime("2017/09/01").Date;
            int month = 09;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinSeptiembre()
        {
            //return Convert.ToDateTime("2017/09/30").Date;
            int month = 09;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioOctubre()
        {
            //return Convert.ToDateTime("2017/10/01").Date;
            int month = 10;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinOctubre()
        {
            //return Convert.ToDateTime("2017/10/30").Date;
            int month = 10;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioNoviembre()
        {
            //return Convert.ToDateTime("2017/11/01").Date;
            int month = 11;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinNoviembre()
        {
            //return Convert.ToDateTime("2017/11/30").Date;
            int month = 11;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime InicioDiciembre()
        {
            //return Convert.ToDateTime("2017/12/01").Date;
            int month = 12;
            int day = 01;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
        }
        public DateTime FinDiciembre()
        {
            //return Convert.ToDateTime("2017/12/30").Date;
            int month = 12;
            int day = 30;
            return Convert.ToDateTime(year + "/" + month + "/" + day).Date;
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