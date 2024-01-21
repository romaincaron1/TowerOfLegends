using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public GameObject victoryPanel;

    [SerializeField]
    public PlacementSystem placementSystem;

    [SerializeField]
    public Button[] buttonsToDisable;

    public bool isGameStarted = false;

    [SerializeField]
    private TMP_Text coinsText;
    
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private TMP_Dropdown difficultyDropdown;

    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private float spawnDuration = 30.0f;

    private float spawnRate = 2.0f;

    public bool startable = false;

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
        placementSystem.ClearGrid();
        placementSystem.enabled = true;
        foreach (var button in buttonsToDisable)
        {
            button.interactable = true;
        }
        victoryPanel.SetActive(false);
        ResourceManager.instance.ResetCoinsToInitialValue();
    }

    public void StartGame()
    {
        if(!startable)
        {
            return;
        }
        isGameStarted = true;
        AdjustSpawnRateBasedOnDifficulty();
        StartCoroutine(SpawnEnemies());
        placementSystem.StopPlacement();
        placementSystem.enabled = false;
        foreach (var button in buttonsToDisable)
        {
            button.interactable = false;
        }
    }

    private void AdjustSpawnRateBasedOnDifficulty()
    {
        switch (difficultyDropdown.value)
        {
            case 0:
                spawnRate = 3.0f;
                break;
            case 1:
                spawnRate = 2.0f;
                break;
            case 2:
                spawnRate = 1.0f;
                break;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        float elapsedTime = 0;

        while (isGameStarted && elapsedTime < spawnDuration)
        {
            yield return new WaitForSeconds(spawnRate);
            elapsedTime += spawnRate;

            // Sélectionner un point de spawn aléatoire
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void UpdateCoinsDisplay(int coins)
    {
        coinsText.text = coins.ToString();
    }

    public void EndGame()
    {
        victoryPanel.SetActive(true);
    }
}