using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public PlacementSystem placementSystem;

    [SerializeField]
    public Button[] buttonsToDisable;

    public bool isGameStarted = false;

    void Awake()
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

    public void PrepareGame()
    {
        isGameStarted = false;
        placementSystem.enabled = true;
        foreach (var button in buttonsToDisable)
        {
            button.interactable = true;
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        placementSystem.StopPlacement();
        placementSystem.enabled = false;
        foreach (var button in buttonsToDisable)
        {
            button.interactable = false;
        }
    }
}