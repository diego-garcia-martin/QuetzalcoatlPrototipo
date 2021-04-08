using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_MapGenerator : MonoBehaviour
{

    //******************************************************** LINE LOGIC ************************************************************
    // En esta parte del codigo se describe la linea de objetos, con sus metodos y variables para usar despues en la matriz
    struct _line{
        public List<GameObject> _lineObjects;
        public int lineType;

        public _line(int type){
            lineType = type;
            _lineObjects = new List<GameObject>();
        }

        public void AddObject(GameObject obj, Vector2 pos)
        {
            _lineObjects.Add(Instantiate(obj, new Vector3(pos.x, pos.y, 0), Quaternion.identity));
        }

        public void RemoveObjectAt(int index)
        {
            if (index < _lineObjects.Count)
            {
                if (_lineObjects[index])
                {
                    GameObject.Destroy(_lineObjects[index]);
                    _lineObjects.RemoveAt(index);
                }
            }
        }

        public void RemoveEmpty()
        {
        for (int index = 0; index < _lineObjects.Count; index++)
            {
                if (_lineObjects[index] == null)
                {
                    _lineObjects.RemoveAt(index);
                }
            } 
        }

        public void EmptyLine()
        {
            RemoveEmpty();
            for (int index = 0; index < _lineObjects.Count; index++)
            {
                if (_lineObjects[index])
                {
                    GameObject.Destroy(_lineObjects[index]);
                    _lineObjects.RemoveAt(index);
                }
            }
        }

        public void MoveLineDown(float movY)
        {
            RemoveEmpty();
            for (int index = 0; index < _lineObjects.Count; index++)
            {
                if (_lineObjects[index])
                {
                    _lineObjects[index].transform.position = new Vector3(_lineObjects[index].transform.position.x, _lineObjects[index].transform.position.y - movY, 0);
                }
            } 
        }
    };


    // ************************************************************ MATRIX LOGIC **************************************************************
    // Lista con objetos para cada linea del mapa
    private List<_line> _mapMatrix;
    //Tipo de linea actual del mapa, pueden ser low, mid, high, en ese orden y se alternan
    public enum _lineType{
        low,
        mid,
        high,
        end,
    };
    //Aqui van los objetos que son los tipos
    public GameObject groundBasic;
    public GameObject groundHard;
    public GameObject groundSoft;
    public GameObject groundHazard;
    public GameObject middleHazard;
    public GameObject middleDecor1;
    public GameObject middleDecor2;
    // Variables de constantes a tomar en cuenta para el generador
    private const int LINES_PER_SCREEN = 12;
    private const int TILES_PER_LINE = 20;
    private const int LINE_START = -10;
    private const int BOTTOM_LIMIT = -10;



    void initFirstScreen()
    {
        for(int i = 0 ; i < LINES_PER_SCREEN; i++)
        {
            _mapMatrix.Add(new _line(i % (int)_lineType.end));
        }

        for(int i = 0 ; i < _mapMatrix.Count; i++)
        {
            generateLine(_mapMatrix[i], _mapMatrix[i].lineType, i);
        }
    }

    void generateLine(_line line, int type, int lineNumber)
    {
        switch(type){
            case 0:
                for(int i = 0; i < TILES_PER_LINE; i++)
                {
                    if (Random.Range(0, 10) > 3)
                    {
                        int tileType = Random.Range(0, 2);
                        switch(tileType){
                            case 0:
                                line.AddObject(groundBasic, new Vector2(LINE_START + i, lineNumber - BOTTOM_LIMIT));
                                break;
                            case 1:
                                line.AddObject(groundHard, new Vector2(LINE_START + i, lineNumber - BOTTOM_LIMIT));
                                break;
                            case 2:
                                line.AddObject(groundSoft, new Vector2(LINE_START + i, lineNumber - BOTTOM_LIMIT));
                                break;
                            default:
                                break;
                        }
                    }
                }
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _mapMatrix = new List<_line>();
        initFirstScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
