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


}//Piezas