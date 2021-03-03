using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hacer aparecer tiles bien chidas 

public class sc_tile_spawner : MonoBehaviour
{

    public float TileSpeed;
    public int NivelDeDificultad;
    /*Variables de entrada al script modificables desde Unity, las probabilidades totales deben sumar 100*/
    public int ProbablySpawnTileNormal;
    public int ProbablySpawnTileRock;
    public int ProbablySpawnTileFreeze;
    public int ProbablySpawnTileFake;
    public int ProbablySpawnTileCloud;
    /*Variables de probabilidad de aparecer trampas sobre tiles*/
    public int ProbablySpawnSpikes;
    public int ProbablySpawnExplosion;
    /*Variable de probabilidad de aparecer micelanios sobre tiles*/
    public int ProbablySpawnMicelanios;

    /*Prefabs que se utilizaran*/
    public GameObject tileNormal;
    public GameObject tileRock;
    public GameObject tileFreeze;
    public GameObject tileFake;
    public GameObject tileCloud;
    public GameObject objectSpikes;
    public GameObject objectExplosion;
    public GameObject objectMicelanio;

    //Constantes
    private const int SIZE_MATRIX_X = 10;
    private const int SIZE_MATRIX_Y = 18;
    private const float POSITION_MIN_X = -8.5f;
    private const float POSITION_MIN_Y = 20.5f;
    private const float SCREEN_STEP = 1f;

    //Indice de tipo de prefabs para diccionario
    private enum TypeObject
    {
        MinTypeTile,
        Normal = MinTypeTile,
        Rock,
        Freeze,
        Fake,
        Cloud,
        MaxTypeTile,
        MinTypeTramp = MaxTypeTile,
        Spikes = MinTypeTramp,
        Explosion,
        MaxTypeTramp,
        MinMicelaneos = MaxTypeTramp,
        Miscelaneos = MinMicelaneos,
        MaxMiscelaneos
    }

    //Estructura de datos para diccionario
    private struct TypeObjectStruct
    {
        public GameObject gameObject;
        public int probablySpawnerMininum;
        public int probablySpawnerMaximum;
    }

    //Lista de prefabs instanciados en el juego
    private List<GameObject> l_tile;

    //Diccionario de prefabs para usar en el juego
    private Dictionary<TypeObject, TypeObjectStruct> dictionaryObjectsProbably;

    //Calcula nuevas coordenadas
    private Vector3 CalculateCoordenatesForSpawn(int x, int y) 
    {
        float newX = POSITION_MIN_X + (float)x;
        float newY = POSITION_MIN_Y + (float)y;
        return new Vector3(newX, newY, 0);
    }
    
    //Metodo para agregar un prefab con su numero de index y la probabilidad de aparicion deseada en el juego
    private void AddObjectsToDictionary(TypeObject type, GameObject Object, int probably)
    {
        TypeObjectStruct buf = new TypeObjectStruct();

        buf.gameObject = Object;
        buf.probablySpawnerMininum = 1;
        buf.probablySpawnerMaximum = probably;

        dictionaryObjectsProbably.Add(type, buf);
    }

    //Metodo para inicializar el diccionario de prefabs para su uso
    private void InitializeDictionaryObjectsProbably()
    {
        //Inicia el diccionario
        dictionaryObjectsProbably = new Dictionary<TypeObject, TypeObjectStruct>();
        //Agrega cada prefab al diccionario con su index y la probabilidad insertada para las tiles
        AddObjectsToDictionary(TypeObject.Normal, tileNormal, ProbablySpawnTileNormal);
        AddObjectsToDictionary(TypeObject.Rock, tileRock, ProbablySpawnTileRock);
        AddObjectsToDictionary(TypeObject.Freeze, tileFreeze, ProbablySpawnTileFreeze);
        AddObjectsToDictionary(TypeObject.Fake, tileFake, ProbablySpawnTileFake);
        AddObjectsToDictionary(TypeObject.Cloud, tileCloud, ProbablySpawnTileCloud);
        //Agrega cada prefab de trampas
        AddObjectsToDictionary(TypeObject.Spikes, objectSpikes, ProbablySpawnSpikes);
        AddObjectsToDictionary(TypeObject.Explosion, objectExplosion, ProbablySpawnExplosion);
        //Agrega Micelanios
        AddObjectsToDictionary(TypeObject.Miscelaneos, objectMicelanio, ProbablySpawnMicelanios);

        //Recalcula las probabilidades de manera que cada prefab tenga rangos de probabilidad y no solo porcentajes de probabilidad para Tiles
        TypeObjectStruct value1 = new TypeObjectStruct();
        TypeObjectStruct value2 = new TypeObjectStruct();

        for (TypeObject index = TypeObject.MinTypeTile + 1; index < TypeObject.MaxTypeTile; index++)
        {

            dictionaryObjectsProbably.TryGetValue(index - 1, out value1);
            dictionaryObjectsProbably.TryGetValue(index, out value2);
            value2.probablySpawnerMaximum += value1.probablySpawnerMaximum;
            value2.probablySpawnerMininum = value1.probablySpawnerMaximum + 1;
            dictionaryObjectsProbably[index] = value2;
        }


    }

    //Instancia un prefab usando el diccionario de prefabs, el tipo de prefab depende de num calculado de forma random
    public void InsertObject(int x, int y)
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        //Starts Objects
        l_tile = new List<GameObject>();
        InitializeDictionaryObjectsProbably();

        //Populate screen
    }

    // Update is called once per frame
    void Update()
    {
        //Mueve cada tile a nueva posicion y elimina la que salga de rango
        
    }
}
