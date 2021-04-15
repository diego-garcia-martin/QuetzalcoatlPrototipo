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

    //Las variables que alteran las posibilidades de la poblacion de tiles
    private int GroundPopulation = 30;
    public int GroundSize = 40;
    //Aqui van los objetos que son los tipos de pisos, decoraciones, etc.
    public GameObject groundBasic;
    public GameObject groundHard;
    public GameObject groundSoft;
    public GameObject groundHazard;
    public GameObject middleHazard;
    public GameObject middleDecor1;
    public GameObject middleDecor2;
    // Variables de constantes a tomar en cuenta para el generador
    private const int LINES_PER_SCREEN = 18;
    private const int TILES_PER_LINE = 20;
    private const int LINE_START = -10;
    private const int BOTTOM_LIMIT = -7;


    //Funcion para iniciar la primera pantalla, para que no comience vacio el juego
    void initFirstScreen()
    {
        for(int i = 0 ; i < LINES_PER_SCREEN; i++)
        {
            _mapMatrix.Add(new _line(i % (int)_lineType.end));
        }

        for(int i = 0 ; i < _mapMatrix.Count; i++)
        {
            generateLine(_mapMatrix[i], (_lineType)_mapMatrix[i].lineType, i);
            if (i == 6)
            {
                _mapMatrix[i].AddObject(groundBasic, new Vector2(0, -1));
            }
        }

    }

    void generateLine(_line line, _lineType type, int lineNumber)
    {
        switch(type){
            case _lineType.low:
                generateGround(line, lineNumber);
                break;
            case _lineType.mid:
                generateMiddle(line, lineNumber);
                break;
            case _lineType.high:
                break;
            default:
                break;
        }
    }

    void generateGround(_line line, int lineNumber)
    {
        // Index we are working on
        int index = 0;
        // We determine the number of platforms in this line
        int platforms = Random.Range(1, 10);
        // Weight for different ground types, will change depending on what is being created
        int basicWeight = 5;
        int hardWeight = 7;
        int softWeight = 9;
        int hazardWeight = 10;

        while (platforms > 0)
        {
            basicWeight = 5;
            hardWeight = 7;
            softWeight = 9;
            hazardWeight = 10;
            for(; index < TILES_PER_LINE; index++)
            {
                if (Random.Range(1, 100) < GroundPopulation)
                {
                    break;
                }
                if (index == TILES_PER_LINE - 1)
                {
                    platforms = 0;
                }
            }

            for(; index < TILES_PER_LINE; index++)
            {
                int tileSelection =Random.Range(0, 10);
                if (tileSelection <= basicWeight){
                    line.AddObject(groundBasic, new Vector2(index + LINE_START, lineNumber + BOTTOM_LIMIT));
                }
                else if (tileSelection <= hardWeight)
                {
                    line.AddObject(groundHard, new Vector2(index + LINE_START, lineNumber + BOTTOM_LIMIT));
                }
                else if (tileSelection <= softWeight)
                {
                    line.AddObject(groundSoft, new Vector2(index + LINE_START, lineNumber + BOTTOM_LIMIT));
                    basicWeight = 2;
                    hardWeight = 3;
                    softWeight = 8;
                    hazardWeight = 10;
                }
                else if (tileSelection <= hazardWeight)
                {
                    line.AddObject(groundHazard, new Vector2(index + LINE_START, lineNumber + BOTTOM_LIMIT));
                    basicWeight = 1;
                    hardWeight = 2;
                    softWeight = 5;
                    hazardWeight = 10;
                }
                if (Random.Range(1, 100) < 100 - GroundSize)
                {
                    break;
                }
                if (index == TILES_PER_LINE - 1)
                {
                    platforms = 0;
                }
            }

            platforms--;
        }
    }

    void generateMiddle(_line line, int lineNumber)
    {
        for(int index = 0; index < _mapMatrix[lineNumber - 1]._lineObjects.Count; index++)
        {
            GameObject prevLine = _mapMatrix[lineNumber - 1]._lineObjects[index];
            if (prevLine != null)
            {
                if (Random.Range(0, 10) < 2)
                {
                    line.AddObject(middleHazard, new Vector2(prevLine.transform.position.x, prevLine.transform.position.y + 1));
                }
            }
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
