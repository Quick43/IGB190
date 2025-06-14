using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Handles all of the functionality for displaying the unit's UI
public class UnitUI : MonoBehaviour
{
    public RectTransform container;
    public Image healthBar;
    public Vector3 offset = new Vector3(0, 2, 0);
    private IDamageable trackedDamageable;
    private Transform trackedTransform;
    public Image experienceBar;
    

   
    // Cache initial references to the unit and damageable interface
    private void Awake()
    {
        trackedDamageable = GetComponentInParent<IDamageable>();
        trackedTransform = transform.parent;  
    }

    // Update the position and display of the UI every frame
    private void LateUpdate()
    {
        // Move the health bar to the correct screen position
        Vector3 world = trackedTransform.position + offset;
        container.anchoredPosition = Camera.main.WorldToScreenPoint(world);

        // Update the amount of the red health bar which is visible
        healthBar.fillAmount = trackedDamageable.GetCurrentHealthPercent();
        if (experienceBar != null)
        {
            float experiencePercentage = GameObject.Find("Player").GetComponent<Player>().experiencePercent;
            experienceBar.fillAmount = experiencePercentage;
        }
    }
}
