using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Guaflix_1104017_1169317.Clases;

namespace Guaflix_1104017_1169317.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(FormCollection collection)
        {
            Usuario UserLogin = new Usuario();
            UserLogin.Nombre = collection["Username"];
            UserLogin.Password = collection["Password"];

            Usuario usuarioEncontrado = DataBase.Instance.ArboldeUsuarios.Buscar(UserLogin);

            //Se inserta el nuevo usuario al Árbol de usuarios
            if (usuarioEncontrado != null || (collection["Username"] == "Admin" && collection["Password"] == "Admin") || (collection["Username"] == "admin" && collection["Password"] == "admin"))
            {
                if (usuarioEncontrado != null)
                {
                    ViewBag.Message = usuarioEncontrado.Nombre;
                    UserLogin.Logeado = true;
                }

                else
                {
                    Usuario UserAdmin = new Usuario();
                    UserAdmin.Nombre = collection["Username"];
                    UserAdmin.Password = collection["Password"];
                    UserAdmin.Nombre = "Administrador";
                    UserAdmin.Logeado = true;
                    ViewBag.Message = "Admin";

                    DataBase.Instance.ArboldeUsuarios.Insertar(UserAdmin);
                }
                    
                if (collection["Username"] == "Admin" || collection["Username"] == "admin")
                {
                    return View("MenudeDecision");
                }
                else
                {
                    return View("UsuarioDecision");
                }
            }
            else
            {
                //Si no es correcto se envia un mensaje de Error
                TempData["msg1"] = "<script> alert('El Usuario o La Contraseña es Incorrecta');</script>";
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}