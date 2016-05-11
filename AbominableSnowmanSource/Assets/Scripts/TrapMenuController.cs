
using UnityEngine;
using System.Collections;

public class TrapMenuController : MonoBehaviour
{

    public GameObject blizzard;
    [SerializeField]    private int healthPoints;
    private GameManager gm;
    private GameObject player;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void Clicked(string objectClicked)
    {
        Debug.Log("inside clicked bra " + objectClicked);
        CharacterController player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        if (objectClicked == "Blizzard")
        {
            Instantiate(blizzard);
            //deduct from score inside TrapPlacementScript to get mouse position
        }
        else if (objectClicked == "Health")
        {
            player.addHealthPoints(healthPoints);

            gm.deductFromScore(100, transform);
        }
    }
}