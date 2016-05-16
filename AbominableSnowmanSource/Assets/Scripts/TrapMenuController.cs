
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TrapMenuController : MonoBehaviour
{

    public GameObject blizzard;
    [SerializeField]    public int healthPoints;
    [SerializeField]    public int manaPoints;
    private GameManager gm;
    private CharacterController player;
    private Transform t;
    Button healthButton;
    Button manaButton;
    Button blizzardButton;
    [SerializeField] public int healthCost;
    [SerializeField] public int manaCost;
    [SerializeField] public int blizzardCost;
    [SerializeField] public double healthPercentage;
    [SerializeField] public double manaPercentage;
    [SerializeField] public double blizzardPercentage;
    EventTrigger healthEventTrigger;
    int waveCounter;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        t = player.transform;
        healthButton = GameObject.Find("health").GetComponent<Button>();
        manaButton = GameObject.Find("mana").GetComponent<Button>();
        blizzardButton = GameObject.Find("cloud").GetComponent<Button>();
        waveCounter = 1;
        blizzardPercentage = 0.01 * blizzardPercentage;
        manaPercentage = 0.01 * manaPercentage;
        healthPercentage = 0.01 * healthPercentage;
    }

    void Update()
    {
        Debug.Log("waaaves: " + waveCounter + "waveees " + gm.waveCount);
        // only call CalculateCost once every wave
        if (waveCounter == gm.waveCount)
        {
            CalculateCost();
            waveCounter++;
            Debug.Log("waaaves: " + waveCounter);
        }
        if (gm.Score < healthCost)
        {
            healthButton.interactable = false;
        }
        else
        {
            healthButton.interactable = true;
        }

        if(gm.Score < manaCost)
        {
            manaButton.interactable = false;
        }
        else
        {
            manaButton.interactable = true;
        }

        if(gm.Score < blizzardCost)
        {
            blizzardButton.interactable = false;
        }
        else
        {
            blizzardButton.interactable = true;
        }
    }

    private void CalculateCost()
    {
        int newManaCost = (int)Math.Floor(manaPercentage * gm.Score);
        Debug.Log("newManacost: " + newManaCost);
        if(newManaCost > manaCost)
        {
            manaCost = newManaCost;
        }
        int newHealthCost = (int)Math.Floor(healthPercentage * gm.Score);
        Debug.Log("newHealthCost: " + newHealthCost);
        if (newHealthCost > healthCost)
        {
            healthCost = newHealthCost;
        }
        int newBlizzardCost = (int)Math.Floor(blizzardPercentage * gm.Score);
        Debug.Log("newBlizzardCost: " + newBlizzardCost);
        if (newBlizzardCost > blizzardCost)
        {
            blizzardCost = newBlizzardCost;
        }
    }


    public void Clicked(string objectClicked)
    {
        if (objectClicked == "Blizzard")
        {
            // Make sure player affords buying a blizzard
            if(gm.Score - blizzardCost >= 0)
            {
                //deduct from score inside TrapPlacementScript to get mouse position
                Instantiate(blizzard);
            }
        }
        else if (objectClicked == "Health")
        {
            // Make sure player affords buying health
            if (gm.Score - healthCost >= 0)
            {
                player.addHealthPoints(healthPoints);
                gm.deductFromScore(healthCost, transform);
                FloatingTextController.CreateFloatingText(-healthCost, player.transform.position);
            }
        }
        else if(objectClicked == "Mana")
        { 
            // Make sure player affords buying health
            if (gm.Score - manaCost >= 0)
            {
                player.BuyMana(manaPoints);
                gm.deductFromScore(manaCost, transform);
                FloatingTextController.CreateFloatingText(-manaCost, player.transform.position);
            }
        }
    }
}