using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Guaflix_1104017_1169317.Clases;
using Libreria_de_Clases;
using Newtonsoft.Json;

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
            if (usuarioEncontrado != null || (collection["Username"] == "admin" && collection["Password"] == "admin") || (collection["Username"] == "Admin" && collection["Password"] == "Admin"))
            {
                if (usuarioEncontrado != null)
                {
                    if (usuarioEncontrado.Password == collection["Password"])
                    {
                        var Mensaje = usuarioEncontrado.Nombre + " " + usuarioEncontrado.Apellido;
                        ViewBag.Message = Mensaje;
                        foreach (var item in DataBase.Instance.ArboldeUsuarios.ObtenerArbol())
                        {
                            if (item.Username == usuarioEncontrado.Username)
                            {
                                item.Logeado = true;
                            }
                        }
                 
                    }
                    else
                    {
                        //Si no es correcto se envia un mensaje de Error
                        TempData["msg"] = "<script> alert('El Usuario o La Contraseña es Incorrecta');</script>";
                        return View();
                    }
                  
                }

                else
                {
                    Usuario UserAdmin = new Usuario();
                    UserAdmin.Username = collection["Username"];
                    UserAdmin.Password = collection["Password"];
                    UserAdmin.Nombre = "Administrador";
                    UserAdmin.Logeado = true;
                    ViewBag.Message = "Administrador";

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
                TempData["msg"] = "<script> alert('El Usuario o La Contraseña es Incorrecta');</script>";
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
                TempData["msg"] = "<script> alert('Usuario ingresado con éxito');</script>";
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
                    var Mensaje = item.Nombre + item.Apellido;
                    ViewBag.Message = Mensaje;
                }
            }

            return View();

        }

        public ActionResult UsuarioDecision()
        {
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            //Se evalua la decision y se envia el nombre de usuario para mostrarlo en las vistas
            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    var Mensaje = item.Nombre + item.Apellido;
                    ViewBag.Message = Mensaje;
                }
            }
            return View();
        }

        public ActionResult MiUsuario()
        {

            var EsAdmin = false;
            //Se crea un nuevo Usuario
            Usuario Nuevo = new Usuario();
            //Se crea una lista temporal de usuarios y una lista de usuarios para poder pasarla posteriormente.
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            List<Usuario> ListadeUsuarios = new List<Usuario>();

            //a la lista temporal de usuarios se le asignan los usuarios que posee el arbol.
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            //Se busca el usuario que esta logeado y es el que se envia a la vista.
            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    var Mensaje = item.Nombre + " " + item.Apellido;
                    ViewBag.Message = Mensaje;
                    if(item.Nombre == "Administrador" || item.Nombre == "administrador")
                    {
                        ViewBag.Message = item.Nombre;
                        EsAdmin = true;
                    }
                    else
                    {
                        EsAdmin = false;
                    }
                    ListadeUsuarios.Add(item);
                }
            }


            if (EsAdmin == false)
            {
                return View(ListadeUsuarios);
            }
            else
            {
                return View(ListaTemporaldeUsuarios);
            }
            
        }

        public ActionResult CerrarSesion()
        {
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            //Se deslogea al usuario que coincide con las especificaciones
            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    item.Logeado = false;
                }
            }
            return View("Index");
        }

        public ActionResult UploadFile()
        {
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            //Se crea una lista temporal de usuarios para ver si esta logeada o no
            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    ViewBag.Message = item.Nombre;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Aca se hace el Ingreso por medio de Archivo de Texto, ya que el Boton de Result esta Linkeado.
        public ActionResult UploadFile(HttpPostedFileBase file, int? Tipo)
        {
            //Se valida el .json
            if (Path.GetExtension(file.FileName) != ".json")
            {
                //Aca se debe de Agregar una Vista de Error, o de Datos No Cargados
                TempData["msg"] = "<script> alert('Error El Archivo Cargado No es de Tipo Json');</script>";
                return RedirectToAction("Error", "Shared");
            }

            Stream Direccion = file.InputStream;
            //Se lee el Archivo que se subio, por medio del Lector

            StreamReader Lector = new StreamReader(Direccion, System.Text.Encoding.UTF8);
            //El Archivo se lee en una linea para luego ingresarlo

            string Dato = "";
            Dato = Lector.ReadToEnd();

            //Se deserealiza el objeto por medio de .json obteniendo una lista de peliculas
            var ListadePeliculasGeneral = JsonConvert.DeserializeObject<List<Pelicula>>(Dato);

            //Se insertan las peliculas en el arbol y se clasifican
            foreach (var item in ListadePeliculasGeneral)
            {
                if (item.Tipo == "Pelicula")
                {
                    DataBase.Instance.ArboldePeliculasPorNombre.Insertar(item);
                    DataBase.Instance.ArboldePeliculasPorAño.Insertar(item);
                    //DataBase.Instance.ArboldePeliculasPorGenero.Insertar(item);
                }
                else if (item.Tipo == "Serie")
                {
                    DataBase.Instance.ArboldeSeriesPorNombre.Insertar(item);
                    DataBase.Instance.ArboldeSeriesPorGenero.Insertar(item);
                    //DataBase.Instance.ArboldeSeriesPorAño.Insertar(item);
                }
                else if (item.Tipo == "Documental")
                {
                    DataBase.Instance.ArboldeDocumentalesPorNombre.Insertar(item);
                    DataBase.Instance.ArboldeDocumentalesPorGenero.Insertar(item);
                    //DataBase.Instance.ArboldeDocumentalesPorAño.Insertar(item);
                }
            }

            //Se crea una lista temporal de usuarios para identificar cual esta logeado
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    ViewBag.Message = item.Nombre;
                }
            }

            return View("MenudeDecision");
        }
    }
}