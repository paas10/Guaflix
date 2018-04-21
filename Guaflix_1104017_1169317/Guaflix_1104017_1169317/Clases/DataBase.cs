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

        public Arbol2_3<Pelicula> ArboldePeliculas;
        public Arbol2_3<Pelicula> ArboldeSeries;
        public Arbol2_3<Pelicula> ArboldeDocumentales;
        public Arbol2_3<Usuario> ArboldeUsuarios;
        public Arbol2_3<Pelicula> WatchListUsuario;
        public List<string> ArchivoTexto;

        public List<Pelicula> ListadePrueba;
        public List<Usuario> ListadePruebaUser;

        public DataBase()
        {
            ArboldePeliculas = new Arbol2_3<Pelicula>();
            ArboldeSeries = new Arbol2_3<Pelicula>();
            ArboldeDocumentales = new Arbol2_3<Pelicula>();
            WatchListUsuario = new Arbol2_3<Pelicula>();

            ArboldeUsuarios = new Arbol2_3<Usuario>();

            ArchivoTexto = new List<string>();
            ListadePrueba = new List<Pelicula>();
            ListadePruebaUser = new List<Usuario>();
        }
    }
}