using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> PlacedObjects = new Dictionary<Vector3Int, PlacementData>();

    public void AddObject(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);

        foreach (var pos in positionToOccupy)
        {
            if(PlacedObjects.ContainsKey(pos))
            {
                Debug.LogError("Trying to place object on occupied position");
                throw new System.Exception("Trying to place object on occupied position");
            }
            PlacedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
    
            }
        }
       
        return returnVal;
    }

    public bool canPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (pos.x < -5 || pos.x >= 5 || pos.z < -5 || pos.z >= 5)
            {
                return false;
            }
            if (PlacedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if(PlacedObjects.ContainsKey(gridPosition))
        {
            return PlacedObjects[gridPosition].PlacedObjectIndex;
        }
        return -1;
    }

    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in PlacedObjects[gridPosition].occupiedPositions)
        {
            PlacedObjects.Remove(pos);
        }
    }

    public List<PlacementData> GetAllPlacedObjects()
    {
        return new List<PlacementData>(PlacedObjects.Values);
    }

    public bool HasAnyObject()
    {
        return PlacedObjects.Count > 0;
    }


    public void ClearAllData()
    {
        PlacedObjects.Clear();
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int id, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = id;
        PlacedObjectIndex = placedObjectIndex;
    }
}