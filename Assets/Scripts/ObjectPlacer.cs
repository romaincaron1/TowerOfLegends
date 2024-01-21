using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        placedObjects.Add(newObject);
        return placedObjects.Count - 1;
    }

    public void RemoveObjectAt(int index)
    {
        if(placedObjects.Count <= index)
        {
            Debug.LogError("Trying to remove object that does not exist");
            return;
        }
        Destroy(placedObjects[index]);
        placedObjects[index] = null;
    }
    public void RemoveAllObjects()
    {
        foreach (GameObject obj in placedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        placedObjects.Clear();
    }

}
