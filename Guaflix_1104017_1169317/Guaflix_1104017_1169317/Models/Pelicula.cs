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
        public int AniodeLanzamiento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El Genero de la Pelicula es Requerido")]
        public string Genero { get; set; }


        public Pelicula(string URL, string Trailer, string Nombre, string Tipo, int AniodeLanzamiento, string Genero)
        {
            this.URL = URL;
            this.Trailer = Trailer;
            this.Nombre = Nombre;
            this.Tipo = Tipo;
            this.AniodeLanzamiento = AniodeLanzamiento;
            this.Genero = Genero;
        }

        public override string ToString()
        {
            return $"{Nombre}|{Tipo}|{AniodeLanzamiento}|{Genero}";
        }

        public static int CompareByNombre(Pelicula peli1, Pelicula peli2)
        {
            int result = peli1.Nombre.CompareTo(peli2.Nombre);
            if (result == 0)
            {
                result = peli1.AniodeLanzamiento.CompareTo(peli2.AniodeLanzamiento);
                if (result == 0)
                    result = peli1.Genero.CompareTo(peli2.Genero);
            }

            return result;
        }

        public static int CompareByAño(Pelicula peli1, Pelicula peli2)
        {
            int result = peli1.AniodeLanzamiento.CompareTo(peli2.AniodeLanzamiento);
            if (result == 0)
            {
                result = peli1.Nombre.CompareTo(peli2.Nombre);
                if (result == 0)
                    result = peli1.Genero.CompareTo(peli2.Genero);
            }

            return result;
        }

        public static int CompareByGenero(Pelicula peli1, Pelicula peli2)
        {
            int result = peli1.Genero.CompareTo(peli2.Genero);
            if (result == 0)
            {
                result = peli1.Nombre.CompareTo(peli2.Nombre);
                if (result == 0)
                    result = peli1.AniodeLanzamiento.CompareTo(peli2.AniodeLanzamiento);
            }

            return result;
        }

        public int CompareTo(object obj)
        {
            int res;

            try
            {
                Pelicula pelicula = obj as Pelicula;

                res = CompareByNombre(this, pelicula);

                if (res != 0)
                    return res;
                else
                    res = CompareByAño(this, pelicula);

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