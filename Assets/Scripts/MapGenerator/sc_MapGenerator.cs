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
    // Lista con los tipos de suelo
    GameObject[] groundLiszt = {groundBasic, groundHard, groundSoft, groundHazard};
    // Variables de constantes a tomar en cuenta para el generador
    private const int LINES_PER_SCREEN = 18;
    private const int TILES_PER_LINE = 20;
    private const int LINE_START = -10;
    private const int BOTTOM_LIMIT = -7;

    GameObject selectRandomGround()
    {
        int selection = Random.Range(0, 3);
        switch(selection){
            case 0:
                return groundBasic;
            case 1:
                return groundHard;
            case 2:4
                return groundSoft;
            case 3:
                return groundHazard;
            default:
                return groundBasic;
        }
    }

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
        
        while (index <= TILES_PER_LINE)
        {
            index += Random.Range(1, 5);
            int blocksize = Random.Range(1, 5);
            GameObject baseGround = selectRandomGround();
            switch(blocksize){
                case 1:
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    break;
                case 2:
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    if (Random.Range(0, 1) == 0)
                    {
                        line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                        index++;
                    }
                    else
                    {
                        line.AddObject(groundBasic, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                        index++;
                    }
                    break;
                case 3:
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(selectRandomGround(), new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    break;
                case 4:
                    GameObject innerGround = selectRandomGround();
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(innerGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(innerGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    break;
                case 5:
                    GameObject innerGround2 = selectRandomGround();
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(innerGround2, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(innerGround2, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    line.AddObject(baseGround, new Vector2(index, lineNumber + BOTTOM_LIMIT));
                    index++;
                    break;
            }
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
