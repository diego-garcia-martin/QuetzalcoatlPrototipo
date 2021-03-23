using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_LineConstructor : MonoBehaviour
{
    private List<GameObject> LineObjects;
    public string LineType;
    public int LineSize;

    public void initConstructor()
    {
        LineType = "";
        LineSize = 0;
        LineObjects = new List<GameObject>();
    }

    public int getLineSize()
    {
        return LineSize;
    }

    public string getLineType()
    {
        return LineType;
    }

    public void setLineType(string type)
    {
        LineType = type;
    }

    public void AddObject(GameObject obj, Vector2 pos)
    {
        LineObjects.Add(Instantiate(obj, new Vector3(pos.x, pos.y, 0), Quaternion.identity));
        LineSize++;
    }
    public void RemoveObjectAt(int index)
    {
        if (index < LineObjects.Count)
        {
            if (LineObjects[index])
            {
                GameObject.Destroy(LineObjects[index]);
                LineObjects.RemoveAt(index);
                LineSize--;
            }
        }
    }

    public void EmptyLine()
    {
        RemoveEmpty();
        for (int index = 0; index < LineObjects.Count; index++)
        {
            if (LineObjects[index])
            {
                GameObject.Destroy(LineObjects[index]);
                LineObjects.RemoveAt(index);
                LineSize--;
            }
        }
    }

    public void RemoveEmpty()
    {
       for (int index = 0; index < LineObjects.Count; index++)
        {
            if (LineObjects[index] == null)
            {
                LineObjects.RemoveAt(index);
                LineSize--;
            }
        } 
    }

    public void MoveLineDown(float movY)
    {
        RemoveEmpty();
       for (int index = 0; index < LineObjects.Count; index++)
        {
            if (LineObjects[index])
            {
                LineObjects[index].transform.position = new Vector3(LineObjects[index].transform.position.x, LineObjects[index].transform.position.y - movY, 0);
            }
        } 
    }
}