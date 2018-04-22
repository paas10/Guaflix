using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Libreria_de_Clases;
using Guaflix_1104017_1169317.Clases;

namespace Guaflix_1104017_1169317.Clases
{
    public class DataBase
    {
        private static DataBase instance;
        public static DataBase Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataBase();
                return instance;
            }
        }

        public Arbol2_3<Pelicula> ArboldePeliculasPorNombre;
        public Arbol2_3<Pelicula> ArboldePeliculasPorAño;
        public Arbol2_3<Pelicula> ArboldePeliculasPorGenero;
        public Arbol2_3<Pelicula> ArboldeSeriesPorNombre;
        public Arbol2_3<Pelicula> ArboldeSeriesPorAño;
        public Arbol2_3<Pelicula> ArboldeSeriesPorGenero;
        public Arbol2_3<Pelicula> ArboldeDocumentalesPorNombre;
        public Arbol2_3<Pelicula> ArboldeDocumentalesPorAño;
        public Arbol2_3<Pelicula> ArboldeDocumentalesPorGenero;
        public Arbol2_3<Usuario> ArboldeUsuarios;
        public Arbol2_3<Pelicula> WatchListUsuario;
        public List<string> ArchivoTexto;

        public List<Pelicula> ListadePrueba;
        public List<Usuario> ListadePruebaUser;

        public DataBase()
        {
            ArboldePeliculasPorNombre = new Arbol2_3<Pelicula>();
            ArboldePeliculasPorAño = new Arbol2_3<Pelicula>();
            ArboldePeliculasPorGenero = new Arbol2_3<Pelicula>();
            ArboldeSeriesPorNombre = new Arbol2_3<Pelicula>();
            ArboldeSeriesPorAño = new Arbol2_3<Pelicula>();
            ArboldeSeriesPorGenero = new Arbol2_3<Pelicula>();
            ArboldeDocumentalesPorNombre = new Arbol2_3<Pelicula>();
            ArboldeDocumentalesPorAño = new Arbol2_3<Pelicula>();
            ArboldeDocumentalesPorGenero = new Arbol2_3<Pelicula>();
            WatchListUsuario = new Arbol2_3<Pelicula>();

            ArboldeUsuarios = new Arbol2_3<Usuario>();

            ArchivoTexto = new List<string>();
            ListadePrueba = new List<Pelicula>();
            ListadePruebaUser = new List<Usuario>();
        }
    }
}