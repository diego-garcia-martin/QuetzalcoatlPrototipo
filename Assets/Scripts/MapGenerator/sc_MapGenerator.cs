using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_MapGenerator : MonoBehaviour
{
    // Lista con objetos para cada linea del mapa
    private List<sc_LineConstructor> _mapMatrix;
    //Tipo de linea actual del mapa, pueden ser low, mid, high, en ese orden y se alternan
    private enum _lineType{
        low,
        mid,
        high,
    };
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
