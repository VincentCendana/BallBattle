using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Energy Bar Components")]
    public Slider[] energySliders;                  // slider to display the player's current energy
    private Image[] sliderFills;                    // fill image for each energy slider

    [Header("Player Parameters")]
    public float maxEnergy;                         // player's maximum amount of energy
    public float energyRecoveryRate;                // amount of energy recovered per second

    [Header("Current Status")]
    public float currentEnergy = 0.0f;              // player's current energy point

    // reset current energy points
    public void ResetEnergy()
    {
        currentEnergy = 0.0f;

        // create slider fill array
        sliderFills = new Image[energySliders.Length];

        // initialize energy sliders
        for (int i = 0; i < energySliders.Length; i++)
        {
            // set initial energy bar values to 0
            energySliders[i].value = 0.0f;

            // assign energy slider fill images
            sliderFills[i] = energySliders[i].fillRect.GetComponent<Image>();
        }
    }

    // regenerate energy if it is less than the maximum value
    public void FillEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += GameManager.instance.currentEnergyRate * Time.deltaTime;
        }
        else
        {
            currentEnergy = maxEnergy;
        }

        // update energy UI
        UpdateEnergyUI();
    }

    // update energy sliders to display the current energy status and usable energy points
    private void UpdateEnergyUI()
    {
        // update sliders
        for (int i = 0; i < energySliders.Length; i++)
        {
            float sliderValue = currentEnergy - (1.0f * i);
            energySliders[i].value = sliderValue;

            // highlight slider to show usable energy point
            Color fillColor = sliderFills[i].color;

            if (sliderValue < 1.0f)
            {
                fillColor.a = 0.4f;
            }
            else
            {
                fillColor.a = 1.0f;
            }

            sliderFills[i].color = fillColor;
        }
    }

    // check if player has enough energy to spawn a soldier
    public bool ValidSpawn(bool isAttacker)
    {
        // get spawn cost
        float cost = 0.0f;

        if (isAttacker)
        {
            cost = GameManager.instance.attackerCost;
        }
        else
        {
            cost = GameManager.instance.defenderCost;
        }

        // determine if the current energy is enough to spawn a soldier
        return (currentEnergy >= cost);
    }

    // reduce energy to spawn a soldier
    public void SpawnCost(bool isAttacker)
    {
        // get spawn cost
        float cost = 0.0f;

        if (isAttacker)
        {
            cost = GameManager.instance.attackerCost;
        }
        else
        {
            cost = GameManager.instance.defenderCost;
        }

        // reduce current energy by the spawn cost
        currentEnergy -= cost;
    }
}
