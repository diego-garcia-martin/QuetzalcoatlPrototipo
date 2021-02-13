using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hacer aparecer tiles bien chidas 

public class sc_tile_spawner : MonoBehaviour
{

    public float TileSpeed;
    public int NivelDeDificultad;
    /*Variables de entrada al script modificables desde Unity, las probabilidades totales deben sumar 100*/
    public int ProbablyNormal;
    public int ProbablyRock;
    public int ProbablySpikes;
    public int ProbablyFreeze;
    public int ProbablyExplosion;
    public int ProbablyFake;
    public int ProbablyCloud;
    /*Prefabs que se utilizaran*/
    public GameObject tileNormal;
    public GameObject tileRock;
    public GameObject tileSpikes;
    public GameObject tileFreeze;
    public GameObject tileExplosion;
    public GameObject tileFake;
    public GameObject tileCloud;

    //Constantes
    private const int POPULATION_TILES = 7;
    private const int MIN_POS_IN_Y = -6;
    private const int MAX_POS_IN_Y = 15;
    private const int MIN_POS_IN_X = -8;
    private const int MAX_POS_IN_X = 8;

    //Indice de tipo de prefabs para diccionario
    private enum TypeTile
    {
        Normal,
        Rock,
        Spikes,
        Freeze,
        Explosion,
        Fake,
        Cloud,
        MaxTypeTile
    }

    //Estructura de datos para diccionario
    private struct TypeTileStruct
    {
        public GameObject gameObject;
        public int probablySpawnerMininum;
        public int probablySpawnerMaximum;
    }


    //Lista de prefabs instanciados en el juego
    private List<List<GameObject>> l_tile;

    //Diccionario de prefabs para usar en el juego
    private Dictionary<TypeTile, TypeTileStruct> dictionaryProbably;

    //Metodo para generar valores flotantes random en unidades
    private float RandomFloatUnit(int min, int max, float offset) 
    {
        return (float) Random.Range(min,max) + offset;
    }
    
    //Metodo para agregar un prefab con su numero de index y la probabilidad de aparicion deseada en el juego
    private void AddObjectsToDictionary(TypeTile type, GameObject Object, int probably)
    {
        TypeTileStruct buf = new TypeTileStruct();

        buf.gameObject = Object;
        buf.probablySpawnerMininum = 1;
        buf.probablySpawnerMaximum = probably;

        dictionaryProbably.Add(type, buf);
    }

    //Metodo para inicializar el diccionario de prefabs para su uso
    private void InitializeDictionaryProbably()
    {
        //Inicia el diccionario
        dictionaryProbably = new Dictionary<TypeTile, TypeTileStruct>();
        //Agrega cada prefab al diccionario con su index y la probabilidad insertada
        AddObjectsToDictionary(TypeTile.Normal, tileNormal, ProbablyNormal);
        AddObjectsToDictionary(TypeTile.Rock, tileRock, ProbablyRock);
        AddObjectsToDictionary(TypeTile.Spikes, tileSpikes, ProbablySpikes);
        AddObjectsToDictionary(TypeTile.Freeze, tileFreeze, ProbablyFreeze);
        AddObjectsToDictionary(TypeTile.Explosion, tileExplosion, ProbablyExplosion);
        AddObjectsToDictionary(TypeTile.Fake, tileFake, ProbablyFake);
        AddObjectsToDictionary(TypeTile.Cloud, tileCloud, ProbablyCloud);

        //Recalcula las probabilidades de manera que cada prefab tenga rangos de probabilidad y no solo porcentajes de probabilidad
        TypeTileStruct value1 = new TypeTileStruct();
        TypeTileStruct value2 = new TypeTileStruct();

        for (TypeTile index = TypeTile.Rock; index < TypeTile.MaxTypeTile; index++)
        {

            dictionaryProbably.TryGetValue(index - 1, out value1);
            dictionaryProbably.TryGetValue(index, out value2);
            value2.probablySpawnerMaximum += value1.probablySpawnerMaximum;
            value2.probablySpawnerMininum = value1.probablySpawnerMaximum + 1;
            dictionaryProbably[index] = value2;

        }
    }

    //Instancia un prefab usando el diccionario de prefabs, el tipo de prefab depende de num calculado de forma random
    public void InsertTile(float x, float y)
    {
        List<GameObject> temp_list = new List<GameObject>();
        int num_tiles = Random.Range(3, 8);
        int num;
        bool clouds = false;
        TypeTileStruct tempTile = new TypeTileStruct();

        //Limitar la posicion de los tiles entre tiles
        if (l_tile != null && l_tile.Count > 0 ) 
        {
            if (x > l_tile[l_tile.Count - 1][0].transform.position.x + 8f)
            {
                x = l_tile[l_tile.Count - 1][0].transform.position.x + 8f;
            }
            else if (x < l_tile[l_tile.Count - 1][0].transform.position.x - 8f) 
            {
                x = l_tile[l_tile.Count - 1][0].transform.position.x - 8f;
            }
        }

        //Hacemos un for con el numero de tiles a meter en este piso
        for (int index = 0; index < num_tiles; index++)
        {
            if (clouds)
            {
                temp_list.Add(Instantiate(tileCloud, new Vector3(x + index, y, 0), Quaternion.identity)); //Modify Vector
            }
            else
            {
                num = Random.Range(1, 100);
                //Explora el diccionario
                for (TypeTile subindex = TypeTile.Normal; subindex < TypeTile.MaxTypeTile; subindex++)
                {
                    dictionaryProbably.TryGetValue(subindex, out tempTile);
                    //Si num esta dentro del rango se instancia el prefab contenido en el indice, termina proceso
                    if (num >= tempTile.probablySpawnerMininum && num <= tempTile.probablySpawnerMaximum)
                    {
                        float rotation = 0;
                        if (subindex != TypeTile.Cloud)
                        {
                            rotation = Random.Range(0, 3) * 90;
                            temp_list.Add(Instantiate(tempTile.gameObject, new Vector3(x + index, y, 0), Quaternion.Euler(0f, 0f, rotation))); //Modify Vector
                        }
                        else
                        {
                            clouds = true;
                            temp_list.Add(Instantiate(tempTile.gameObject, new Vector3(x + index, y, 0), Quaternion.identity)); //Modify Vector
                        }
                        break;
                    }
                }
            }
        }
        l_tile.Add(temp_list);
    }


    // Start is called before the first frame update
    void Start()
    {
        //Starts Objects
        l_tile = new List<List<GameObject>>();
        InitializeDictionaryProbably();

        //Populate screen
        float y = (float)MIN_POS_IN_Y;
        for (int p = 0; p < POPULATION_TILES; p++)
        {
            y += 3f;
            InsertTile(RandomFloatUnit(MIN_POS_IN_X, MAX_POS_IN_X, 0.5f), y);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Mueve cada tile a nueva posicion y elimina la que salga de rango
        for (int index = 0; index < l_tile.Count; index++)
        {
            for (int subindex = 0; subindex < l_tile[index].Count; subindex++)
            {
                l_tile[index][subindex].transform.position = l_tile[index][subindex].transform.position + new Vector3(0, -1f, 0) * Time.deltaTime * TileSpeed;
            }
            //Borra los tiles que van saliendo del mapa
            if (l_tile[index][0].transform.position.y < MIN_POS_IN_Y)
            {
                for (int subindex = 0; subindex < l_tile[index].Count; subindex++)
                {
                    GameObject.Destroy(l_tile[index][subindex]);
                }
                l_tile.RemoveAt(index);
                InsertTile(RandomFloatUnit(MIN_POS_IN_X, MAX_POS_IN_X, 0.5f), (float)MAX_POS_IN_Y);
            }
        }
    }
}
