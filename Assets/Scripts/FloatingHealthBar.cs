using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Camera mainCamera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth/maxHealth;
    }
    
    void Update()
    {
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.position + offset;
    }
}
