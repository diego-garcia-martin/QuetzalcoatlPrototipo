using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_map_gen_main : MonoBehaviour
{
    //Variables Generales
    public float TileSpeed;
    public int NivelDeDificultad;
    //Probabilidad de aparicion de escenarios, deben sumar 100 en total y no ser 0
    public int ProbabilidadEscenarioNormal;
    public int ProbabilidadEscenarioCongelado;
    public int ProbabilidadEscenarioFalso;
    public int ProbabilidadEscenarioDeNubes;
    //Probabilidad de aparicion de Tipos de Plataformas en cada escenario, se modifican dependiendo del escenario, deben sumar 100 y no ser 0
    public int ProbabilidadPlataformaNormal;
    public int ProbabilidadPlataformaRoca;
    public int ProbabilidadPlataformaCongelada;
    public int ProbabilidadPlataformaFalsa;
    public int ProbabilidadPlataformaNube;
    //Probabilidad de aparicion de Tipos de Trampas en los escenarios, no deben ser 0, la probabilidad restante a 100 se asiganara a posibilidad de no aparecer nada
    public int ProbabilidadDePicos;
    public int ProbabilidadDeExplosion;
    //Probabilidad de aparicion de miscelanios en el mapa, no debe ser 0, la probabilidad restante a 100 se asignara a posibililidad de no aparecer nada
    public int ProbabilidadDeMiscelaneos;

    //Entrada de Objetos
    public GameObject TileNormal;
    public GameObject TileRock;
    public GameObject TileFrezee;
    public GameObject TileFake;
    public GameObject TileCloud;
    public GameObject TrampSpikes;
    public GameObject TrampExplosion;
    public GameObject Miscelaneos; //Discusion con equipo para revisar cuales existiran y crear el diccionario correspondiente

    //Diccionario de Objectos
    private enum eDiccionaryTypeObject
    {
        TypeObjectStart,
        TypeObjectTiles = TypeObjectStart,
        TypeObjectTramp,
        TypeObjectMiscelaneo,
        TypeObjectEnd
    }

    //Indice de probabilidad de escenarios
    private enum eTypeEscenarios
    {
        TypeEscenariosStart,
        TypeNormal = TypeEscenariosStart,
        TypeCongelado,
        TypeFalso,
        TypeNubes,
        TypeEscenariosEnd
    }

    private enum eMACROLEVEL 
    {
        LevelTerra,
        LevelMountain,
        LevelSky,
        LevelSpace
    }

    private struct StructDataObject
    {
        public GameObject Object;
        public int ProbabilityMinor;
        public int ProbabilityMayor;
    }

    private struct StructEscenarioData
    {
        public int ProbabilityMinor;
        public int ProbabilityMayor;
    }

    private Dictionary<eDiccionaryTypeObject, List<StructDataObject>> DictionaryOfGameObjects;

    private Dictionary<eTypeEscenarios, StructEscenarioData> ProbablyEscenarios;

    private List<GameObject> ObjectsListInTheScreen;

    private eMACROLEVEL MacroLevel;

    private const float COORDENATE_HIGH_Y = 22.5f;
    private const float COORDENATE_LOW_Y = -13.5f;
    private const float COORDENATE_LEFT_X = -8.5f;
    private const float COORDENATE_RIGHT_X = 8.5f;
    private const float UNIT_FLOAT = 1f;
    private const int OBJECTS_IN_SCREEN_X = 18;
    private const int OBJECTS_IN_SCREEN_Y = 10;


    private List<StructDataObject> initTilesObjects() 
    {
        List<StructDataObject> listRet = new List<StructDataObject>();
        listRet.Add(new StructDataObject() { Object = TileNormal, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadPlataformaNormal });
        listRet.Add(new StructDataObject() { Object = TileRock, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadPlataformaRoca });
        listRet.Add(new StructDataObject() { Object = TileFrezee, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadPlataformaCongelada });
        listRet.Add(new StructDataObject() { Object = TileFake, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadPlataformaFalsa });
        listRet.Add(new StructDataObject() { Object = TileCloud, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadPlataformaNube });
        return listRet;
    }

    private List<StructDataObject> initTrampsObjects() 
    {
        List<StructDataObject> listRet = new List<StructDataObject>();
        listRet.Add(new StructDataObject() { Object = TrampSpikes, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadDePicos });
        listRet.Add(new StructDataObject() { Object = TrampExplosion, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadDeExplosion });
        return listRet;
    }

    private List<StructDataObject> initMicelaneosObjects()
    {
        List<StructDataObject> listRet = new List<StructDataObject>();
        listRet.Add(new StructDataObject() { Object = Miscelaneos, ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadDeMiscelaneos });
        return listRet;
    }

    private void initDictionaryOfGameObjects() 
    {
        //Inicia y agrega listas de objetos de juego
        DictionaryOfGameObjects = new Dictionary<eDiccionaryTypeObject, List<StructDataObject>>();
        DictionaryOfGameObjects.Add(eDiccionaryTypeObject.TypeObjectTiles, initTilesObjects());
        DictionaryOfGameObjects.Add(eDiccionaryTypeObject.TypeObjectTramp, initTrampsObjects());
        DictionaryOfGameObjects.Add(eDiccionaryTypeObject.TypeObjectMiscelaneo, initMicelaneosObjects());

        //Reorganiza probabilidades

        for (eDiccionaryTypeObject index = eDiccionaryTypeObject.TypeObjectStart; index < eDiccionaryTypeObject.TypeObjectEnd; index++) 
        {
            List<StructDataObject> bufList = new List<StructDataObject>();
            DictionaryOfGameObjects.TryGetValue(index, out bufList);
            for (int index2 = 1; index2 < bufList.Count; index2++) 
            {
                StructDataObject buf = new StructDataObject();
                buf = bufList[index2];
                buf.ProbabilityMinor = bufList[index2 - 1].ProbabilityMayor + 1;
                buf.ProbabilityMayor += bufList[index2 - 1].ProbabilityMayor;
                bufList[index2] = buf;
            }
        }
    }

    private void initProbablyEscenarios() 
    {
        ProbablyEscenarios = new Dictionary<eTypeEscenarios, StructEscenarioData>();
        ProbablyEscenarios.Add(eTypeEscenarios.TypeNormal, new StructEscenarioData() { ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadEscenarioNormal });
        ProbablyEscenarios.Add(eTypeEscenarios.TypeCongelado, new StructEscenarioData() { ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadEscenarioCongelado });
        ProbablyEscenarios.Add(eTypeEscenarios.TypeFalso, new StructEscenarioData() { ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadEscenarioFalso });
        ProbablyEscenarios.Add(eTypeEscenarios.TypeNubes, new StructEscenarioData() { ProbabilityMinor = 0, ProbabilityMayor = ProbabilidadEscenarioDeNubes });

        for (eTypeEscenarios index = eTypeEscenarios.TypeEscenariosStart + 1; index < eTypeEscenarios.TypeEscenariosEnd; index++) 
        {
            StructEscenarioData valueObjective = new StructEscenarioData();
            StructEscenarioData valueBefore = new StructEscenarioData();
            ProbablyEscenarios.TryGetValue(index, out valueObjective);
            ProbablyEscenarios.TryGetValue(index - 1, out valueBefore);
            valueObjective.ProbabilityMinor = valueBefore.ProbabilityMayor + 1;
            valueObjective.ProbabilityMayor += valueBefore.ProbabilityMayor;
            ProbablyEscenarios[index] = valueObjective;
        }
    }

    private void CreateLevel(bool ignoreRandom = false, eTypeEscenarios manualScenario = 0) 
    {
        eTypeEscenarios scenario;
        if (ignoreRandom)
        {
            scenario = manualScenario;
        }
        else 
        {
            int scenarioProbably = Random.Range(0, 100);
            for (scenario = eTypeEscenarios.TypeEscenariosStart; scenario < eTypeEscenarios.TypeEscenariosEnd; scenario++)
            {
                StructEscenarioData dataScenario = new StructEscenarioData();
                ProbablyEscenarios.TryGetValue(scenario, out dataScenario);
                if (dataScenario.ProbabilityMinor >= scenarioProbably && dataScenario.ProbabilityMayor <= scenarioProbably) 
                {
                    break;
                }
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
