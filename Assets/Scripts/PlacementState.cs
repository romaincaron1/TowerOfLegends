using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData gridData;
    ObjectPlacer objectPlacer;

    public PlacementState(int ID, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData gridData, ObjectPlacer objectPlacer)
    {
        this.ID = ID;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex >-1){
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        }else{
            Debug.LogError("Object with ID: " + ID + " does not exist in the database");
        }
        
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValiditiy = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if(placementValiditiy == false)
        {
            return;
        }

        Vector2Int buildingSize = database.objectsData[selectedObjectIndex].Size;
        Vector3 offset = new Vector3(buildingSize.x * 2.5f, 0f, buildingSize.y * 2.5f);
        Vector3 cellCenterPosition = grid.CellToWorld(gridPosition) + offset;

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, cellCenterPosition);
        
        GridData selectedData = gridData;
        selectedData.AddObject(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);

        previewSystem.UpdatePosition(cellCenterPosition, false);
        ResourceManager.instance.SpendCoins(database.objectsData[selectedObjectIndex].Price);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = gridData;
        return selectedData.canPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValiditiy = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        Vector2Int buildingSize = database.objectsData[selectedObjectIndex].Size;
        Vector3 offset = new Vector3(buildingSize.x * 2.5f, 0f, buildingSize.y * 2.5f);
        Vector3 cellCenterPosition = grid.CellToWorld(gridPosition)+ offset;

        previewSystem.UpdatePosition(cellCenterPosition, placementValiditiy);
    }
}
