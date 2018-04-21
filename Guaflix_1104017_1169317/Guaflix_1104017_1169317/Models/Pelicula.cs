using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Guaflix_1104017_1169317.Clases
{
    public class Pelicula : IComparable
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El URL del Film es Requerido")]
        public string URL { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El URL del Trailer es Requerido")]
        public string Trailer { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Nombre de La Pelicula es Requerido")]
        public string Nombre { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Tipo del Film es Requerido")]
        public string Tipo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Año de Lazamiento de La Pelicula es Requerido")]
        public string AniodeLanzamiento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Genero de la Pelicula es Requerido")]
        public string Genero { get; set; }

        public Pelicula(string URL, string Trailer, string Nombre, string Tipo, string AniodeLanzamiento, string Genero)
        {
            this.URL = URL;
            this.Trailer = Trailer;
            this.Nombre = Nombre;
            this.Tipo = Tipo;
            this.AniodeLanzamiento = AniodeLanzamiento;
            this.Genero = Genero;
        }

        public int CompareByNombredePelicula(Pelicula pelicula)
        {
            return Nombre.CompareTo(pelicula.Nombre);
        }

        public int CompareByAño(Pelicula pelicula)
        {
            return AniodeLanzamiento.CompareTo(pelicula.AniodeLanzamiento);
        }

        public int CompareByGenero(Pelicula pelicula)
        {
            return Genero.CompareTo(pelicula.Genero);
        }

        public int CompareTo(object obj)
        {
            int res;

            try
            {
                Pelicula pelicula = obj as Pelicula;

                res = CompareByNombredePelicula(pelicula);

                if (res != 0)
                    return res;
                else
                    res = CompareByAño(pelicula);

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public delegate int Comparar(Pelicula Pelicula);

        public int CompareTo(Pelicula pelicula, Comparar criterio)
        {
            return criterio(pelicula);
        }
    }
}