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
            UserLogin.Username = collection["Username"];
            UserLogin.Password = collection["Password"];

            if (collection["Username"] == null)
                return View();

            Usuario usuarioEncontrado = DataBase.Instance.ArboldeUsuarios.Buscar(UserLogin);

            //Se inserta el nuevo usuario al Árbol de usuarios
            if (usuarioEncontrado != null || (collection["Username"] == "admin" && collection["Password"] == "admin"))
            {
                if (usuarioEncontrado != null)
                {
                    ViewBag.Message = usuarioEncontrado.Nombre;
                    UserLogin.Logeado = true;
                }

                else
                {
                    Usuario UserAdmin = new Usuario();
                    UserAdmin.Username = collection["Username"];
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

        public ActionResult RegistrarUsuarios(FormCollection collection)
        {
            if (collection["Nombre"] != null)
            {
                Usuario Nuevo = new Usuario();
                Nuevo.Nombre = collection["Nombre"];
                Nuevo.Apellido = collection["Apellido"];
                Nuevo.Edad = Convert.ToInt32(collection["Edad"]);
                Nuevo.Username = collection["Username"];
                Nuevo.Password = collection["Password"];
                Nuevo.Logeado = false;

                DataBase.Instance.ArboldeUsuarios.Insertar(Nuevo);
                TempData["msg"] = "<script> alert('Usuario insertado con éxito');</script>";
            }
            return View();
        }

        public ActionResult MenudeDecision()
        {
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            //Se evalua la decision y se envia el nombre de usuario para mostrarlo en las vistas
            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    ViewBag.Message = item.Nombre;
                }
            }

            return View();

        }

        public ActionResult UsuarioDecision()
        {
            return View();
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