using Guaflix_1104017_1169317.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Guaflix_1104017_1169317.Controllers
{
    public class PeliculaController : Controller
    {
        // GET: Pelicula
        public ActionResult Index()
        {
            return View();
        }

        //Se Escribe el Buffer con el Archivo de Texto
        public void imprimirArchivo()
        {

            StreamWriter escritor = new StreamWriter(@"C:\Users\Admin\Desktop\Bitacora.txt");

            foreach (var linea in DataBase.Instance.ArchivoTexto)
            {
                escritor.WriteLine(linea);
            }
            escritor.Close();
        }

        //Se Acceden a las Peliculas Almacenadas en el Arbol
        public ActionResult MisPeliculas(string NombreUsuario, List<Pelicula> Lista)
        {
            //Se crean listas temporales de usuarios y peliculas para usarlas despues
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            List<Pelicula> ListaTemporaldePeliculas = new List<Pelicula>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            foreach (var item in DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListaTemporaldePeliculas.Add(item);
                }
            }

            foreach (var item in DataBase.Instance.ArboldeDocumentalesPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListaTemporaldePeliculas.Add(item);
                }
            }

            foreach (var item in DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListaTemporaldePeliculas.Add(item);
                }
            }

            //se evalua si el usuario esta logeado
            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    var Mensaje = item.Nombre + " "+ item.Apellido;
                    ViewBag.Message = Mensaje;
                }
            }
            if (ListaTemporaldePeliculas == null)
            {
                //Se envia un mensaje de error si el usuario no tiene peliculas agregadas
                TempData["msg"] = "<script> alert('No Tienes Peliculas Agregadas, Porfavor Agrega Peliculas Antes para ver tu Inicio');</script>";
                return RedirectToAction("MenudeDecision", "Home");
            }
            else
            {
                if (Lista != null)
                {
                    return View(Lista);
                }
                //Se envia la lista temporal de peliculas a la vista
                return View(ListaTemporaldePeliculas);
            }
        }

        //Se va a una Pelicula especifica para ver sus especificaciones
        [ValidateInput(false)]
        public ActionResult AgregarWatchlist(string URL, string Trailer, string Nombre, string Tipo, string Genero, int Anio)
        {
            Pelicula nuevaPelicula = new Pelicula(URL, Trailer, Nombre, Tipo, Anio, Genero);

            DataBase.Instance.WatchListUsuario.Insertar(nuevaPelicula);

            List<Pelicula> ListaTemporaldePeliculas = new List<Pelicula>();

            foreach (var item in DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListaTemporaldePeliculas.Add(item);
                }
            }

            foreach (var item in DataBase.Instance.ArboldeDocumentalesPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListaTemporaldePeliculas.Add(item);
                }
            }

            foreach (var item in DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListaTemporaldePeliculas.Add(item);
                }
            }

            return RedirectToAction("VerPelicula", "PeliculaController", ListaTemporaldePeliculas);
        }

        //Se va a una Pelicula especifica para ver sus especificaciones
        [ValidateInput(false)]
        public ActionResult VerPelicula(string URL, string Trailer, string Nombre, string Tipo, string Genero, int Anio)
        {
            //Se Crea una lista temporal de usuario para evaluar cual esta logeado
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    var Mensaje = item.Nombre + " " + item.Apellido;
                    ViewBag.Message = Mensaje;
                }
            }

            //Se manda la lista especial a la vista para poder visualizar la unica pelicula
            Pelicula NuevaPelicula = new Pelicula(URL, Trailer, Nombre, Tipo, Anio, Genero);
            List<Pelicula> ListadePeliculas = new List<Pelicula>();
            ListadePeliculas.Add(NuevaPelicula);
            Session.Add("URL", NuevaPelicula.Trailer);
            return View(ListadePeliculas);
        }

        //Buscador de Peliculas
        public ActionResult BuscarPelicula(string Tipo, string Search)
        {
            //Se Crea una lista temporal de usuario para evaluar cual esta logeado
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            List<Pelicula> ListaTemporaldePeliculas = new List<Pelicula>();
            List<Pelicula> ListaTemporaldeSeries = new List<Pelicula>();
            List<Pelicula> ListaTemporaldeDocumentales = new List<Pelicula>();
            List<Pelicula> ListaGeneral = new List<Pelicula>();

            ListaTemporaldePeliculas = DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbol();
            ListaTemporaldeSeries = DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbol();
            ListaTemporaldeDocumentales = DataBase.Instance.ArboldeDocumentalesPorNombre.ObtenerArbol();

            if (Tipo == "Nombre" && Search != null)
            {
                foreach (var item in ListaTemporaldePeliculas)
                {
                    if (item.Nombre == Search)
                    {
                        ListaGeneral.Add(item);
                    }
                }

                foreach (var item in ListaTemporaldeSeries)
                {
                    if (item.Nombre == Search)
                    {
                        ListaGeneral.Add(item);
                    }
                }

                foreach (var item in ListaTemporaldeDocumentales)
                {
                    if (item.Nombre == Search)
                    {
                        ListaGeneral.Add(item);
                    }
                }
            }
            else if (Tipo == "AniodeLanzamiento" && Search != null)
            {
                foreach (var item in ListaTemporaldePeliculas)
                {
                    if (item.AniodeLanzamiento == int.Parse(Search))
                    {
                        ListaGeneral.Add(item);
                    }
                }

                foreach (var item in ListaTemporaldeSeries)
                {
                    if (item.AniodeLanzamiento == int.Parse(Search))
                    {
                        ListaGeneral.Add(item);
                    }
                }

                foreach (var item in ListaTemporaldeDocumentales)
                {
                    if (item.AniodeLanzamiento == int.Parse(Search))
                    {
                        ListaGeneral.Add(item);
                    }
                }
            }
            else if (Tipo == "Genero" && Search != null)
            {
                foreach (var item in ListaTemporaldePeliculas)
                {
                    if (item.Genero == Search)
                    {
                        ListaGeneral.Add(item);
                    }
                }

                foreach (var item in ListaTemporaldeSeries)
                {
                    if (item.Genero == Search)
                    {
                        ListaGeneral.Add(item);
                    }
                }

                foreach (var item in ListaTemporaldeDocumentales)
                {
                    if (item.Genero == Search)
                    {
                        ListaGeneral.Add(item);
                    }
                }
            }
            else if (ListaGeneral.Count() == 0)
            {
                if (Tipo != null && Search != null)
                {
                    TempData["msg"] = "<script> alert('El Dato que Buscas no Existe');</script>";
                }
                else
                {
                    return View();
                }
            }

            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    ViewBag.Message = item.Nombre;
                }
            }

            return View("MisPeliculas", ListaGeneral);
        }

        //Accion que Carga el .Json de usuarios desde la pestanaña de "Mi Usuario"
        public ActionResult json()
        {
            List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
            ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

            //Se crea una lista temporal de usuarios para ver si esta logeada o no
            foreach (var item in ListaTemporaldeUsuarios)
            {
                if (item.Logeado == true)
                {
                    var Mensaje = item.Nombre + " " + item.Apellido;
                    ViewBag.Message = Mensaje;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Aca se hace el Ingreso por medio de Archivo de Texto, ya que el Boton de Result esta Linkeado.
        public ActionResult json(HttpPostedFileBase file, int? Tipo)
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
            var ListadeUsuarios = JsonConvert.DeserializeObject<List<Usuario>>(Dato);

            //Se insertan las peliculas en el arbol y se clasifican
            foreach (var item in ListadeUsuarios)
            {
                DataBase.Instance.ArboldeUsuarios.Insertar(item);
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

            TempData["msg"] = "<script> alert('Los Usuarios han sido Cargados Correctamente');</script>";
            return RedirectToAction("MiUsuario","Home");
        }


        public ActionResult EliminarPelis()
        {
            List<Pelicula> ListadePeliculas = new List<Pelicula>();

            foreach (var item in DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListadePeliculas.Add(item);
                }
            }

            foreach (var item in DataBase.Instance.ArboldeDocumentalesPorAño.ObtenerArbol())
            {
                if (item != null)
                {
                    ListadePeliculas.Add(item);
                }
            }

            foreach (var item in DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbol())
            {
                if (item != null)
                {
                    ListadePeliculas.Add(item);
                }
            }


            return View(ListadePeliculas);
        }

        // GET: Pelicula/Delete/5
        public ActionResult Delete(string Nombre, int Anio)
        {
            List<Pelicula> Lista = new List<Pelicula>();
            List<Pelicula> ListaFinal = new List<Pelicula>();

            foreach (var item in DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbol())
            {
                Lista.Add(item);
            }

            foreach (var item in DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbol())
            {
                Lista.Add(item);
            }

            foreach (var item in DataBase.Instance.ArboldeDocumentalesPorNombre.ObtenerArbol())
            {
                Lista.Add(item);
            }

            if (Nombre == null)
            {
                return View(Lista);
            }
            else
            {
                foreach (var item in Lista)
                {
                    if (item.Nombre != Nombre)
                    {
                        if (item.AniodeLanzamiento != Anio)
                        {
                            ListaFinal.Add(item);
                        }
                    }
                }
                return View(ListaFinal);
            }
        }
       
        // GET: Pelicula/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pelicula/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pelicula/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pelicula/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

       
    }
}
