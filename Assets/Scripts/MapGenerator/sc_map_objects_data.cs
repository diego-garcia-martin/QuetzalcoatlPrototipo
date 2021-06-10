using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

public class sc_map_objects_data : MonoBehaviour
{
    public struct ObjectInformation 
    {
        public int Rarely;
        public GameObject gameObject;
    }
    public class ObjectsTableOnStage // Objects per Scenerio
    {
        public Dictionary<string, Dictionary<string, ObjectInformation>> l_dObjectsTableOnStage;
        public ObjectsTableOnStage() 
        {
            l_dObjectsTableOnStage = new Dictionary<string, Dictionary<string, ObjectInformation>>();
        }

        //Access for Type - NameObject - ObjectInformation
    }

    private List<string> m_lTypeObjects;

    private List<string> m_lScenariosInTable;

    private Dictionary<string, List<string>> m_lObjectsInTable;

    private Dictionary<string, ObjectsTableOnStage> m_sObjectsTableDictionary;

    private bool CreateGameObjectsDiccionary()
    {
        bool Ok = false;
        //Obten la ruta donde esta el archivo de la lista de objectos y verifica que existe
        string path = Directory.GetCurrentDirectory() + @"\Assets\Resources\objectsList.csv";
        if (File.Exists(path))
        {
            //Guarda la informacion bruta del archivo en un string
            string fileData = File.ReadAllText(path);
            //Guarda en una matriz de string cada renglon por separado
            string[] lines = fileData.Split('\n');
            //Lee el primer renglon de texto, divide y guarda cada elemento por separado
            m_lTypeObjects = new List<string>(lines[0].Split(','));
            Debug.Log("m_lTypeObjects size: " + m_lTypeObjects.Count.ToString());
            //Elimina el primer elemento, es basura, solo es apoyo al usuario, lista de tipos de objectos obtenida
            m_lTypeObjects.RemoveAt(0);
            Debug.Log("m_lTypeObjects size: " + m_lTypeObjects.Count.ToString());
            //Elimina elementos vacios
            m_lTypeObjects = cleanEmptyElements(m_lTypeObjects);
            //Crea una lista de dos dimensiones para el resto de los datos en el archivo
            List<List<string>> allData = new List<List<string>>();
            //Guarda el contenido de la matriz de renglones en una matriz de escenarios y objectos separando cada uno de sus elementos
            for (int indexlines = 1; indexlines < lines.Length; indexlines++)
            {
                allData.Add(new List<string>(lines[indexlines].Split(',')));
            }
            //Crea la lista que contendra los valores en las objectos, copia la primera escenario de texto
            List<string> ObjectsInTable = new List<string>(allData[0]);
            //Elimina el primer elemento de la lista de objectos por ser basura
            ObjectsInTable.RemoveAt(0);
            //Elimina la escenario de nombres de objecto de la tabla de datos total
            allData.RemoveAt(0);
            //Limpia la tabla de elementos en objecto
            ObjectsInTable = cleanEmptyElements(ObjectsInTable);
            //Crea la lista de elementos en escenario
            m_lScenariosInTable = new List<string>();
            //Accede por medio de indexado
            for (int indexRows = 0; indexRows < allData.Count; indexRows++) 
            {
                //Copia el elemento a la lista de elementos en escenario, solo el primero
                m_lScenariosInTable.Add(allData[indexRows][0]);
                //Elimina ese elemento de la tabla total
                allData[indexRows].RemoveAt(0);
            }
            //Limpia la tabla de elementos vacios en escenario
            m_lScenariosInTable = cleanEmptyElements(m_lScenariosInTable);

            /*Tipos de objectos, Escenarios, Objectos y Disponibilidad de objectos se ha leido y separado,
             con esto continua la interpretacion y ordenamiento en un diccionario*/

            //Creacion de diccionario maestro
            m_sObjectsTableDictionary = new Dictionary<string, ObjectsTableOnStage>();
            //Carga todos los prefabs disponibles en Resources y la tabla
            Dictionary<string, ObjectInformation> prefabslist = loadPrefabs(ObjectsInTable);
            //Por cada Escenario...
            for (int indexScenario = 0; indexScenario < m_lScenariosInTable.Count; indexScenario++) 
            {
                //Carga una lista de objectos de ese escenario
                ObjectsTableOnStage TempObjectsTableOnStage = new ObjectsTableOnStage();
                //Por cada tipo...
                for (int indexTypes = 0; indexTypes < m_lTypeObjects.Count; indexTypes++) 
                {
                    Dictionary<string, ObjectInformation> tempDictionary = new Dictionary<string, ObjectInformation>();
                    //Y por cada Objecto disponible en la lista...
                    for (int indexObjects = 0; indexObjects < ObjectsInTable.Count; indexObjects++) 
                    {
                        //Revisa en la tabla si el objecto esta disponible para ese escenario y coincide con el tipo actual
                        if (allData[indexScenario][indexObjects] != "" && ObjectsInTable[indexObjects].Contains(m_lTypeObjects[indexTypes])) 
                        {
                            //Si lo esta, obten el objecto de la lista cargada y guarala en un diccionario que solo contenga los objectos de ese escenario
                            ObjectInformation tempObjectInformation = new ObjectInformation();
                            prefabslist.TryGetValue(ObjectsInTable[indexObjects], out tempObjectInformation);
                            tempObjectInformation.Rarely = int.Parse(allData[indexScenario][indexObjects]);
                            tempDictionary.Add(ObjectsInTable[indexObjects], tempObjectInformation);
                        }
                    }
                    //Agrega la lista de objectos filtrada a una lista que contendra cada lista de cada tipo...
                    TempObjectsTableOnStage.l_dObjectsTableOnStage.Add(m_lTypeObjects[indexTypes], tempDictionary);
                }
                //Y finalmente agrega cada una de esas listas a un diccionario que contendra toda la informacion decodificada y disponible para su uso
                m_sObjectsTableDictionary.Add(m_lScenariosInTable[indexScenario], TempObjectsTableOnStage);
            }
            m_lObjectsInTable = new Dictionary<string, List<string>>();
            for (int indexType = 0; indexType < m_lTypeObjects.Count; indexType++) 
            {
                List<string> listTemp = new List<string>();
                for (int indexObjects = 0; indexObjects < ObjectsInTable.Count; indexObjects++) 
                {
                    if (ObjectsInTable[indexObjects].Contains(m_lTypeObjects[indexType])) 
                    {
                        listTemp.Add(ObjectsInTable[indexObjects]);
                    }
                }
                m_lObjectsInTable.Add(m_lTypeObjects[indexType], listTemp);
            }

            Ok = true;
        }
        else
        {
            Debug.LogError("sc_map_objects_data: loadCVSFile: Error: objectsList.csv do not exist!!");
        }
        return Ok;
    }

    //Carga una lista de prefabs que haya en Resources dependiendo de su clasificacion por tipo
    private Dictionary<string, ObjectInformation> loadPrefabs( List<string> objectsList) 
    {
        //Crea la lista que contendra los prefabs
        Dictionary<string, ObjectInformation> GameObjectsList = new Dictionary<string, ObjectInformation>();
        //Obten todas las rutas de archivos de prefab que haya en la carpeta de Resources
        string path = Directory.GetCurrentDirectory() + @"\Assets\Resources\";
        string[] prefabsList = Directory.GetFiles(path, "*.prefab");
        string prefabsString = "";
        //Limpia el nombre de los prefabs y agrega a un string total
        for (int index = 0; index < prefabsList.Length; index++) 
        {
            prefabsString += (prefabsList[index].Replace(".prefab", "")).Replace(path, "");
        }
        //Carga a la lista de GameObjects cada prefab que encuentre en la carpeta y lo deja disponible para instanciar
        for(int index = 0; index < objectsList.Count; index++)
        {
            if (prefabsString.Contains(objectsList[index])) 
            {
                ObjectInformation Objectbuffer = new ObjectInformation();
                Objectbuffer.gameObject = Resources.Load<GameObject>(objectsList[index]);
                GameObjectsList.Add(objectsList[index], Objectbuffer);
            }
            //GameObjectsList.Add(Resources.Load<GameObject>((filePaths[indexFile].Replace(".prefab", "")).Replace(path, "")));
        }
        return GameObjectsList;
    }

    private List<string> cleanEmptyElements(List<string> list) 
    {
        list.RemoveAt(list.Count -1);
        int index = list.Count - 1;
        while (index >= 0) 
        {
            if (list[index] == "") 
            {
                Debug.Log("cleanEmptyElements delete item index: " + index.ToString());
                list.RemoveAt(index);
            }
            index--;
        }

        for (int i = 0; i < list.Count; i++) 
        {
            Debug.Log(list[i]);
        }

        return list;
    }

    public List<string> getListTypeObjects() 
    {
        return m_lTypeObjects;
    }

    public List<string> getListScenarios() 
    {
        return m_lScenariosInTable;
    }

    public List<string> getListObjectsForStage(string Scenario) 
    {
        List<string> list = new List<string>();
        m_lObjectsInTable.TryGetValue(Scenario, out list);
        return list;
    }

    public ObjectsTableOnStage getObjectsTableOnStage(string Scenario) 
    {
        ObjectsTableOnStage objectsTableOnStage = new ObjectsTableOnStage();
        m_sObjectsTableDictionary.TryGetValue(Scenario, out objectsTableOnStage);
        return objectsTableOnStage;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateGameObjectsDiccionary();
        List<string> scenarios = getListScenarios();
        List<string> types = getListTypeObjects();
        List<string> objectsJungle = getListTypeObjects();
        ObjectsTableOnStage objectsTableOnStage = getObjectsTableOnStage(scenarios[0]);
        Dictionary<string, ObjectInformation> buffer = new Dictionary<string, ObjectInformation>();
        objectsTableOnStage.l_dObjectsTableOnStage.TryGetValue(types[0], out buffer);
        ObjectInformation obj = new ObjectInformation();
        buffer.TryGetValue(objectsJungle[0], out obj);
        Instantiate(obj.gameObject, new Vector3(12.5f, 5.5f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
