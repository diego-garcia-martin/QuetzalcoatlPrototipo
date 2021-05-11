using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_MapGenerator : MonoBehaviour
{

    //******************************************************** LINE LOGIC ************************************************************
    // En esta parte del codigo se describe la linea de objetos, con sus metodos y variables para usar despues en la matriz
    public enum _lineType{
        low,
        mid,
        high,
        end,
    };
    class _line{
        public List<GameObject> _lineObjects;
        public _lineType lineType;
        public float linePos;

        public _line(int type, float pos){
            lineType = (_lineType)type;
            linePos = pos;
            _lineObjects = new List<GameObject>();
        }

        public _line(_lineType type, float pos){
            lineType = type;
            linePos = pos;
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
            for (int index = _lineObjects.Count - 1; index >= 0; index--)
            {
                if (_lineObjects[index] == null)
                {
                    _lineObjects.RemoveAt(index);
                }
            } 
        }

        public void EmptyLine()
        {
            for (int index = _lineObjects.Count - 1; index >= 0; index--)
            {
                if (_lineObjects[index] != null)
                {
                    GameObject.Destroy(_lineObjects[index]);
                }
                _lineObjects.RemoveAt(index);
            }
        }

        public void MoveLineDown(float movY)
        {
            linePos = linePos - (movY * Time.deltaTime);
            RemoveEmpty();
            foreach (GameObject tile in _lineObjects)
            {
                tile.transform.position = new Vector3(tile.transform.position.x, linePos, 0);
            }
        }
    };


    // ************************************************************ MATRIX LOGIC **************************************************************
    // Lista con objetos para cada linea del mapa
    private List<_line> _mapMatrix;
    public int GapSize = 5;
    public int HazardChance = 10;
    public float LineSpeed = 0.3f;
    //Aqui van los objetos que son los tipos de pisos, decoraciones, etc.
    public GameObject groundBasic;
    public GameObject groundHard;
    public GameObject groundSoft;
    public GameObject groundHazard;
    public GameObject middleHazard;
    public GameObject middleDecor1;
    public GameObject middleDecor2;
    // Variables de constantes a tomar en cuenta para el generador
    private const int LINES_PER_SCREEN = 20;
    private const int TILES_PER_LINE = 30;
    private const int LINE_START = -15;
    private const int BOTTOM_LIMIT = -10;

    GameObject selectRandomGround()
    {
        int selection = Random.Range(0, 3);
        switch(selection){
            case 0:
                return groundBasic;
            case 1:
                return groundHard;
            case 2:
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
            _mapMatrix.Add(new _line(i % (int)_lineType.end, i + BOTTOM_LIMIT));
        }

        for(int i = 0 ; i < _mapMatrix.Count; i++)
        {
            generateLine(_mapMatrix[i], i);
            if (i == 6)
            {
                _mapMatrix[i].AddObject(groundBasic, new Vector2(0, -1));
            }
        }

    }

    void generateLine(_line line, int lineNumber)
    {
        switch(line.lineType){
            case _lineType.low:
                generateGround(line);
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

    void generateGround(_line line)
    {
        // Index we are working on
        int index = 0;
        
        while (index < TILES_PER_LINE && (index + LINE_START <= 10))
        {
            index += Random.Range(Mathf.FloorToInt(GapSize/2 - 1), GapSize);
            int blocksize = Random.Range(1, 5);
            GameObject baseGround = selectRandomGround();
            GameObject otherGround = groundBasic;
            if (baseGround.GetComponent<sc_tile_ID>().id == "normal") otherGround = groundHard;
            else if (baseGround.GetComponent<sc_tile_ID>().id == "hard") otherGround = groundBasic;
            else if (baseGround.GetComponent<sc_tile_ID>().id == "soft") otherGround = groundHazard;
            else if (baseGround.GetComponent<sc_tile_ID>().id == "hazard") otherGround = groundSoft;
            switch(blocksize){
                case 1:
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    break;
                case 2:
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    if (Random.Range(0, 2) == 0)
                    {
                        line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    else{
                        if (baseGround.GetComponent<sc_tile_ID>().id == "hard")
                        {
                            line.AddObject(otherGround, new Vector2(index + LINE_START, line.linePos));
                            index++;
                        }
                    }
                    break;
                case 3:
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    if (Random.Range(0, 2) == 0)
                    {
                        line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    else{
                        line.AddObject(selectRandomGround(), new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    break;
                case 4:
                    GameObject innerGround = selectRandomGround();
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    if (Random.Range(0, 2) == 0)
                    {
                        line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                        line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    else{
                        line.AddObject(innerGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                        line.AddObject(innerGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    break;
                case 5:
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    if (Random.Range(0, 2) == 0)
                    {
                        line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    else{
                        line.AddObject(otherGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                    index++;
                    if (Random.Range(0, 2) == 0)
                    {
                        line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    else{
                        line.AddObject(otherGround, new Vector2(index + LINE_START, line.linePos));
                        index++;
                    }
                    line.AddObject(baseGround, new Vector2(index + LINE_START, line.linePos));
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
                if ((Random.Range(0, 100) < HazardChance) && (prevLine.GetComponent<sc_tile_ID>().id == "normal" || prevLine.GetComponent<sc_tile_ID>().id == "hard"))
                {
                    line.AddObject(middleHazard, new Vector2(prevLine.transform.position.x, prevLine.transform.position.y + 1));
                }
            }
        }
    }

    void moveMapDown()
    {
        for (int index = _mapMatrix.Count - 1; index >= 0; index--)
        {
            _mapMatrix[index].MoveLineDown(LineSpeed);
            if (_mapMatrix[index].linePos < BOTTOM_LIMIT - 1)
            {
                _mapMatrix.Add(new _line(_mapMatrix[index].lineType, LINES_PER_SCREEN + BOTTOM_LIMIT));
                generateLine(_mapMatrix[_mapMatrix.Count-1], _mapMatrix.Count-1);
                _mapMatrix[index].EmptyLine();
                _mapMatrix.RemoveAt(index);
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
    void FixedUpdate()
    {
        moveMapDown();
    }
}
