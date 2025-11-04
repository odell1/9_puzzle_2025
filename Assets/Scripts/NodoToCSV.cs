using System;
using System.Collections.Generic;
using System.IO;

public class NodoToCSV
{
    public NodoToCSV()
    {
      //  string inputFileName = "nodo_final.txt";
      //  string outputFileName = "nodo_final.csv";
                string inputFileName = "nodo_inicial.txt";
        string outputFileName = "nodo_inicial.csv";


        try
        {
            List<int[,]> nodos = LeerNodosDesdeArchivo(inputFileName);
            EscribirNodosACSV(nodos, outputFileName);
            Console.WriteLine("¡Los nodos se han guardado en nodos.csv!");
        }
        catch (IOException e)
        {
            Console.WriteLine("Ocurrió un error: " + e.Message);
        }
    }

    private static List<int[,]> LeerNodosDesdeArchivo(string fileName)
    {
        List<int[,]> nodos = new List<int[,]>();
        using (StreamReader reader = new StreamReader(fileName))
        {
            string line;
            int[,] nodo = new int[3, 3];
            int row = 0;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    // Fin de un nodo, añadir a la lista y resetear
                    nodos.Add(nodo);
                    nodo = new int[3, 3];
                    row = 0;
                }
                else
                {
                    string[] values = line.Trim().Split(' ');
                    for (int col = 0; col < values.Length; col++)
                    {
                        nodo[row, col] = int.Parse(values[col]);
                    }
                    row++;
                }
            }
            // Añadir el último nodo si no se añadió
            if (row > 0)
            {
                nodos.Add(nodo);
            }
        }
        return nodos;
    }

    private static void EscribirNodosACSV(List<int[,]> nodos, string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (int[,] nodo in nodos)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        writer.Write(nodo[i, j]);
                        if (j < 2)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                }
                writer.WriteLine(); // Separar nodos con una línea en blanco
            }
        }
    }
}