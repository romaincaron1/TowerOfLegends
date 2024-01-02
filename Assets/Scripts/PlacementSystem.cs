using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO objectsDatabase;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualiazation;

    private GridData gridData;

    private List<GameObject> placedObjects = new List<GameObject>();

    [SerializeField]
    private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;


    private void Start()
    {
        StopPlacement();
        gridData = new GridData();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = objectsDatabase.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex <0)
        {
            Debug.LogError("Object with ID: " + ID + " does not exist in the database");
            return;
        }
        previewSystem.StartShowingPlacementPreview(objectsDatabase.objectsData[selectedObjectIndex].Prefab, objectsDatabase.objectsData[selectedObjectIndex].Size);
        gridVisualiazation.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValiditiy = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if(placementValiditiy == false)
        {
            return;
        }

        Vector2Int buildingSize = objectsDatabase.objectsData[selectedObjectIndex].Size;
        Debug.Log("Building size: " + buildingSize);
        Vector3 offset = new Vector3(buildingSize.x * 2.5f, 0f, buildingSize.y * 2.5f);
        Debug.Log("offset: " + offset);
        Debug.Log("gridPosition: " + gridPosition);
        Vector3 cellCenterPosition = grid.CellToWorld(gridPosition) + offset;
        GameObject newObject = Instantiate(objectsDatabase.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = cellCenterPosition;

        placedObjects.Add(newObject);
        GridData selectedData = gridData;
        selectedData.AddObject(gridPosition, objectsDatabase.objectsData[selectedObjectIndex].Size, objectsDatabase.objectsData[selectedObjectIndex].ID, placedObjects.Count - 1);

        previewSystem.UpdatePosition(cellCenterPosition, false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = gridData;

        gridPosition.x += objectsDatabase.objectsData[selectedObjectIndex].Size.x;
        gridPosition.z += objectsDatabase.objectsData[selectedObjectIndex].Size.y;

        // Est utile pour vérifier les objects avec une size > 1
        if (gridPosition.x < 6 && gridPosition.z < 6)
        {
            return selectedData.canPlaceObjectAt(gridPosition, objectsDatabase.objectsData[selectedObjectIndex].Size);
        }
        return false;
    }

    private void StopPlacement()
    {
        Debug.Log("StopPlacement");
        selectedObjectIndex = -1;

        previewSystem.StopShowingPlacementPreview();
        gridVisualiazation.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;        
    }   

    void Update()
    {
        if(selectedObjectIndex < 0)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(gridPosition != lastDetectedPosition)
        {
            bool placementValiditiy = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            // Ajoutez un décalage ici pour centrer le rectangle sur la cellule*
            Vector2Int buildingSize = objectsDatabase.objectsData[selectedObjectIndex].Size;
            Vector3 offset = new Vector3(buildingSize.x * 2.5f, 0f, buildingSize.y * 2.5f);
            Vector3 cellCenterPosition = grid.CellToWorld(gridPosition)+ offset;
            // Assurez-vous que le rectangle est positionné au centre de la cellule

            previewSystem.UpdatePosition(cellCenterPosition, placementValiditiy);

            cellCenterPosition += offset;
            lastDetectedPosition = gridPosition;
        }
    }

}
