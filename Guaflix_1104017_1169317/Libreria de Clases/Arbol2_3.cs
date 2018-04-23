using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libreria_de_Clases
{
    public class Arbol2_3<T> where T : IComparable
    {
        public Nodo2_3<T> Raiz;
        private Nodo2_3<T> RaizSecundaria;
        private T NuevoOriginal;
        private bool TrabajarSobreSecundaria = false;
        private bool terminado = false;
        private int nElementos;
        // Determinante indica de donde viene el hijo para dividir bien los nodos en una doble subida
        private string determinante;
        private bool[] HijosCorrectos = new bool[3];

        private List<T> Noditos = new List<T>();

        public Arbol2_3()
        {
            this.Raiz = null;
            this.nElementos = 0;
        }

    
        public void Insertar(T vNuevo)
        {
            this.RaizSecundaria = null;
            terminado = false;
            NuevoOriginal = vNuevo;
            HijosCorrectos[0] = true;
            HijosCorrectos[1] = true;
            HijosCorrectos[2] = true;
            Noditos = new List<T>();

            Insertar(vNuevo, ref Raiz);
            nElementos += 1;

            if (TrabajarSobreSecundaria == true)
            {
                Raiz = RaizSecundaria;
                TrabajarSobreSecundaria = false;
            }

            CorregirPadres(ref Raiz);
        }

        private T Insertar(T vNuevo, ref Nodo2_3<T> nAuxiliar)
        {
            // Si el nodo esta vacío (raiz inicialmente) únicamente se inserta el valor en el nodo.
            if (nAuxiliar == null)
            {
                Nodo2_3<T> nNuevo = new Nodo2_3<T>(vNuevo);
                nAuxiliar = nNuevo;
            }
            // Si es un nodo hoja se puede insertar el primer o segundo valor del nodo.
            else if (nAuxiliar.EsHoja == true)
            {
                return InsertarAca(ref nAuxiliar, vNuevo);
            }
            // Si la siguiente condicion se cumple quiere decir que solo hay opción de hijo izquierdo o derecho.
            else if (nAuxiliar.Elementos[1] == null)
            {
                // El valor nuevo es MENOR que la llave padre, se debe dirigir al hijo izquierdo.
                if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == 1)
                {
                    determinante = "Hijo Izquierdo";
                    T valorTemp = Insertar(vNuevo, ref nAuxiliar.Hijos[0]);

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);

                }
                // El valor nuevo es MAYOR que la llave padre, se debe dirigir al hijo derecho.
                else if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == -1)
                {
                    determinante = "Hijo Derecho";
                    T valorTemp = Insertar(vNuevo, ref nAuxiliar.Hijos[2]);

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);

                }

            }
            // Si la siguiente condicion se cumple quiere decir que hay opción hijo izquierdo, derecho o central.
            else if (nAuxiliar.Elementos[0] != null && nAuxiliar.Elementos[1] != null)
            {
                // El valor nuevo es MENOR que la llave padre izquierda, se debe dirigir al hijo izquierdo.
                if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == 1)
                {
                    determinante = "Hijo Izquierdo";
                    T valorTemp = Insertar(vNuevo, ref nAuxiliar.Hijos[0]);

                    if (terminado == true)
                        return default(T);

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);
                }
                // El valor nuevo es MAYOR que la llave padre izquierda y MENOR que la llave padre derecha, se debe dirigir al hijo central.
                if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == -1 && nAuxiliar.Elementos[1].CompareTo(vNuevo) == 1)
                {
                    determinante = "Hijo Central";
                    T valorTemp = Insertar(vNuevo, ref nAuxiliar.Hijos[1]);

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);
                }
                // El valor nuevo es MAYOR que la llave padre derecha, se debe dirigir al hijo derecho.
                else if (nAuxiliar.Elementos[1].CompareTo(vNuevo) == -1)
                {
                    determinante = "Hijo Derecho";
                    T valorTemp = Insertar(vNuevo, ref nAuxiliar.Hijos[2]);

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);
                }
            }

            return default(T);
        }

        private T InsertarAca(ref Nodo2_3<T> nAuxiliar, T vNuevo)
        {
            // Si el nodo únicamente tiene un valor, se inserta el segundo donde corresponde

            // FUNCION EXPERIMENTAL PARA METER A LOS HIJITOS
            if (nAuxiliar.Elementos[1] == null)
            {
                if (HijosCorrectos[0] == false || HijosCorrectos[1] == false || HijosCorrectos[2] == false)
                {
                    if (nAuxiliar.Elementos[0] == null)
                    {
                        nAuxiliar.Elementos[0] = vNuevo;
                    }
                    else
                    {
                        if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == -1)
                        {
                            nAuxiliar.Elementos[1] = vNuevo;
                        }
                        else if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == 1)
                        {
                            T valorAuxiliar = nAuxiliar.Elementos[0];
                            nAuxiliar.Elementos[1] = valorAuxiliar;
                            nAuxiliar.Elementos[0] = vNuevo;
                        }
                    }

                }

                if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == -1)
                {
                    nAuxiliar.Elementos[1] = vNuevo;
                }
                else if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == 1)
                {
                    T valorAuxiliar = nAuxiliar.Elementos[0];
                    nAuxiliar.Elementos[1] = valorAuxiliar;
                    nAuxiliar.Elementos[0] = vNuevo;
                }
            }
            // Si el nodo tiene dos valores, se ordenan los valores para subir el valor central.
            // Por deduccion, si el elemento derecho no está vacío... el nodo está lleno.
            else
            {
                if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == 1)
                {
                    // subir nAuxiliar.Elementos[0]

                    // Si el papá del actual es nulo, tiene que crearse un nuevo nodo que sea papi para el acutual.
                    if (nAuxiliar.Padre == null)
                    {
                        Nodo2_3<T> nPadre = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                        Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>(vNuevo);
                        Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                        nHijoIzquierdo.Padre = nPadre;
                        nHijoDerecho.Padre = nPadre;

                        nPadre.Hijos[0] = nHijoIzquierdo;
                        nPadre.Hijos[2] = nHijoDerecho;

                        int cont = 0;
                        ContarLLaves(nPadre, ref cont);

                        if (nElementos == cont)
                        {
                            Raiz = nPadre;
                        }
                        else
                        {
                            TrabajarSobreSecundaria = true;
                            RaizSecundaria = nPadre;
                        }

                    }
                    // si si existe papá y tiene espacio el valor se mete en él y se reorganizan los hijos.
                    else
                    {
                        T subir = nAuxiliar.Elementos[0];
                        nAuxiliar.Elementos[0] = vNuevo;

                        InsertarAca(ref nAuxiliar.Padre, subir);

                        if (terminado == true)
                            return default(T);

                        // Si en el nodo que se encuentra es un hijo izquierdo se acomodan la nueva distribucion de los hijos dependiendo de este criterio
                        // Es exactamente lo mismo con las demás opciones.
                        if (nAuxiliar.PosicionHijo == "Hijo Izquierdo")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[2].Hijos[0] = Raiz.Hijos[1];
                                RaizSecundaria.Hijos[2].Hijos[2] = Raiz.Hijos[2];
                            }

                            Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>();
                            Nodo2_3<T> nHijoCentral = new Nodo2_3<T>();
                            Nodo2_3<T> nHijoDerecho;

                            if (nAuxiliar.Padre.Hijos[1] != null)
                                nHijoDerecho = nAuxiliar.Padre.Hijos[1];
                            else
                                nHijoDerecho = nAuxiliar.Padre.Hijos[2];

                            try
                            {
                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nAuxiliar);
                                nHijoCentral = nodoTemp1;
                                nHijoCentral.Elementos[0] = nHijoCentral.Elementos[1];
                                nHijoCentral.Elementos[1] = default(T);
                                nHijoCentral.Hijos[0] = nHijoCentral.Hijos[1];
                                nHijoCentral.Hijos[1] = null;

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nAuxiliar);
                                nHijoIzquierdo = nodoTemp2;
                                nHijoIzquierdo.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp3 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp3, nHijoIzquierdo.Hijos[0]);
                                Nodo2_3<T> nodoTemp4 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp4, nHijoIzquierdo.Hijos[0]);

                                nHijoIzquierdo.Hijos[0] = nodoTemp3;
                                nHijoIzquierdo.Hijos[0].Elementos[1] = default(T);

                                nHijoIzquierdo.Hijos[2] = nodoTemp4;
                                nHijoIzquierdo.Hijos[2].Elementos[0] = nHijoIzquierdo.Hijos[2].Elementos[1];
                                nHijoIzquierdo.Hijos[2].Elementos[1] = default(T);

                                nHijoIzquierdo.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoIzquierdo = new Nodo2_3<T>(vNuevo);
                                nHijoCentral = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                            }

                            // FUNC EXPERIMENTAL
                            try
                            {
                                HijosCorrectos[1] = verificarHijosCorrectos(nHijoIzquierdo);
                                HijosCorrectos[2] = verificarHijosCorrectos(nHijoCentral);

                                // ACA HACER QUE GUARDE LOS HIJOS EN LA LISTA / LIMPIAR LOS HIJOS INSERVIBLES
                                if (HijosCorrectos[0] == false)
                                    GuardarHijos(nHijoIzquierdo);
                                else if (HijosCorrectos[1] == false)
                                    GuardarHijos(nHijoCentral);
                            }
                            catch
                            {
                                HijosCorrectos[0] = true;
                                HijosCorrectos[1] = true;
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoCentral.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[1] = nHijoCentral;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = nHijoIzquierdo;
                                RaizSecundaria.Hijos[0].Hijos[2] = nHijoCentral;

                                int cant = 0;
                                ContarLLaves(RaizSecundaria, ref cant);
                                if (nElementos == cant && determinante == nAuxiliar.PosicionHijo)
                                    terminado = true;
                            }

                            try
                            {
                                if (nHijoIzquierdo.Hijos[0].Hijos[0] != null)
                                {
                                    Nodo2_3<T> temp = new Nodo2_3<T>();
                                    copiarNodo(ref temp, nHijoIzquierdo.Hijos[0].Hijos[0]);

                                    Nodo2_3<T> vacio1 = new Nodo2_3<T>(temp.Elementos[0]);
                                    Nodo2_3<T> vacio2 = new Nodo2_3<T>(temp.Elementos[1]);

                                    nHijoIzquierdo.Hijos[0].Hijos[0] = vacio1;
                                    nHijoIzquierdo.Hijos[0].Hijos[1] = null;
                                    nHijoIzquierdo.Hijos[0].Hijos[2] = vacio2;


                                    Nodo2_3<T> temp2 = new Nodo2_3<T>();
                                    copiarNodo(ref temp2, nHijoIzquierdo.Hijos[2].Hijos[1]);
                                    nHijoIzquierdo.Hijos[2].Hijos[0] = temp2;
                                    nHijoIzquierdo.Hijos[2].Hijos[1] = null;

                                }

                                terminado = true;
                            }
                            catch { }

                            //LIMPIAR LOS HIJOS INSERVIBLES
                            if (HijosCorrectos[0] == false)
                                LimpiarHijos(nHijoIzquierdo);
                            else if (HijosCorrectos[1] == false)
                                LimpiarHijos(nHijoCentral);


                            if (HijosCorrectos[0] == false || HijosCorrectos[1] == false)
                            {
                                foreach (var item in Noditos)
                                    Insertar(item);

                                HijosCorrectos[0] = true;
                                HijosCorrectos[1] = true;
                            }

                        }
                        else if (nAuxiliar.PosicionHijo == "Hijo Central")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = Raiz.Hijos[0];
                                RaizSecundaria.Hijos[2].Hijos[2] = Raiz.Hijos[2];
                            }

                            Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                            Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);

                            try
                            {
                                copiarNodo(ref nHijoIzquierdo, nAuxiliar);
                                nHijoIzquierdo.Elementos[1] = default(T);
                                nHijoIzquierdo.Hijos[2] = nHijoIzquierdo.Hijos[1];
                                nHijoIzquierdo.Hijos[1] = null;

                                copiarNodo(ref nHijoDerecho, nAuxiliar);
                                nHijoDerecho.Elementos[0] = nHijoDerecho.Elementos[1];
                                nHijoDerecho.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[0] = nodoTemp1;
                                nHijoDerecho.Hijos[0].Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[2] = nodoTemp2;
                                nHijoDerecho.Hijos[2].Elementos[0] = nHijoDerecho.Hijos[2].Elementos[1];
                                nHijoDerecho.Hijos[2].Elementos[1] = default(T);
                                nHijoDerecho.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                                nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[0].Hijos[2] = nHijoIzquierdo;
                                RaizSecundaria.Hijos[2].Hijos[0] = nHijoDerecho;

                                terminado = true;
                            }
                        }
                        else if (nAuxiliar.PosicionHijo == "Hijo Derecho")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = Raiz.Hijos[0];
                                RaizSecundaria.Hijos[0].Hijos[2] = Raiz.Hijos[1];
                            }

                            Nodo2_3<T> nHijoIzquierdo = nAuxiliar.Padre.Hijos[1];

                            if (nAuxiliar.Padre.Hijos[1] != null)
                                nHijoIzquierdo = nAuxiliar.Padre.Hijos[1];
                            else
                                nHijoIzquierdo = nAuxiliar.Padre.Hijos[0];

                            Nodo2_3<T> nHijoCentral = new Nodo2_3<T>();
                            Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>();

                            try
                            {
                                copiarNodo(ref nHijoCentral, nAuxiliar);
                                nHijoCentral.Elementos[1] = default(T);
                                nHijoCentral.Hijos[2] = nHijoCentral.Hijos[1];
                                nHijoCentral.Hijos[1] = null;

                                copiarNodo(ref nHijoDerecho, nAuxiliar);
                                nHijoDerecho.Elementos[0] = nHijoDerecho.Elementos[1];
                                nHijoDerecho.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[0] = nodoTemp1;
                                nHijoDerecho.Hijos[0].Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[2] = nodoTemp2;
                                nHijoDerecho.Hijos[2].Elementos[0] = nHijoDerecho.Hijos[2].Elementos[1];
                                nHijoDerecho.Hijos[2].Elementos[1] = default(T);
                                nHijoDerecho.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoCentral = new Nodo2_3<T>(vNuevo);
                                nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                            }

                            // FUNC EXPERIMENTAL
                            try
                            {
                                HijosCorrectos[1] = verificarHijosCorrectos(nHijoCentral);
                                HijosCorrectos[2] = verificarHijosCorrectos(nHijoDerecho);

                                // ACA HACER QUE GUARDE LOS HIJOS EN LA LISTA / LIMPIAR LOS HIJOS INSERVIBLES
                                if (HijosCorrectos[1] == false)
                                    GuardarHijos(nHijoCentral);
                                if (HijosCorrectos[2] == false)
                                    GuardarHijos(nHijoDerecho);
                            }
                            catch
                            {
                                HijosCorrectos[1] = true;
                                HijosCorrectos[2] = true;
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoCentral.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[1] = nHijoCentral;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[2].Hijos[0] = nHijoCentral;
                                RaizSecundaria.Hijos[2].Hijos[2] = nHijoDerecho;

                                int cant = 0;
                                ContarLLaves(RaizSecundaria, ref cant);
                                if (nElementos == cant && determinante == nAuxiliar.PosicionHijo)
                                    terminado = true;
                            }

                            try
                            {
                                if (nHijoDerecho.Hijos[0].Hijos[0] != null)
                                {
                                    verificarHijos(ref nHijoDerecho.Hijos[0], "Hijo Izquierdo");
                                    verificarHijos(ref nHijoDerecho.Hijos[2], "Hijo Derecho");
                                }

                                terminado = true;
                            }
                            catch { }

                            //LIMPIAR LOS HIJOS INSERVIBLES
                            if (HijosCorrectos[1] == false)
                                LimpiarHijos(nHijoCentral);
                            if (HijosCorrectos[2] == false)
                                LimpiarHijos(nHijoDerecho);


                            if (HijosCorrectos[1] == false || HijosCorrectos[2] == false)
                            {
                                foreach (var item in Noditos)
                                    Insertar(item, ref Raiz);

                                HijosCorrectos[1] = true;
                                HijosCorrectos[2] = true;
                            }
                        }

                    }
                }
                else if (nAuxiliar.Elementos[0].CompareTo(vNuevo) == -1 && nAuxiliar.Elementos[1].CompareTo(vNuevo) == 1)
                {
                    // Subir vNuevo

                    // Si el papá del actual es nulo, tiene que crearse un nuevo nodo que sea papi para el acutual.
                    if (nAuxiliar.Padre == null)
                    {
                        Nodo2_3<T> nPadre = new Nodo2_3<T>(vNuevo);
                        Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                        Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);

                        nHijoIzquierdo.Padre = nPadre;
                        nHijoDerecho.Padre = nPadre;

                        nPadre.Hijos[0] = nHijoIzquierdo;
                        nPadre.Hijos[2] = nHijoDerecho;

                        int cont = 0;
                        ContarLLaves(nPadre, ref cont);

                        if (nElementos == cont)
                        {
                            Raiz = nPadre;
                        }
                        else
                        {
                            TrabajarSobreSecundaria = true;
                            RaizSecundaria = nPadre;
                        }

                    }
                    // si si existe papá y tiene espacio el valor se mete en él y se reorganizan los hijos.
                    else
                    {
                        InsertarAca(ref nAuxiliar.Padre, vNuevo);

                        if (terminado == true)
                            return default(T);

                        // Si en el nodo que se encuentra es un hijo izquierdo se acomodan la nueva distribucion de los hijos dependiendo de este criterio
                        // Es exactamente lo mismo con las demás opciones.
                        if (nAuxiliar.PosicionHijo == "Hijo Izquierdo")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[2].Hijos[0] = Raiz.Hijos[1];
                                RaizSecundaria.Hijos[2].Hijos[2] = Raiz.Hijos[2];
                            }

                            Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                            Nodo2_3<T> nHijoCentral = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                            Nodo2_3<T> nHijoDerecho;

                            if (nAuxiliar.Padre.Hijos[1] != null)
                                nHijoDerecho = nAuxiliar.Padre.Hijos[1];
                            else
                                nHijoDerecho = nAuxiliar.Padre.Hijos[2];

                            try
                            {
                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nAuxiliar);
                                nHijoCentral = nodoTemp1;
                                nHijoCentral.Elementos[0] = nHijoCentral.Elementos[1];
                                nHijoCentral.Elementos[1] = default(T);
                                nHijoCentral.Hijos[0] = nHijoCentral.Hijos[1];
                                nHijoCentral.Hijos[1] = null;

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nAuxiliar);
                                nHijoIzquierdo = nodoTemp2;
                                nHijoIzquierdo.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp3 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp3, nHijoIzquierdo.Hijos[0]);
                                Nodo2_3<T> nodoTemp4 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp4, nHijoIzquierdo.Hijos[0]);

                                nHijoIzquierdo.Hijos[0] = nodoTemp3;
                                nHijoIzquierdo.Hijos[0].Elementos[1] = default(T);

                                nHijoIzquierdo.Hijos[2] = nodoTemp4;
                                nHijoIzquierdo.Hijos[2].Elementos[0] = nHijoIzquierdo.Hijos[2].Elementos[1];
                                nHijoIzquierdo.Hijos[2].Elementos[1] = default(T);

                                nHijoIzquierdo.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                                nHijoCentral = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                            }

                            // FUNC EXPERIMENTAL
                            try
                            {
                                HijosCorrectos[1] = verificarHijosCorrectos(nHijoIzquierdo);
                                HijosCorrectos[2] = verificarHijosCorrectos(nHijoCentral);

                                // ACA HACER QUE GUARDE LOS HIJOS EN LA LISTA / LIMPIAR LOS HIJOS INSERVIBLES
                                if (HijosCorrectos[0] == false)
                                    GuardarHijos(nHijoIzquierdo);
                                else if (HijosCorrectos[1] == false)
                                    GuardarHijos(nHijoCentral);
                            }
                            catch
                            {
                                HijosCorrectos[0] = true;
                                HijosCorrectos[1] = true;
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoCentral.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[1] = nHijoCentral;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = nHijoIzquierdo;
                                RaizSecundaria.Hijos[0].Hijos[2] = nHijoCentral;

                                int cant = 0;
                                ContarLLaves(RaizSecundaria, ref cant);
                                if (nElementos == cant && determinante == nAuxiliar.PosicionHijo)
                                    terminado = true;
                            }

                            try
                            {
                                if (nHijoIzquierdo.Hijos[0].Hijos[0] != null)
                                {
                                    Nodo2_3<T> temp = new Nodo2_3<T>();
                                    copiarNodo(ref temp, nHijoIzquierdo.Hijos[0].Hijos[0]);

                                    Nodo2_3<T> vacio1 = new Nodo2_3<T>(temp.Elementos[0]);
                                    Nodo2_3<T> vacio2 = new Nodo2_3<T>(temp.Elementos[1]);

                                    nHijoIzquierdo.Hijos[0].Hijos[0] = vacio1;
                                    nHijoIzquierdo.Hijos[0].Hijos[1] = null;
                                    nHijoIzquierdo.Hijos[0].Hijos[2] = vacio2;


                                    Nodo2_3<T> temp2 = new Nodo2_3<T>();
                                    copiarNodo(ref temp2, nHijoIzquierdo.Hijos[2].Hijos[1]);
                                    nHijoIzquierdo.Hijos[2].Hijos[0] = temp2;
                                    nHijoIzquierdo.Hijos[2].Hijos[1] = null;

                                }

                                terminado = true;
                            }
                            catch { }

                            //LIMPIAR LOS HIJOS INSERVIBLES
                            if (HijosCorrectos[0] == false)
                                LimpiarHijos(nHijoIzquierdo);
                            else if (HijosCorrectos[1] == false)
                                LimpiarHijos(nHijoCentral);


                            if (HijosCorrectos[0] == false || HijosCorrectos[1] == false)
                            {
                                foreach (var item in Noditos)
                                    Insertar(item);

                                HijosCorrectos[0] = true;
                                HijosCorrectos[1] = true;
                            }

                        }
                        else if (nAuxiliar.PosicionHijo == "Hijo Central")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = Raiz.Hijos[0];
                                RaizSecundaria.Hijos[2].Hijos[2] = Raiz.Hijos[2];
                            }

                            Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                            Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);

                            try
                            {
                                copiarNodo(ref nHijoIzquierdo, nAuxiliar);
                                nHijoIzquierdo.Elementos[1] = default(T);
                                nHijoIzquierdo.Hijos[2] = nHijoIzquierdo.Hijos[1];
                                nHijoIzquierdo.Hijos[1] = null;

                                copiarNodo(ref nHijoDerecho, nAuxiliar);
                                nHijoDerecho.Elementos[0] = nHijoDerecho.Elementos[1];
                                nHijoDerecho.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[0] = nodoTemp1;
                                nHijoDerecho.Hijos[0].Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[2] = nodoTemp2;
                                nHijoDerecho.Hijos[2].Elementos[0] = nHijoDerecho.Hijos[2].Elementos[1];
                                nHijoDerecho.Hijos[2].Elementos[1] = default(T);
                                nHijoDerecho.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                                nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[0].Hijos[2] = nHijoIzquierdo;
                                RaizSecundaria.Hijos[2].Hijos[0] = nHijoDerecho;

                                terminado = true;
                            }
                        }
                        else if (nAuxiliar.PosicionHijo == "Hijo Derecho")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = Raiz.Hijos[0];
                                RaizSecundaria.Hijos[0].Hijos[2] = Raiz.Hijos[1];
                            }

                            Nodo2_3<T> nHijoIzquierdo;

                            if (nAuxiliar.Padre.Hijos[1] != null)
                                nHijoIzquierdo = nAuxiliar.Padre.Hijos[1];
                            else
                                nHijoIzquierdo = nAuxiliar.Padre.Hijos[0];

                            Nodo2_3<T> nHijoCentral = new Nodo2_3<T>();
                            Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>();

                            try
                            {
                                copiarNodo(ref nHijoCentral, nAuxiliar);
                                nHijoCentral.Elementos[1] = default(T);
                                nHijoCentral.Hijos[2] = nHijoCentral.Hijos[1];
                                nHijoCentral.Hijos[1] = null;

                                copiarNodo(ref nHijoDerecho, nAuxiliar);
                                nHijoDerecho.Elementos[0] = nHijoDerecho.Elementos[1];
                                nHijoDerecho.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[0] = nodoTemp1;
                                nHijoDerecho.Hijos[0].Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[2] = nodoTemp2;
                                nHijoDerecho.Hijos[2].Elementos[0] = nHijoDerecho.Hijos[2].Elementos[1];
                                nHijoDerecho.Hijos[2].Elementos[1] = default(T);
                                nHijoDerecho.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoCentral = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                                nHijoDerecho = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                            }

                            // FUNC EXPERIMENTAL
                            try
                            {
                                HijosCorrectos[1] = verificarHijosCorrectos(nHijoCentral);
                                HijosCorrectos[2] = verificarHijosCorrectos(nHijoDerecho);

                                // ACA HACER QUE GUARDE LOS HIJOS EN LA LISTA / LIMPIAR LOS HIJOS INSERVIBLES
                                if (HijosCorrectos[1] == false)
                                    GuardarHijos(nHijoCentral);
                                else if (HijosCorrectos[2] == false)
                                    GuardarHijos(nHijoDerecho);
                            }
                            catch
                            {
                                HijosCorrectos[1] = true;
                                HijosCorrectos[2] = true;
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoCentral.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[1] = nHijoCentral;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[2].Hijos[0] = nHijoCentral;
                                RaizSecundaria.Hijos[2].Hijos[2] = nHijoDerecho;

                                int cant = 0;
                                ContarLLaves(RaizSecundaria, ref cant);
                                if (nElementos == cant && determinante == nAuxiliar.PosicionHijo)
                                    terminado = true;
                            }

                            try
                            {
                                if (nHijoDerecho.Hijos[0].Hijos[0] != null)
                                {
                                    verificarHijos(ref nHijoDerecho.Hijos[0], "Hijo Izquierdo");
                                    verificarHijos(ref nHijoDerecho.Hijos[2], "Hijo Derecho");
                                }

                                terminado = true;
                            }
                            catch { }

                            //LIMPIAR LOS HIJOS INSERVIBLES
                            if (HijosCorrectos[1] == false)
                                LimpiarHijos(nHijoCentral);
                            else if (HijosCorrectos[2] == false)
                                LimpiarHijos(nHijoDerecho);


                            if (HijosCorrectos[1] == false || HijosCorrectos[2] == false)
                            {
                                foreach (var item in Noditos)
                                    Insertar(item);

                                HijosCorrectos[1] = true;
                                HijosCorrectos[2] = true;
                            }

                        }

                    }
                }
                else
                {
                    // Subir nAuxiliar.Elementos[1]

                    // Si el papá del actual es nulo, tiene que crearse un nuevo nodo que sea papi para el acutual.
                    if (nAuxiliar.Padre == null)
                    {
                        Nodo2_3<T> nPadre = new Nodo2_3<T>(nAuxiliar.Elementos[1]);
                        Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                        Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>(vNuevo);
                        nHijoIzquierdo.Padre = nPadre;
                        nHijoDerecho.Padre = nPadre;

                        nPadre.Hijos[0] = nHijoIzquierdo;
                        nPadre.Hijos[2] = nHijoDerecho;

                        int cont = 0;
                        ContarLLaves(nPadre, ref cont);

                        if (nElementos == cont)
                        {
                            Raiz = nPadre;
                        }
                        else
                        {
                            TrabajarSobreSecundaria = true;
                            RaizSecundaria = nPadre;
                        }
                    }
                    // si si existe papá y tiene espacio el valor se mete en él y se reorganizan los hijos.
                    else
                    {
                        T subir = nAuxiliar.Elementos[1];
                        nAuxiliar.Elementos[1] = vNuevo;

                        InsertarAca(ref nAuxiliar.Padre, subir);

                        if (terminado == true)
                            return default(T);

                        // Si en el nodo que se encuentra es un hijo izquierdo se acomodan la nueva distribucion de los hijos dependiendo de este criterio
                        // Es exactamente lo mismo con las demás opciones.
                        if (nAuxiliar.PosicionHijo == "Hijo Izquierdo")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[2].Hijos[0] = Raiz.Hijos[1];
                                RaizSecundaria.Hijos[2].Hijos[2] = Raiz.Hijos[2];
                            }

                            Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>();
                            Nodo2_3<T> nHijoCentral = new Nodo2_3<T>();
                            Nodo2_3<T> nHijoDerecho;

                            if (nAuxiliar.Padre.Hijos[1] != null)
                                nHijoDerecho = nAuxiliar.Padre.Hijos[1];
                            else
                                nHijoDerecho = nAuxiliar.Padre.Hijos[2];

                            try
                            {
                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nAuxiliar);
                                nHijoCentral = nodoTemp1;
                                nHijoCentral.Elementos[0] = nHijoCentral.Elementos[1];
                                nHijoCentral.Elementos[1] = default(T);
                                nHijoCentral.Hijos[0] = nHijoCentral.Hijos[1];
                                nHijoCentral.Hijos[1] = null;

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nAuxiliar);
                                nHijoIzquierdo = nodoTemp2;
                                nHijoIzquierdo.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp3 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp3, nHijoIzquierdo.Hijos[0]);
                                Nodo2_3<T> nodoTemp4 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp4, nHijoIzquierdo.Hijos[0]);

                                nHijoIzquierdo.Hijos[0] = nodoTemp3;
                                nHijoIzquierdo.Hijos[0].Elementos[1] = default(T);

                                nHijoIzquierdo.Hijos[2] = nodoTemp4;
                                nHijoIzquierdo.Hijos[2].Elementos[0] = nHijoIzquierdo.Hijos[2].Elementos[1];
                                nHijoIzquierdo.Hijos[2].Elementos[1] = default(T);

                                nHijoIzquierdo.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                                nHijoCentral = new Nodo2_3<T>(vNuevo);
                            }

                            // FUNC EXPERIMENTAL
                            try
                            {
                                HijosCorrectos[1] = verificarHijosCorrectos(nHijoIzquierdo);
                                HijosCorrectos[2] = verificarHijosCorrectos(nHijoCentral);

                                // ACA HACER QUE GUARDE LOS HIJOS EN LA LISTA / LIMPIAR LOS HIJOS INSERVIBLES
                                if (HijosCorrectos[0] == false)
                                    GuardarHijos(nHijoIzquierdo);
                                else if (HijosCorrectos[1] == false)
                                    GuardarHijos(nHijoCentral);
                            }
                            catch
                            {
                                HijosCorrectos[0] = true;
                                HijosCorrectos[1] = true;
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoCentral.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[1] = nHijoCentral;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = nHijoIzquierdo;
                                RaizSecundaria.Hijos[0].Hijos[2] = nHijoCentral;

                                int cant = 0;
                                ContarLLaves(RaizSecundaria, ref cant);
                                if (nElementos == cant && determinante == nAuxiliar.PosicionHijo)
                                    terminado = true;
                            }

                            try
                            {
                                if (nHijoIzquierdo.Hijos[0].Hijos[0] != null)
                                {
                                    Nodo2_3<T> temp = new Nodo2_3<T>();
                                    copiarNodo(ref temp, nHijoIzquierdo.Hijos[0].Hijos[0]);

                                    Nodo2_3<T> vacio1 = new Nodo2_3<T>(temp.Elementos[0]);
                                    Nodo2_3<T> vacio2 = new Nodo2_3<T>(temp.Elementos[1]);

                                    nHijoIzquierdo.Hijos[0].Hijos[0] = vacio1;
                                    nHijoIzquierdo.Hijos[0].Hijos[1] = null;
                                    nHijoIzquierdo.Hijos[0].Hijos[2] = vacio2;


                                    Nodo2_3<T> temp2 = new Nodo2_3<T>();
                                    copiarNodo(ref temp2, nHijoIzquierdo.Hijos[2].Hijos[1]);
                                    nHijoIzquierdo.Hijos[2].Hijos[0] = temp2;
                                    nHijoIzquierdo.Hijos[2].Hijos[1] = null;

                                }

                                terminado = true;
                            }
                            catch { }

                            //LIMPIAR LOS HIJOS INSERVIBLES
                            if (HijosCorrectos[0] == false)
                                LimpiarHijos(nHijoIzquierdo);
                            else if (HijosCorrectos[1] == false)
                                LimpiarHijos(nHijoCentral);


                            if (HijosCorrectos[0] == false || HijosCorrectos[1] == false)
                            {
                                foreach (var item in Noditos)
                                    Insertar(item);

                                HijosCorrectos[0] = true;
                                HijosCorrectos[1] = true;
                            }

                        }
                        else if (nAuxiliar.PosicionHijo == "Hijo Central")
                        {

                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = Raiz.Hijos[0];
                                RaizSecundaria.Hijos[2].Hijos[2] = Raiz.Hijos[2];
                            }

                            Nodo2_3<T> nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                            Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>(vNuevo);

                            try
                            {
                                copiarNodo(ref nHijoIzquierdo, nAuxiliar);
                                nHijoIzquierdo.Elementos[1] = default(T);
                                nHijoIzquierdo.Hijos[2] = nHijoIzquierdo.Hijos[1];
                                nHijoIzquierdo.Hijos[1] = null;

                                copiarNodo(ref nHijoDerecho, nAuxiliar);
                                nHijoDerecho.Elementos[0] = nHijoDerecho.Elementos[1];
                                nHijoDerecho.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[0] = nodoTemp1;
                                nHijoDerecho.Hijos[0].Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[2] = nodoTemp2;
                                nHijoDerecho.Hijos[2].Elementos[0] = nHijoDerecho.Hijos[2].Elementos[1];
                                nHijoDerecho.Hijos[2].Elementos[1] = default(T);
                                nHijoDerecho.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoIzquierdo = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                                nHijoDerecho = new Nodo2_3<T>(vNuevo);
                            }

                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                RaizSecundaria.Hijos[0].Hijos[2] = nHijoIzquierdo;
                                RaizSecundaria.Hijos[2].Hijos[0] = nHijoDerecho;
                            }
                            else
                            {
                                RaizSecundaria.Hijos[0].Hijos[2] = nHijoIzquierdo;
                                RaizSecundaria.Hijos[2].Hijos[0] = nHijoDerecho;

                                terminado = true;
                            }
                        }
                        else if (nAuxiliar.PosicionHijo == "Hijo Derecho")
                        {
                            if (TrabajarSobreSecundaria == true)
                            {
                                RaizSecundaria.Hijos[0].Hijos[0] = Raiz.Hijos[0];
                                RaizSecundaria.Hijos[0].Hijos[2] = Raiz.Hijos[1];
                            }

                            // Hermano adyacente
                            Nodo2_3<T> nHijoIzquierdo;
                            HijosCorrectos[0] = true;

                            if (nAuxiliar.Padre.Hijos[1] != null)
                                nHijoIzquierdo = nAuxiliar.Padre.Hijos[1];
                            else
                                nHijoIzquierdo = nAuxiliar.Padre.Hijos[0];

                            Nodo2_3<T> nHijoDerecho = new Nodo2_3<T>();
                            Nodo2_3<T> nHijoCentral = new Nodo2_3<T>();

                            try
                            {
                                copiarNodo(ref nHijoCentral, nAuxiliar);
                                nHijoCentral.Elementos[1] = default(T);
                                nHijoCentral.Hijos[2] = nHijoCentral.Hijos[1];
                                nHijoCentral.Hijos[1] = null;

                                copiarNodo(ref nHijoDerecho, nAuxiliar);
                                nHijoDerecho.Elementos[0] = nHijoDerecho.Elementos[1];
                                nHijoDerecho.Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp1 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp1, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[0] = nodoTemp1;
                                nHijoDerecho.Hijos[0].Elementos[1] = default(T);

                                Nodo2_3<T> nodoTemp2 = new Nodo2_3<T>();
                                copiarNodo(ref nodoTemp2, nHijoDerecho.Hijos[2]);
                                nHijoDerecho.Hijos[2] = nodoTemp2;
                                nHijoDerecho.Hijos[2].Elementos[0] = nHijoDerecho.Hijos[2].Elementos[1];
                                nHijoDerecho.Hijos[2].Elementos[1] = default(T);
                                nHijoDerecho.Hijos[1] = null;
                            }
                            catch
                            {
                                nHijoCentral = new Nodo2_3<T>(nAuxiliar.Elementos[0]);
                                nHijoDerecho = new Nodo2_3<T>(vNuevo);
                            }

                            // FUNC EXPERIMENTAL
                            try
                            {
                                HijosCorrectos[1] = verificarHijosCorrectos(nHijoCentral);
                                HijosCorrectos[2] = verificarHijosCorrectos(nHijoDerecho);

                                // ACA HACER QUE GUARDE LOS HIJOS EN LA LISTA / LIMPIAR LOS HIJOS INSERVIBLES
                                if (HijosCorrectos[1] == false)
                                    GuardarHijos(nHijoCentral);
                                else if (HijosCorrectos[2] == false)
                                    GuardarHijos(nHijoDerecho);
                            }
                            catch
                            {
                                HijosCorrectos[1] = true;
                                HijosCorrectos[2] = true;
                            }


                            nHijoIzquierdo.Padre = nAuxiliar.Padre;
                            nHijoCentral.Padre = nAuxiliar.Padre;
                            nHijoDerecho.Padre = nAuxiliar.Padre;

                            if (TrabajarSobreSecundaria == false)
                            {
                                nAuxiliar.Padre.Hijos[0] = nHijoIzquierdo;
                                nAuxiliar.Padre.Hijos[1] = nHijoCentral;
                                nAuxiliar.Padre.Hijos[2] = nHijoDerecho;
                            }

                            else
                            {
                                RaizSecundaria.Hijos[2].Hijos[0] = nHijoCentral;
                                RaizSecundaria.Hijos[2].Hijos[2] = nHijoDerecho;

                                int cant = 0;
                                ContarLLaves(RaizSecundaria, ref cant);
                                if (nElementos == cant && determinante == nAuxiliar.PosicionHijo)
                                    terminado = true;
                            }


                            try
                            {
                                if (nHijoDerecho.Hijos[0].Hijos[0] != null)
                                {
                                    verificarHijos(ref nHijoDerecho.Hijos[0], "Hijo Izquierdo");
                                    verificarHijos(ref nHijoDerecho.Hijos[2], "Hijo Derecho");
                                }

                                terminado = true;
                            }
                            catch { }

                            //LIMPIAR LOS HIJOS INSERVIBLES
                            if (HijosCorrectos[1] == false)
                                LimpiarHijos(nHijoCentral);
                            else if (HijosCorrectos[2] == false)
                                LimpiarHijos(nHijoDerecho);


                            if (HijosCorrectos[1] == false || HijosCorrectos[2] == false)
                            {
                                foreach (var item in Noditos)
                                    Insertar(item);

                                HijosCorrectos[1] = true;
                                HijosCorrectos[2] = true;
                            }


                        }

                    }
                }
            }

            return default(T);
        }


        public T Buscar(T valor)
        {
            return Buscar(Raiz, valor);
        }

        private T Buscar(Nodo2_3<T> nAuxiliar, T Valor)
        {
            if (nAuxiliar == null)
            {
                return default(T);
            }
            // Si la siguiente condicion se cumple quiere decir que solo hay opción de hijo izquierdo o derecho.
            else if (nAuxiliar.Elementos[1] == null)
            {
                // Se encontró
                if (nAuxiliar.Elementos[0].CompareTo(Valor) == 0)
                    return nAuxiliar.Elementos[0];
                // Como no se encontró me tengo que ir a los hijos
                // Se debe dirigir a la izquierda
                else if (nAuxiliar.Elementos[0].CompareTo(Valor) == 1)
                    return Buscar(nAuxiliar.Hijos[0], Valor);
                // Se debe dirigir a la derecha
                else if (nAuxiliar.Elementos[0].CompareTo(Valor) == -1)
                    return Buscar(nAuxiliar.Hijos[2], Valor);
            }
            // Si la siguiente condicion se cumple quiere decir que hay opción hijo izquierdo, derecho o central.
            else if (nAuxiliar.Elementos[0] != null && nAuxiliar.Elementos[1] != null)
            {
                // Se encontró en la llave izquierda
                if (nAuxiliar.Elementos[0].CompareTo(Valor) == 0)
                    return nAuxiliar.Elementos[0];
                // Se encontró en la llave derecha
                else if (nAuxiliar.Elementos[1].CompareTo(Valor) == 0)
                    return nAuxiliar.Elementos[1];
                // Como no se encontró me tengo que ir a los hijos
                // Se debe dirigir a la izquierda
                else if (nAuxiliar.Elementos[0].CompareTo(Valor) == 1)
                    return Buscar(nAuxiliar.Hijos[0], Valor);
                // Se debe dirigir al centro
                else if (nAuxiliar.Elementos[0].CompareTo(Valor) == -1 && nAuxiliar.Elementos[1].CompareTo(Valor) == 1)
                    return Buscar(nAuxiliar.Hijos[1], Valor);
                // Se debe dirigir a la derecha
                else if (nAuxiliar.Elementos[1].CompareTo(Valor) == -1)
                    return Buscar(nAuxiliar.Hijos[2], Valor);
            }

            return default(T);
        }


        public void Eliminar(ref Nodo2_3<T> nAuxiliar, T vEliminar)
        {
            if (nAuxiliar == null)
                return;

            // Si el valor a eliminar está en una hoja solo lo quito
            else if (nAuxiliar.EsHoja == true)
            {
                if (nAuxiliar.Elementos[0] != null && nAuxiliar.Elementos[1] != null)
                {
                    if (nAuxiliar.Elementos[0].CompareTo(vEliminar) == 0)
                    {
                        nAuxiliar.Elementos[0] = default(T);
                        nAuxiliar.Elementos[0] = nAuxiliar.Elementos[1];
                    }
                    else if (nAuxiliar.Elementos[1].CompareTo(vEliminar) == 0)
                    {
                        nAuxiliar.Elementos[1] = default(T);
                    }
                }
                else if (nAuxiliar.Elementos[1] == null)
                {
                    if (nAuxiliar.PosicionHijo == "Hijo Izquierdo")
                    {
                        if (nAuxiliar.Padre.Hijos[2].Elementos[0] != null && nAuxiliar.Padre.Hijos[2].Elementos[1] != null)
                        {
                            if (nAuxiliar.Padre.Elementos[1] == null)
                            {
                                nAuxiliar.Elementos[0] = nAuxiliar.Padre.Elementos[0];
                                nAuxiliar.Padre.Elementos[0] = nAuxiliar.Padre.Hijos[2].Elementos[0];
                                nAuxiliar.Padre.Hijos[2].Elementos[0] = nAuxiliar.Padre.Hijos[2].Elementos[1];
                            }
                            else
                            {
                                nAuxiliar.Elementos[0] = nAuxiliar.Padre.Elementos[0];
                                nAuxiliar.Padre.Elementos[0] = nAuxiliar.Padre.Elementos[1];
                                nAuxiliar.Padre.Elementos[1] = nAuxiliar.Padre.Hijos[2].Elementos[0];
                                nAuxiliar.Padre.Hijos[2].Elementos[0] = nAuxiliar.Padre.Hijos[2].Elementos[1];
                            }

                        }
                        else if (nAuxiliar.Padre.Hijos[2].Elementos[1] != null)
                        {

                        }


                    }
                }

            }
            // Si la siguiente condicion se cumple quiere decir que solo hay opción de hijo izquierdo o derecho.
            else if (nAuxiliar.Elementos[1] == null)
            {
                // El valor a eliminar es MENOR que la llave padre, se debe dirigir al hijo izquierdo.
                if (nAuxiliar.Elementos[0].CompareTo(vEliminar) == 1)
                    Eliminar(ref nAuxiliar.Hijos[0], vEliminar);
                // El valor a eliminar es MAYOR que la llave padre, se debe dirigir al hijo derecho.
                else if (nAuxiliar.Elementos[0].CompareTo(vEliminar) == -1)
                    Eliminar(ref nAuxiliar.Hijos[2], vEliminar);
                // El valor a eliminar está en esa llave
                else if (nAuxiliar.Elementos[0].CompareTo(vEliminar) == 0)
                {

                }

                // DESDE ACA NO TOQUE NADA
            }
            // Si la siguiente condicion se cumple quiere decir que hay opción hijo izquierdo, derecho o central.
            else if (nAuxiliar.Elementos[0] != null && nAuxiliar.Elementos[1] != null)
            {
                // El valor nuevo es MENOR que la llave padre izquierda, se debe dirigir al hijo izquierdo.
                if (nAuxiliar.Elementos[0].CompareTo(vEliminar) == 1)
                {
                    determinante = "Hijo Izquierdo";
                    T valorTemp = Insertar(vEliminar, ref nAuxiliar.Hijos[0]);

                    if (terminado == true)
                        return;

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);
                }
                // El valor nuevo es MAYOR que la llave padre izquierda y MENOR que la llave padre derecha, se debe dirigir al hijo central.
                if (nAuxiliar.Elementos[0].CompareTo(vEliminar) == -1 && nAuxiliar.Elementos[1].CompareTo(vEliminar) == 1)
                {
                    determinante = "Hijo Central";
                    T valorTemp = Insertar(vEliminar, ref nAuxiliar.Hijos[1]);

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);
                }
                // El valor nuevo es MAYOR que la llave padre derecha, se debe dirigir al hijo derecho.
                else if (nAuxiliar.Elementos[1].CompareTo(vEliminar) == -1)
                {
                    determinante = "Hijo Derecho";
                    T valorTemp = Insertar(vEliminar, ref nAuxiliar.Hijos[2]);

                    // INSERTAR ACÁ
                    if (valorTemp != null)
                        InsertarAca(ref nAuxiliar, valorTemp);
                }
            }
        }







        public List<T> ObtenerArbol()
        {
            List<T> Elemetnos = new List<T>();
            InOrder(Raiz, ref Elemetnos);

            return Elemetnos;
        }

        private void InOrder(Nodo2_3<T> Aux, ref List<T> Elements)
        {
            if (Aux != null)
            {
                if (Aux.Hijos[0] != null)
                    InOrder(Aux.Hijos[0], ref Elements);

                Elements.Add(Aux.Elementos[0]);

                if (Aux.Hijos[1] != null)
                    InOrder(Aux.Hijos[1], ref Elements);

                if (Aux.Elementos[1] != null)
                    Elements.Add(Aux.Elementos[1]);

                if (Aux.Hijos[2] != null)
                    InOrder(Aux.Hijos[2], ref Elements);
            }
        }

        private void ContarLLaves(Nodo2_3<T> Aux, ref int Elements)
        {
            if (Aux != null)
            {
                ContarLLaves(Aux.Hijos[0], ref Elements);
                if (Aux.Elementos[0] != null)
                    Elements++;
                ContarLLaves(Aux.Hijos[1], ref Elements);
                if (Aux.Elementos[1] != null)
                    Elements++;
                ContarLLaves(Aux.Hijos[2], ref Elements);
            }
        }

        /// <summary>
        /// Le asigna a todo el mundo su papi
        /// </summary>
        /// <param name="Aux"></param>
        private void CorregirPadres(ref Nodo2_3<T> Aux)
        {
            if (Aux.EsHoja == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Aux.Hijos[i] != null)
                    {
                        Aux.Hijos[i].Padre = Aux;
                        CorregirPadres(ref Aux.Hijos[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Retorna el nivel del arbol
        /// </summary>
        private int nivelArbol
        {
            get
            {
                int cont = 0;
                Nodo2_3<T> Aux = Raiz;

                while (Aux != null)
                {
                    cont++;
                    Aux = Aux.Hijos[0];
                }

                return cont;
            }
        }

        /// <summary>
        /// Copia el contenido de un nodo para evitar modificar el original por su naturaleza como apuntadores.
        /// </summary>
        /// <param name="copia"></param>
        /// <param name="original"></param>
        private void copiarNodo(ref Nodo2_3<T> copia, Nodo2_3<T> original)
        {
            copia.Elementos[0] = original.Elementos[0];
            copia.Elementos[1] = original.Elementos[1];
            copia.Hijos[0] = original.Hijos[0];
            copia.Hijos[1] = original.Hijos[1];
            copia.Hijos[2] = original.Hijos[2];
            copia.Padre = original.Padre;
        }

        /// <summary>
        /// Retorna si todos los hijos son correctos o no
        /// </summary>
        /// <param name="aux"></param>
        /// <returns></returns>
        private bool verificarHijosCorrectos(Nodo2_3<T> aux)
        {
            if (aux.Hijos[0].Elementos[1] != null)
                if (aux.Hijos[0].Elementos[1].CompareTo(aux.Elementos[0]) != -1)
                    return false;

            if (aux.Hijos[2].Elementos[1] != null)
                if (aux.Hijos[2].Elementos[1].CompareTo(aux.Elementos[0]) != 1)
                    return false;

            if (aux.Hijos[0].Elementos[0].CompareTo(aux.Elementos[0]) != -1)
                return false;

            if (aux.Hijos[2].Elementos[0].CompareTo(aux.Elementos[0]) != 1)
                return false;

            if (aux.Elementos[1] != null)
            {
                if (aux.Hijos[2].Elementos[1].CompareTo(aux.Elementos[0]) != 1)
                    return false;

                if (aux.Hijos[1].Elementos[1] != null)
                    if (aux.Hijos[1].Elementos[1].CompareTo(aux.Elementos[0]) != -1 || aux.Hijos[1].Elementos[1].CompareTo(aux.Elementos[1]) != 1)
                        return false;
            }

            return true;
        }

        private void verificarHijos(ref Nodo2_3<T> aux, string posicion)
        {
            if (posicion == "Hijo Izquierdo")
            {
                if (aux.Elementos[1] == null)
                {
                    Nodo2_3<T> temp = new Nodo2_3<T>();
                    copiarNodo(ref temp, aux.Hijos[1]);
                    aux.Hijos[2] = temp;
                    aux.Hijos[1] = null;
                }

            }
            else if (posicion == "Hijo Derecho")
            {
                Nodo2_3<T> temp = new Nodo2_3<T>();
                copiarNodo(ref temp, aux.Hijos[2]);

                Nodo2_3<T> vacio1 = new Nodo2_3<T>(temp.Elementos[0]);
                Nodo2_3<T> vacio2 = new Nodo2_3<T>(temp.Elementos[1]);

                aux.Hijos[0] = vacio1;
                aux.Hijos[1] = null;
                aux.Hijos[2] = vacio2;

            }

        }

        private void GuardarHijos(Nodo2_3<T> Padre)
        {
            // Hijo izquierdo
            if (Padre.Hijos[0] != null)
            {
                if (Padre.Hijos[0].Elementos[0] != null)
                    Noditos.Add(Padre.Hijos[0].Elementos[0]);
                if (Padre.Hijos[0].Elementos[1] != null)
                    Noditos.Add(Padre.Hijos[0].Elementos[1]);
            }

            // Hijo central
            if (Padre.Hijos[1] != null)
            {
                if (Padre.Hijos[1].Elementos[0] != null)
                    Noditos.Add(Padre.Hijos[1].Elementos[0]);
                if (Padre.Hijos[1].Elementos[1] != null)
                    Noditos.Add(Padre.Hijos[1].Elementos[1]);
            }

            // Hijo Derecho
            if (Padre.Hijos[2] != null)
            {
                if (Padre.Hijos[2].Elementos[0] != null)
                    Noditos.Add(Padre.Hijos[2].Elementos[0]);
                if (Padre.Hijos[2].Elementos[1] != null)
                    Noditos.Add(Padre.Hijos[2].Elementos[1]);
            }

        }

        private void LimpiarHijos(Nodo2_3<T> Padre)
        {
            // Hijo izquierdo
            if (Padre.Hijos[0] != null)
            {
                if (Padre.Hijos[0].Elementos[0] != null)
                    Padre.Hijos[0].Elementos[0] = default(T);
                if (Padre.Hijos[0].Elementos[1] != null)
                    Padre.Hijos[0].Elementos[1] = default(T);
            }

            // Hijo central
            if (Padre.Hijos[1] != null)
            {
                if (Padre.Hijos[1].Elementos[0] != null)
                    Padre.Hijos[1].Elementos[0] = default(T);
                if (Padre.Hijos[1].Elementos[1] != null)
                    Padre.Hijos[1].Elementos[1] = default(T);
            }

            // Hijo Derecho
            if (Padre.Hijos[0] != null)
            {
                if (Padre.Hijos[2].Elementos[0] != null)
                    Padre.Hijos[2].Elementos[0] = default(T);
                if (Padre.Hijos[2].Elementos[1] != null)
                    Padre.Hijos[2].Elementos[1] = default(T);
            }

        }
    }
}
