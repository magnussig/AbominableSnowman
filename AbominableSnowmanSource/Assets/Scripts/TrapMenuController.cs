
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    EventTrigger healthEventTrigger;


    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        t = player.transform;
        healthButton = GameObject.Find("health").GetComponent<Button>();
        manaButton = GameObject.Find("mana").GetComponent<Button>();
        blizzardButton = GameObject.Find("cloud").GetComponent<Button>();
        healthCost = 100;
        manaCost = 100;
        blizzardCost = 200;
        //might want to use this later
        healthEventTrigger = GameObject.Find("health").GetComponent<EventTrigger>();
    }

    void Update()
    {
        if(gm.Score < healthCost)
        {
            healthButton.interactable = false;
            manaButton.interactable = false;
        }
        else
        {
            healthButton.interactable = true;
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
            else
            {
                //notify:
                //FloatingTextController.CreateFloatingText(0, t, true);
            }
        }
        else if (objectClicked == "Health")
        {
            // Make sure player affords buying health
            if (gm.Score - healthCost >= 0)
            {
                player.addHealthPoints(healthPoints);
                gm.deductFromScore(healthCost, transform);
            }
            else
            {
                //notify:
                //FloatingTextController.CreateFloatingText(0, t, true);
            }            
        }
        else if(objectClicked == "Mana")
        { 
            // Make sure player affords buying health
            if (gm.Score - manaCost >= 0)
            {
                player.BuyMana(manaPoints);
                gm.deductFromScore(manaCost, transform);
            }
            else
            {
                //notify
            }
        }
    }
}