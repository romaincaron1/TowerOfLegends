using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    public Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO objectsDatabase;

    [SerializeField]
    private GameObject gridVisualiazation;

    private GridData gridData;

    [SerializeField]
    private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;
    private int buildingID;
    public static PlacementSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StopPlacement();
        gridData = new GridData();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualiazation.SetActive(true);
        buildingID = ID;
        buildingState = new PlacementState(ID, grid, previewSystem, objectsDatabase, gridData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualiazation.SetActive(true);
        buildingState = new RemovingState(grid, previewSystem, gridData, objectPlacer);
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

        if(ResourceManager.instance.CanAfford(objectsDatabase.objectsData[buildingID].Price))
        {
            buildingState.OnAction(gridPosition);
        }
    }

    public void StopPlacement()
    {
        if(buildingState == null)
        {
            return;
        }
        gridVisualiazation.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;        
        buildingState = null;
    }
    public void ClearGrid()
    {
        objectPlacer.RemoveAllObjects();
        gridData.ClearAllData();
    }

    public void RemoveObjectAtGridPosition(Vector3Int gridPosition)
    {
        if (!gridData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            int objectIndex = gridData.GetRepresentationIndex(gridPosition);
            if (objectIndex != -1)
            {
                gridData.RemoveObjectAt(gridPosition);
                objectPlacer.RemoveObjectAt(objectIndex);
            }
        }
    }

    void Update()
    {
        if(buildingState == null)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(gridPosition != lastDetectedPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }

        if(gridData.HasAnyObject())
        {
            GameManager.instance.startable = true;
        }
        else
        {
            GameManager.instance.startable = false;
        }
    }
}
