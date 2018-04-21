using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_de_Clases
{
    public class Nodo2_3<T>
    {
        public T[] Elementos = new T[2];
        public Nodo2_3<T>[] Hijos = new Nodo2_3<T>[3];
        public Nodo2_3<T> Padre;


        public Nodo2_3()
        {
            this.Padre = null;
        }

        public bool EsHoja
        {
            get
            {
                return Hijos[0] == null && Hijos[1] == null && Hijos[2] == null;
            }
        }

        public string PosicionHijo
        {
            get
            {
                if (Padre == null)
                    return "No tiene padre";
                else if (Padre.Hijos[0] == this)
                    return "Hijo Izquierdo";
                else if (Padre.Hijos[1] == this)
                    return "Hijo Central";
                else
                    return "Hijo Derecho";
            }
        }

        public int NumerodeElementos()
        {
            int count = 0;
            for (int i = 0; i < this.Elementos.Length; i++)
            {
                if (this.Elementos[i] != null)
                    count++;
            }
            return count;
        }

        public Nodo2_3(T valor)
        {
            this.Elementos[0] = valor;
            for (int i = 0; i < 3; i++)
            {
                Hijos[i] = null;
            }
        }
    }
}
