using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using MVCKanban.Utilitarios;
using MVCKanban.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCKanban.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Utilitarios.Utilitarios utl = new Utilitarios.Utilitarios();
        private GetErrors errors = new GetErrors();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return PartialView();
        }

        public JsonResult get()
        {

            // ES NECESARIO PONER EXACTAMENTE LOS CAMPOS A EXTRAER PORQUE SI NO DA ERROR
            var jsonData = new
            {
                data = (from u in db.Users
                        join p in db.Perfiles on u.Id equals p.UsuarioID
                        select new
                        {
                            IDUser = u.Id,
                            login = u.UserName,
                            nombre = p.Nombre,
                            apellido = p.Apellido
                        }).ToList()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            List<ViewRol> Rol = new List<ViewRol>();
            var varRol = db.Roles.Select(s => new { Id = s.Id, Name = s.Name }).ToList();
            foreach (var item in varRol)
            {

                var viewRol = new ViewRol
                {
                    Name = item.Name,
                    Id = item.Id
                };
                Rol.Add(viewRol);
            }
            ViewBag.Depatarmento = new SelectList(utl.GetDepartamento(), "IDDepartamento", "departamento");
            return PartialView(Rol);
        }

        [HttpPost]
        public JsonResult Create(ViewUserPerfil UserPerfil, HttpPostedFileBase file)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    if (!(db.Users.Any(u => u.UserName == UserPerfil.UserName)))
                    {

                        var userStore = new UserStore<ApplicationUser>(db);
                        var userManager = new UserManager<ApplicationUser>(userStore);
                        var userToInsert = new ApplicationUser
                        {
                            Email = UserPerfil.Email,
                            PasswordHash = UserPerfil.Password,
                            UserName = UserPerfil.UserName,
                        };
                        userManager.Create(userToInsert, UserPerfil.Password);
                        string ruta = string.Empty;
                        if (file != null)
                        {
                            ruta = SaveUploadedFile(file, userToInsert.Id);
                        }
                        else
                        {
                            ruta = "~/Images/empleados/perfil.jpg";
                        }
                        var perfiles = new Perfiles
                        {
                            Nombre = UserPerfil.Nombre,
                            Apellido = UserPerfil.Apellido,
                            UsuarioID = userToInsert.Id,
                            rutaImg = ruta,
                            IDDepartamento = UserPerfil.departamento,
                        };
                        db.Perfiles.Add(perfiles);
                        db.SaveChanges();
                        //agregar los roles al usuario
                        var roleStore = new RoleStore<IdentityRole>(db);
                        var roleManager = new RoleManager<IdentityRole>(roleStore);
                        foreach (var varRol in UserPerfil.rol)
                        {
                            var rolName = roleManager.FindById(varRol);
                            //permite hacer el insert en AspNetUserRoles
                            userManager.AddToRole(userToInsert.Id, rolName.Name);
                        }
                        bsuccess = true;
                    }
                }

                return Json(new { success = bsuccess, Errors = errors.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch
            {
                return Json(new { success = false, Errors = errors.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }

        private string SaveUploadedFile(HttpPostedFileBase file, string id)
        {
            var originalDirectory = new DirectoryInfo(string.Format("~/Images/empleados/" + id));
            string ruta = Path.Combine(Server.MapPath("~/Images/empleados/" + id));
            string ruta_virtual = "~/Images/empleados/" + id;
            string pathString = ruta;
            bool isExists = System.IO.Directory.Exists(pathString);

            if (!isExists)
                System.IO.Directory.CreateDirectory(pathString);

            var path = string.Format("{0}\\{1}", pathString, file.FileName);

            file.SaveAs(path);
            return ruta_virtual + "/" + file.FileName;
        }

        public ActionResult Edit(string id)
        {
            var query = (from u in db.Users
                         join p in db.Perfiles on u.Id equals p.UsuarioID
                         where u.Id == id
                         select new
                         {
                             Id = u.Id,
                             Password = u.PasswordHash,
                             Email = u.Email,
                             Nombre = p.Nombre,
                             Apellido = p.Apellido,
                             IDPerfil = p.IDPerfil,
                             Ruta = p.rutaImg,
                             IDDepartamento = p.IDDepartamento
                         }).SingleOrDefault();


            var roles = db.Users
                    .Where(u => u.Id == id)
                    .SelectMany(u => u.Roles)
                    .Join(db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                    .Select(s => s.Id)
                    .ToList();


            var userPerfil = new ViewUserPerfil
            {
                IDPerfil = query.IDPerfil,
                IDUser = query.Id,
                Email = query.Email,
                Nombre = query.Nombre,
                Apellido = query.Apellido,
                Password = query.Password,
                rutaImg = query.Ruta,
                departamento = query.IDDepartamento
                // Vendor = new SelectList(helper.GetDepartamento(), "IDDepartamento", "departamento")
            };
            List<RolDto> RolDto = new List<RolDto>();

            foreach (var item in db.Roles.ToList())
            {
                //agregamos todos los roles a userDto
                var datos = new RolDto
                {
                    Name = item.Name,
                    Id = item.Id,
                    check = false
                };
                RolDto.Add(datos);
            }
            //ASIGANAMOS LOS ROLES QUE EXISTEN PARA ESE USUARIO Y LES CAMBIAMOS EL CHEK A TRUE
            for (int i = 0; i < roles.Count; i++)
            {
                foreach (var item in RolDto.Where(d => d.Id == roles[i]).ToList())
                {
                    item.check = true;
                }
            }
            ViewBag.roles = RolDto.ToList();
            ViewBag.Depatarmento = new SelectList(utl.GetDepartamento(), "IDDepartamento", "departamento");
            return PartialView(userPerfil);
        }

        [HttpPost]
        public ActionResult Edit(ViewUserPerfil userPerfil, HttpPostedFileBase FileEdit)
        {
            try
            {
                bool bsuccess = false;
                if (ModelState.IsValid)
                {
                    var result = (from u in db.Users
                                  join p in db.Perfiles on u.Id equals p.UsuarioID
                                  where u.Id == userPerfil.IDUser
                                  select new
                                  {
                                      Id = u.Id,
                                      Password = u.PasswordHash,
                                      Email = u.Email,
                                      Nombre = p.Nombre,
                                      Apellido = p.Apellido,
                                      IDPerfil = p.IDPerfil,
                                      Ruta = p.rutaImg
                                  }).SingleOrDefault();
                    string ruta = string.Empty;
                    if (FileEdit != null)
                    {
                        ruta = SaveUploadedFile(FileEdit, userPerfil.IDUser);
                    }
                    else
                    {
                        ruta = result.Ruta;
                    }
                    var userStore = new UserStore<ApplicationUser>(db);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    //SE BUSCA AL USUARIO
                    var usu = userManager.FindById(userPerfil.IDUser);
                    usu.Email = userPerfil.Email;
                    usu.Id = userPerfil.IDUser;
                    usu.UserName = userPerfil.Email;
                    //se actualiza los resultados
                    userManager.Update(usu);
                    //cambiamos el password
                    userManager.ChangePassword(userPerfil.IDUser, result.Password, userPerfil.Password);
                    //actualizamos los datos de perfiles
                    var perfiles = db.Perfiles.Find(userPerfil.IDPerfil);
                    perfiles.Nombre = userPerfil.Nombre;
                    perfiles.Apellido = userPerfil.Apellido;
                    perfiles.rutaImg = ruta;
                    perfiles.IDDepartamento = userPerfil.departamento;
                    db.SaveChanges();
                    //CAMBIAMOS LA INFORMACION DE LOS ROLES
                    var roleStore = new RoleStore<IdentityRole>(db);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    //eliminamos los roles
                    var roles = userManager.GetRoles(usu.Id);
                    userManager.RemoveFromRoles(usu.Id, roles.ToArray());

                    if (userPerfil.rol != null)
                    {
                        foreach (var varRol in userPerfil.rol)
                        {
                            var rolName = roleManager.FindById(varRol);
                            //permite hacer el insert en AspNetUserRoles
                            userManager.AddToRole(usu.Id, rolName.Name);
                        }
                    }

                    bsuccess = true;

                }
                return Json(new { success = bsuccess, Errors = errors.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Errors = errors.GetErrorsFromModelState(ModelState), JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                bool bsuccess = true;
                Perfiles perfil = db.Perfiles.Where(d => d.UsuarioID == id).SingleOrDefault();
                if (perfil != null)
                {

                    string ruta = Path.Combine(Server.MapPath("~/Images/empleados/" + id));
                    bool isExists = System.IO.Directory.Exists(ruta);

                    if (isExists)
                        Directory.Delete(ruta, true);

                    db.Perfiles.Remove(perfil);
                }
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                //se busca el usuario
                var usu = userManager.FindById(id);
                //se elimina
                userManager.Delete(usu);


                db.SaveChanges();
                return Json(new { success = bsuccess });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
    }
}