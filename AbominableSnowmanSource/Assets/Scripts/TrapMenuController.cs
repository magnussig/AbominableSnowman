
using UnityEngine;
using System.Collections;

public class TrapMenuController : MonoBehaviour
{

    public GameObject blizzard;
    [SerializeField]
    private int healthPoints;

    public void Clicked(string objectClicked)
    {
        Debug.Log("inside clicked bra " + objectClicked);
        CharacterController player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        if (objectClicked == "Blizzard")
        {
            Instantiate(blizzard);
        }
        else if (objectClicked == "Health")
        {
            player.addHealthPoints(healthPoints);
        }
    }
}