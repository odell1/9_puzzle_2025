using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PuzzleVisualizer : MonoBehaviour
{
    public GameObject piezaPrefab;
    public Sprite[] sprites;

    private GameObject[,] piezasVisuales = new GameObject[3, 3];
    private List<Nodo> solucion;
    private int pasoActual = 0;

    public TextMeshProUGUI pasoTexto;

    void Start()
    {
        pasoTexto.text = "Desordenado";
        pasoTexto.SetText("Desordenado");
        CrearVisualizacion();
                

        // Crear instancia de Piezas y ejecutar su lógica
        Piezas piezas = new Piezas();
        int[,] estadoInicial = {
            {1, 2, 3},
            {4, 5, 8},
            {6, 0, 7}
        };
        Nodo root = new Nodo(estadoInicial);
        
        ActualizarVisualizacion(root);
        pasoTexto.text = "Buscando solución...";
        solucion = piezas.BusquedaAnchura(root);
        pasoTexto.text = "Solución encontrada!";




        if (solucion != null && solucion.Count > 0)
        {
            solucion.Reverse(); // Mostrar desde el inicio
            StartCoroutine(MostrarSolucionPasoAPaso());
        }
    }

    void CrearVisualizacion()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject pieza = Instantiate(piezaPrefab, transform);
                piezasVisuales[i, j] = pieza;
            }
        }
    }

    void ActualizarVisualizacion(Nodo nodo)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int valor = nodo.nodo[i, j];
                piezasVisuales[i, j].GetComponent<Image>().sprite = sprites[valor];
            }
        }
    }

    IEnumerator MostrarSolucionPasoAPaso()
    {
        while (pasoActual < solucion.Count)
        {
            ActualizarVisualizacion(solucion[pasoActual]);
            pasoActual++;
            yield return new WaitForSeconds(0.2f); // Espera 2 segundos entre pasos
        }
    }
}