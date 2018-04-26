using Guaflix_1104017_1169317.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libreria_de_Clases;

namespace Guaflix_1104017_1169317.Controllers
{
    public class PeliculaController : Controller
    {
        // GET: Pelicula
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Metodo que imprime cualquier arbol en disco
        /// </summary>
        /// <param name="nArbol">Arbol a imprimir</param>
        /// <param name="URL">Direccion con nombre del archivo</param>
        public static void ImprimirArboles(int nArbol, string URL)
        {
            //Se crea un Jugador Momentaneo para pasar los datos (Carga del archivo)
            List<string> ListaEnDisco = new List<string>();

            if (nArbol < 11)
            {
                StreamWriter escritor = new StreamWriter(URL);

                switch (nArbol)
                {
                    case 1: ListaEnDisco = DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbolEnDisco(); break;
                    case 2: ListaEnDisco = DataBase.Instance.ArboldePeliculasPorAño.ObtenerArbolEnDisco(); break;
                    case 3: ListaEnDisco = DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbolEnDisco(); break;
                    case 4: ListaEnDisco = DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbolEnDisco(); break;
                    case 5: ListaEnDisco = DataBase.Instance.ArboldeSeriesPorAño.ObtenerArbolEnDisco(); break;
                    case 6: ListaEnDisco = DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbolEnDisco(); break;
                    case 7: ListaEnDisco = DataBase.Instance.ArboldeDocumentalesPorNombre.ObtenerArbolEnDisco(); break;
                    case 8: ListaEnDisco = DataBase.Instance.ArboldeDocumentalesPorAño.ObtenerArbolEnDisco(); break;
                    case 9: ListaEnDisco = DataBase.Instance.ArboldeDocumentalesPorNombre.ObtenerArbolEnDisco(); break;
                    case 10: ListaEnDisco = DataBase.Instance.ArboldeUsuarios.ObtenerArbolEnDisco(); break;
                    case 11:
                        List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
                        ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

                        foreach (var item in ListaTemporaldeUsuarios)
                        {
                            if (item.Logeado == true)
                            {
                                escritor = new StreamWriter(@"C:\" + item.Username + ".watchList");
                            }
                        }

                        ListaEnDisco = DataBase.Instance.WatchListUsuario.ObtenerArbolEnDisco();
                        break;
                }

                foreach (var linea in ListaEnDisco)
                {
                    escritor.WriteLine(linea);
                }

                escritor.Close();
                escritor.Dispose();
            }
            else
            {
                string direccion = "";
                List<Usuario> ListaTemporaldeUsuarios = new List<Usuario>();
                ListaTemporaldeUsuarios = DataBase.Instance.ArboldeUsuarios.ObtenerArbol();

                foreach (var item in ListaTemporaldeUsuarios)
                {
                    if (item.Logeado == true)
                    {
                        direccion = @"C:\" + item.Username + ".watchList";
                    }
                }

                StreamWriter escritor = new StreamWriter(direccion);

                ListaEnDisco = DataBase.Instance.WatchListUsuario.ObtenerArbolEnDisco();

                foreach (var linea in ListaEnDisco)
                {
                    escritor.WriteLine(linea);
                }

                escritor.Close();
                escritor.Dispose();
            }
            
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
        public ActionResult AgregarWatchlist(string URL, string Trailer, string Nombre, string Tipo, string Genero, int Anio)
        {
            Pelicula nuevaPelicula = new Pelicula(URL, Trailer, Nombre, Tipo, Anio, Genero);

            DataBase.Instance.WatchListUsuario.Insertar(nuevaPelicula);
            ImprimirArboles(11, @"C:\watchlist");

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

            return RedirectToAction("MisPeliculas", "Pelicula", ListaTemporaldePeliculas);
        }

        //Se va a una Pelicula especifica para ver sus especificaciones
        public ActionResult EliminarWatchlist(string URL, string Trailer, string Nombre, string Tipo, string Genero, int Anio)
        {
            Pelicula nuevaPelicula = new Pelicula(URL, Trailer, Nombre, Tipo, Anio, Genero);

            DataBase.Instance.WatchListUsuario.Eliminar(nuevaPelicula);
            ImprimirArboles(11, @"C:\watchlist");

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

            return RedirectToAction("MisPeliculas", "Pelicula", ListaTemporaldePeliculas);
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

        // GET: Pelicula/Delete/5
        public ActionResult Delete(string URL, string Trailer, string Nombre, string Tipo, string Genero)
        {
            if (Nombre == null)
            {
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

                return View(ListaTemporaldePeliculas);

            }
            else
            {
                var Anio = 0;

                if (Tipo == "Pelicula")
                {
                    foreach (var item in DataBase.Instance.ArboldePeliculasPorNombre.ObtenerArbol())
                    {
                        if (item != null)
                        {
                            if (item.Nombre == Nombre && item.URL == URL)
                            {
                                Anio = item.AniodeLanzamiento;
                            }
                        }
                    }

                    ImprimirArboles(1, @"C:\Name.movietree");
                    ImprimirArboles(2, @"C:\year.movietree");
                    ImprimirArboles(3, @"C:\gender.movietree");
                }
                else if (Tipo == "Documental")
                {
                    foreach (var item in DataBase.Instance.ArboldeDocumentalesPorNombre.ObtenerArbol())
                    {
                        if (item != null)
                        {
                            if (item.Nombre == Nombre && item.URL == URL)
                            {
                                Anio = item.AniodeLanzamiento;
                            }
                        }
                    }

                    ImprimirArboles(7, @"C:\Name.documentarytree");
                    ImprimirArboles(8, @"C:\year.documentarytree");
                    ImprimirArboles(9, @"C:\gender.documentarytree");
                }
                else if (Tipo == "Serie")
                {
                    foreach (var item in DataBase.Instance.ArboldeSeriesPorNombre.ObtenerArbol())
                    {
                        if (item != null)
                        {
                            if (item.Nombre == Nombre && item.URL == URL)
                            {
                                Anio = item.AniodeLanzamiento;
                            }
                        }

                    }

                    ImprimirArboles(4, @"C:\name.showtree");
                    ImprimirArboles(5, @"C:\year.showtree");
                    ImprimirArboles(6, @"C:\gender.showtree");
                }

                Pelicula NuevaPelicula = new Pelicula(URL, Trailer, Nombre, Tipo, Anio, Genero);

                if (NuevaPelicula.Tipo == "Pelicula")
                {
                     DataBase.Instance.ArboldePeliculasPorNombre.Eliminar(NuevaPelicula);
                     //DataBase.Instance.ArboldePeliculasPorGenero.Eliminar(NuevaPelicula);
                     DataBase.Instance.ArboldePeliculasPorAño.Eliminar(NuevaPelicula);
                    ImprimirArboles(1, @"C:\Name.movietree");
                    ImprimirArboles(2, @"C:\year.movietree");
                    ImprimirArboles(3, @"C:\gender.movietree");
                }
                else if (NuevaPelicula.Tipo == "Documental")
                {
                     DataBase.Instance.ArboldeDocumentalesPorNombre.Eliminar(NuevaPelicula);
                     //DataBase.Instance.ArboldeDocumentalesPorGenero.Eliminar(NuevaPelicula);
                     DataBase.Instance.ArboldeDocumentalesPorAño.Eliminar(NuevaPelicula);
                    ImprimirArboles(7, @"C:\Name.documentarytree");
                    ImprimirArboles(8, @"C:\year.documentarytree");
                    ImprimirArboles(9, @"C:\gender.documentarytree");
                }
                else if (NuevaPelicula.Tipo == "Serie")
                {
                    DataBase.Instance.ArboldeSeriesPorNombre.Eliminar(NuevaPelicula);
                    //DataBase.Instance.ArboldeSeriesPorGenero.Eliminar(NuevaPelicula);
                    DataBase.Instance.ArboldeSeriesPorAño.Eliminar(NuevaPelicula);
                    ImprimirArboles(4, @"C:\name.showtree");
                    ImprimirArboles(5, @"C:\year.showtree");
                    ImprimirArboles(6, @"C:\gender.showtree");
                }


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

                return RedirectToAction("MisPeliculas", ListaTemporaldePeliculas);
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
                Pelicula NuevaPelicula = new Pelicula(collection["URL"],collection["Trailer"],collection["Nombre"],collection["Tipo"],Convert.ToInt32(collection["AniodeLanzamiento"]),collection["Genero"]);

                if(NuevaPelicula.Tipo == "Pelicula")
                {
                    DataBase.Instance.ArboldePeliculasPorNombre.Insertar(NuevaPelicula);
                    DataBase.Instance.ArboldePeliculasPorAño.Insertar(NuevaPelicula);
                    DataBase.Instance.ArboldePeliculasPorGenero.Insertar(NuevaPelicula);
                    ImprimirArboles(1, @"C:\Name.movietree");
                    ImprimirArboles(2, @"C:\year.movietree");
                    ImprimirArboles(3, @"C:\gender.movietree");
                }
                else if(NuevaPelicula.Tipo == "Documental")
                {
                    DataBase.Instance.ArboldeDocumentalesPorNombre.Insertar(NuevaPelicula);
                    DataBase.Instance.ArboldeDocumentalesPorAño.Insertar(NuevaPelicula);
                    DataBase.Instance.ArboldeDocumentalesPorGenero.Insertar(NuevaPelicula);
                    ImprimirArboles(7, @"C:\Name.documentarytree");
                    ImprimirArboles(8, @"C:\year.documentarytree");
                    ImprimirArboles(9, @"C:\gender.documentarytree");
                }
                else if(NuevaPelicula.Tipo == "Serie")
                {
                    DataBase.Instance.ArboldeSeriesPorNombre.Insertar(NuevaPelicula);
                    DataBase.Instance.ArboldeSeriesPorAño.Insertar(NuevaPelicula);
                    DataBase.Instance.ArboldeSeriesPorGenero.Insertar(NuevaPelicula);
                    ImprimirArboles(4, @"C:\name.showtree");
                    ImprimirArboles(5, @"C:\year.showtree");
                    ImprimirArboles(6, @"C:\gender.showtree");
                }


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

                return RedirectToAction("MisPeliculas", ListaTemporaldePeliculas);
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
