
using UnityEngine;
using System.Collections;

public class TrapMenuController : MonoBehaviour
{

    public GameObject blizzard;
    [SerializeField]    private int healthPoints;
    private GameManager gm;
    private CharacterController player;
    private Transform t;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        t = player.transform;

    }

    public void Clicked(string objectClicked)
    {
        if (objectClicked == "Blizzard")
        {
            // Make sure player affords buying a blizzard
            if(gm.Score - 200 >= 0)
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
            if (gm.Score - 100 >= 0)
            {
                player.addHealthPoints(healthPoints);
                gm.deductFromScore(100, transform);
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
            if (gm.Score - 50 >= 0)
            {
                player.BuyMana();
                gm.deductFromScore(50, transform);
            }
            else
            {
                //notify
            }
        }
    }
}