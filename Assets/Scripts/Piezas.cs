using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Piezas 
{
    // Start is called before the first frame update
    public void Start()
    {
        //Cuando se inicie el juego de rigor por aquí empezamos!
        // Creo el nodo
    
        int[,] piezas= {{1,2,3}, 
                        {4,5,8},
                        {6,0,7}};//Creo el array

        Nodo root=new Nodo(piezas);// Creo el objeto con el array

        Console.WriteLine("¡Anchura! ");
        List<Nodo> solucion = BusquedaAnchura(root);
      /*  Console.WriteLine("¡Estrella! ");
        List<Nodo> solucion2 = BusquedaAEstrella(root);
        Console.WriteLine("¡Estrella sin costo! ");
        List<Nodo> solucion3 = BusquedaAEstrellaSinCosto(root);
        
        Console.WriteLine("¡Estrella Manhattan! ");
        List<Nodo> solucion4 = BusquedaAEstrellaManhattan(root);
        */

        ////////////////// 
        /// Creamos una lista de 1000 nodos iniciales aletorios
        /// Los meterá en un archivo
        /// ///////////////
   /*     for (int i = 0; i < 1000; i++)
        {
            Console.WriteLine("Pasamos por aquí");
            int[,] inicial = GenerarMatrizAleatoria();
            Nodo nodo = new Nodo(inicial);
            Console.WriteLine("metemos ");  nodo.Imprime();
            GuardarNodoEnArchivo(nodo, "nodo_inicial.txt");
            Nodo solucion5 = BusquedaAEstrellaManhattanModificado(nodo);
            Console.WriteLine("final ");  solucion5.Imprime();
            GuardarNodoEnArchivo(solucion5, "nodo_final.txt");

        }

*/
      /*  if (solucion4.Count > 0)
            {
                Console.WriteLine("Imprimiendo la solución: ");
                solucion4.Reverse();
                for (int i = 0; i < solucion4.Count; i++)
                {
                   // solucion[i].Imprime();
                }
            }
            else
            {
                 Console.WriteLine("No hemos encontrado la solucion");
            }*/

    }//Start

 /*   private void GuardarNodoEnArchivo(Nodo nodo, string archivo)
    {
        using (StreamWriter writer = new StreamWriter(archivo,true))
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    writer.Write(nodo.nodo[i, j] + " ");
                }
                writer.WriteLine();
            }
            writer.WriteLine();
        }
    }
*/
    /////////////
    /// Búsqueda en anchura
    /// ////////
    public List<Nodo> BusquedaAnchura(Nodo root)
    {
        //Variables para el algoritmo
        List<Nodo> Abiertos=new List<Nodo>();//Nodos que faltan por visitar
        List<Nodo> Cerrados=new List<Nodo>();// Nodos que ya he visitado
        List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
        bool encontrado=false;
        Abiertos.Add(root);//añado el raíz
        int contador = 0;

        while(Abiertos.Count > 0 && !encontrado)
        {
            Nodo actual = Abiertos[0]; //cogemos el primer elemento
            Abiertos.RemoveAt(0);//eliminamos el elemento
            Cerrados.Add(actual);//ya visitamos este nodo
            //Tratamos el nodo actual. 
            if (actual.EsMeta())
            {
               // Console.WriteLine("Hemos encontrado el nodo solución!!!!");
                encontrado = true;
                //tenemos que devolver el camino a la solución, es decir, la lista de los nodos
                // por los que tenemos que pasar
                Trazo(CaminoSoluccion, actual);
                //Console.WriteLine("Número de pasos de la solución: "+ CaminoSoluccion.Count());
                return CaminoSoluccion;
                //break;//Salimos del todo
            }//if

            //Expandimos el nodo actual
            actual.Expandir();
            for(int i =0; i<actual.hijos.Count; i++)// Recorremos todos los hijos
            {
                Nodo hijoActual=actual.hijos[i];
                if(!Contiene(Abiertos,hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                   // hijoActual.Imprime();
                    Abiertos.Add(hijoActual);// Metemos como una posible solución
                    contador++;
                  //  Console.WriteLine("Número de nodos metidos: "+contador);

                }//if

            }//for
            //if (contador == 50000) break;
        }//while

        //No hemos encontrado solución
        Console.WriteLine("No hemos encontrado solución, Alma de cántaro.");
        return null;
            
        
       

    }//BusquedaAnchura

    private bool Contiene(List<Nodo> lista,Nodo hijoActual)
    {
        /*   for (int i = 0; i < lista.Count; i++)
           {
               if (lista[i].EsMismoNodo(hijoActual.nodo))
                   return true;
           }

           return false;
        */
        foreach (Nodo nodo in lista)
        {
            if(nodo.EsMismoNodo(hijoActual.nodo)) { return true; }
        }
        return false;
    }//Contiene



    //Metodo que hace la solucion
    public void Trazo(List<Nodo> camino, Nodo n)
    {
        Console.WriteLine("Trazando el camino: ");
        Nodo actual = n;
        camino.Add(actual);
        while (actual.padre != null)
        {
            actual = actual.padre;
            camino.Add(actual);
        }

    }


/////////////////////////////
///   /////////////
    /// Búsqueda A* con costo y heurística mal colocadas
    /////////////
    private List<Nodo> BusquedaAEstrella(Nodo root)
    {
        // Variables para el algoritmo
        List<Nodo> Abiertos = new List<Nodo>(); // Nodos que faltan por visitar
        List<Nodo> Cerrados = new List<Nodo>(); // Nodos que ya he visitado
        List<Nodo> CaminoSolucion = new List<Nodo>(); // Lista con el camino
        Abiertos.Add(root); // añado el raíz

        while (Abiertos.Count > 0)
        {
            // Ordenar la lista de abiertos por la suma de costo y heurística
            Abiertos.Sort((n1, n2) => (n1.Costo + n1.Heuristica).CompareTo(n2.Costo + n2.Heuristica));
            Nodo actual = Abiertos[0]; // cogemos el primer elemento
            Abiertos.RemoveAt(0); // eliminamos el elemento
            Cerrados.Add(actual); // ya visitamos este nodo

            // Tratamos el nodo actual
            if (actual.EsMeta())
            {
                Console.WriteLine("Hemos encontrado el nodo solución!!!!");
                
                Trazo(CaminoSolucion, actual);
                Console.WriteLine("Número de pasos de la solución: "+ CaminoSolucion.Count());
                return CaminoSolucion;
            }

            // Expandimos el nodo actual
            actual.Expandir();
            foreach (var hijoActual in actual.hijos)
            {
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    hijoActual.Costo = actual.Costo + 1; // Actualizamos el costo
                    hijoActual.CalcularHeuristicaPiezasMalColocadas(); // Calculamos la heurística
                    Abiertos.Add(hijoActual); // Metemos como una posible solución
                }
            }
        }

        // No hemos encontrado solución
        Console.WriteLine("No hemos encontrado solución, Alma de cántaro.");
        return null;
    }




    /////////////
    /// Búsqueda A* sin costo
    /////////////
    private List<Nodo> BusquedaAEstrellaSinCosto(Nodo root)
    {
        // Variables para el algoritmo
        List<Nodo> Abiertos = new List<Nodo>(); // Nodos que faltan por visitar
        List<Nodo> Cerrados = new List<Nodo>(); // Nodos que ya he visitado
        List<Nodo> CaminoSolucion = new List<Nodo>(); // Lista con el camino
        Abiertos.Add(root); // añado el raíz

        while (Abiertos.Count > 0)
        {
            // Ordenar la lista de abiertos solo por la heurística
            //Abiertos.Sort((n1, n2) => n1.Heuristica.CompareTo(n2.Heuristica));
              OrdenarPorHeuristica(Abiertos);
            Nodo actual = Abiertos[0]; // cogemos el primer elemento
            Abiertos.RemoveAt(0); // eliminamos el elemento
            Cerrados.Add(actual); // ya visitamos este nodo

            // Tratamos el nodo actual
            if (actual.EsMeta())
            {
                Console.WriteLine("Hemos encontrado el nodo solución!!!!");
                
                Trazo(CaminoSolucion, actual);
                Console.WriteLine("Número de pasos de la solución: "+ CaminoSolucion.Count());
                return CaminoSolucion;
            }

            // Expandimos el nodo actual
            actual.Expandir();
            foreach (var hijoActual in actual.hijos)
            {
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    hijoActual.CalcularHeuristicaPiezasMalColocadas(); // Calculamos la heurística
                    Abiertos.Add(hijoActual); // Metemos como una posible solución
                }
            }
        }

        // No hemos encontrado solución
        Console.WriteLine("No hemos encontrado solución, Alma de cántaro.");
        return null;
    }




/////////////////////////////
///   /////////////
    /// Búsqueda A* con costo y heurística
    /////////////
    private List<Nodo> BusquedaAEstrellaManhattan(Nodo root)
    {
        // Variables para el algoritmo
        List<Nodo> Abiertos = new List<Nodo>(); // Nodos que faltan por visitar
        List<Nodo> Cerrados = new List<Nodo>(); // Nodos que ya he visitado
        List<Nodo> CaminoSolucion = new List<Nodo>(); // Lista con el camino
        Abiertos.Add(root); // añado el raíz

        while (Abiertos.Count > 0)
        {
            // Ordenar la lista de abiertos por la suma de costo y heurística
            Abiertos.Sort((n1, n2) => (n1.Costo + n1.manhattan).CompareTo(n2.Costo + n2.manhattan));
            Nodo actual = Abiertos[0]; // cogemos el primer elemento
            Abiertos.RemoveAt(0); // eliminamos el elemento
            Cerrados.Add(actual); // ya visitamos este nodo

            // Tratamos el nodo actual
            if (actual.EsMeta())
            {
                Console.WriteLine("Hemos encontrado el nodo solución!!!!");
                
                Trazo(CaminoSolucion, actual);
                Console.WriteLine("Número de pasos de la solución: "+ CaminoSolucion.Count());
                return CaminoSolucion;
            }

            // Expandimos el nodo actual
            actual.Expandir();
            foreach (var hijoActual in actual.hijos)
            {
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    hijoActual.Costo = actual.Costo + 1; // Actualizamos el costo
                    hijoActual.CalcularHeuristicaManhattan(); // Calculamos la heurística
                    Abiertos.Add(hijoActual); // Metemos como una posible solución
                }
            }
        }

        // No hemos encontrado solución
        Console.WriteLine("No hemos encontrado solución, Alma de cántaro.");
        return null;
    }
///////////
///
/////////////
    private void OrdenarPorHeuristica(List<Nodo> lista)
    {
       for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = i + 1; j < lista.Count; j++)
            {
                if (lista[i].Heuristica > lista[j].Heuristica)
                {
                    Nodo temp = lista[i];
                    lista[i] = lista[j];
                    lista[j] = temp;
                }
            }
        }
    }


    private int[,] GenerarMatrizAleatoria()
    {
        int[,] matriz = new int[3, 3];
        List<int> numeros = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        Random rand = new Random();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int index = rand.Next(numeros.Count);
                matriz[i, j] = numeros[index];
                numeros.RemoveAt(index);
            }
        }

        return matriz;
    }


    /////////////////////////////
///   /////////////
    /// Búsqueda A* con costo y heurística
    /////////////
    private Nodo BusquedaAEstrellaManhattanModificado(Nodo root)
    {
        // Variables para el algoritmo
        List<Nodo> Abiertos = new List<Nodo>(); // Nodos que faltan por visitar
        List<Nodo> Cerrados = new List<Nodo>(); // Nodos que ya he visitado
        List<Nodo> CaminoSolucion = new List<Nodo>(); // Lista con el camino
        Abiertos.Add(root); // añado el raíz
        int contador=0;
        while (Abiertos.Count > 0)
        {   contador++;
            // Ordenar la lista de abiertos por la suma de costo y heurística
            Abiertos.Sort((n1, n2) => (n1.Costo + n1.manhattan).CompareTo(n2.Costo + n2.manhattan));
            Nodo actual = Abiertos[0]; // cogemos el primer elemento
            if (contador==2) return actual;
            Abiertos.RemoveAt(0); // eliminamos el elemento
            Cerrados.Add(actual); // ya visitamos este nodo

            // Tratamos el nodo actual
            if (actual.EsMeta())
            {
                Console.WriteLine("Hemos encontrado el nodo solución!!!!");
                
                Trazo(CaminoSolucion, actual);
                Console.WriteLine("Número de pasos de la solución: "+ CaminoSolucion.Count());
                return null;
            }

            // Expandimos el nodo actual
            actual.Expandir();
            foreach (var hijoActual in actual.hijos)
            {
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    hijoActual.Costo = actual.Costo + 1; // Actualizamos el costo
                    hijoActual.CalcularHeuristicaManhattan(); // Calculamos la heurística
                    Abiertos.Add(hijoActual); // Metemos como una posible solución
                }
            }
        }

        // No hemos encontrado solución
        Console.WriteLine("No hemos encontrado solución, Alma de cántaro.");
        return null;
    }

}//Piezas