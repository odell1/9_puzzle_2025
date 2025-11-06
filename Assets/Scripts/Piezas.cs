using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piezas 
{
    // Start is called before the first frame update
    public void Start()
    {
        //Cuando se inicie el juego de rigor por aquí empezamos!
        // Creo el nodo
    
        int[,] piezas= {{1,2,3}, 
                        {5,4,8},
                        {6,0,7}};//Creo el array

        Nodo root=new Nodo(piezas);// Creo el objeto con el array

//Debug.Log("¡Anchura! ");
        List<Nodo> solucion = BusquedaAnchura(root);
    
        if (solucion.Count > 0)
            {
              //  Debug.Log("Imprimiendo la solución: ");
                solucion.Reverse();
                for (int i = 0; i < solucion.Count; i++)
                {
                   // solucion[i].Imprime();
                }
            }
            else
            {
                 //Debug.Log("No hemos encontrado la solucion");
            }

    }//Start

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
        int contador = 0; // Por si acaso nos quedásemos enganchados

        while(Abiertos.Count > 0 && !encontrado)
        {
            Nodo actual = Abiertos[0]; //cogemos el primer elemento
            Abiertos.RemoveAt(0);//eliminamos el elemento
            Cerrados.Add(actual);//ya visitamos este nodo
            //Tratamos el nodo actual. 
            if (actual.EsMeta())
            {
               
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


    /// <summary>
    /// Búsqueda en Profundidad
    /// </summary>
    /// <param name="lista"></param>
    /// <param name="hijoActual"></param>
    /// <returns></returns>
    /// 
    public List<Nodo> BusquedaProfundidad(Nodo root)
    {

        List<Nodo> Abiertos = new List<Nodo>();//La lógica es de una Pila
        //Stack<Nodo> Abiertos=new Stack<Nodo>();
        List<Nodo> Cerrados = new List<Nodo>();// Nodos que ya he visitado
        List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
        bool encontrado = false;
        Abiertos.Add(root);// añado el raíz (Push)
        //Abiertos.Push(root);
        int contador = 0;

        while (Abiertos.Count > 0 && !encontrado)
        {
            int ultimoIndice = Abiertos.Count - 1; //Para coger el último elemento (lógica LIFO)
            Nodo actual=Abiertos[ultimoIndice]; // Cogemos el último elmento
            //Nodo actual=Abiertos.pop();
            Abiertos.RemoveAt(ultimoIndice);
            Cerrados.Add(actual);
            if (actual.EsMeta())
            {
                encontrado = true;
                Trazo(CaminoSoluccion, actual);
                return CaminoSoluccion;
            }//if
            actual.Expandir();
            for (int i = 0; i < actual.hijos.Count; i++)// Recorremos todos los hijos
            {
                Nodo hijoActual = actual.hijos[i];
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    Abiertos.Add(hijoActual);// metemos como una posible solución (Push)
                    contador++;
                }//if

            }//for

        }//while

        return null;

    }//BusquedaProfundidad



    /// <summary>
    /// Búsqueda en Profundidad
    /// </summary>
    /// <param name="lista"></param>
    /// <param name="hijoActual"></param>
    /// <returns></returns>
    /// 
    public List<Nodo> BusquedaProfundidadAcotada(Nodo root, int limite)
    {

        List<Nodo> Abiertos = new List<Nodo>();//La lógica es de una Pila
        List<Nodo> Cerrados = new List<Nodo>();// Nodos que ya he visitado
        List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
        bool encontrado = false;
        Abiertos.Add(root);// añado el raíz (Push)
        root.Costo = 0; //Incializo el valor de profundidad

        while (Abiertos.Count > 0 && !encontrado)
        {
            int ultimoIndice = Abiertos.Count - 1; //Para coger el último elemento (lógica LIFO)
            Nodo actual = Abiertos[ultimoIndice]; // Cogemos el último elmento
            Abiertos.RemoveAt(ultimoIndice);
            Cerrados.Add(actual);
            if (actual.EsMeta())
            {
                encontrado = true;
                Trazo(CaminoSoluccion, actual);
                return CaminoSoluccion;
            }//if
            if (actual.Costo < limite)
            {
                actual.Expandir();
                for (int i = 0; i < actual.hijos.Count; i++)// Recorremos todos los hijos
                {
                    Nodo hijoActual = actual.hijos[i];
                    hijoActual.Costo = actual.Costo + 1;//Aquí se actualiza el valor de nivel del árbol
                    if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                    {

                        Abiertos.Add(hijoActual);// metemos como una posible solución (Push)
 
                    }//if

                }//1 if
            }//for

        }//while


        return null;


    }//BusquedaProfundidad


    /// <summary>
    /// Búsqueda A* 
    /// </summary>
    /// <param name="lista"></param>
    /// <param name="hijoActual"></param>
    /// <returns></returns>
    /// 
    public List<Nodo> BusacaAsterisco(Nodo root)
    {

        List<Nodo> Abiertos = new List<Nodo>();
        List<Nodo> Cerrados = new List<Nodo>();// Nodos que ya he visitado
        List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
        bool encontrado = false;
        Abiertos.Add(root);// añado el raíz (Push)
        root.Costo = 0; //Incializo el valor de profundidad
        root.calculaMalColocadas();

        while (Abiertos.Count > 0 && !encontrado)
        {
            //Reordeno abiertos para que el costo+heurística sea de menor a mayor
            Abiertos=Abiertos.OrderBy(n=>n.Costo+n.Heuristica).ToList();//Ordenamos por coste+heurística
            // Cogemos el primer nodo de abiertos
            Nodo actual = Abiertos[0];
            Abiertos.RemoveAt(0);

            if (actual.EsMeta())
            {
                encontrado = true;
                UnityEngine.Debug.Log("Hemos encontrado la solución");
                Trazo(CaminoSoluccion, actual);
                return CaminoSoluccion;
            }
            Cerrados.Add(actual);
            actual.Expandir();
            for (int i = 0; i < actual.hijos.Count; i++)// Recorremos todos los hijos
            {
                Nodo hijoActual = actual.hijos[i];
                hijoActual.Costo = actual.Costo + 1;//Aquí se actualiza el valor de nivel del árbol
                hijoActual.calculaMalColocadas(); //Calculo la heurística
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {

                    Abiertos.Add(hijoActual);// metemos como una posible solución (Push)

                }//if


            }//for

        }//while


        return null;


    }//BusacaAsterisco






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


}//Piezas