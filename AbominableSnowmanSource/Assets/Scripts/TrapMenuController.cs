using UnityEngine;
using System.Collections;

public class TrapMenuController : MonoBehaviour {
    
    public GameObject blizzard;
    private GameObject blizzardInstance;

    public void Clicked()
    {
        blizzardInstance = Instantiate(blizzard);
        blizzard.SetActive(true);

        /*String
        if (objectClicked.Equals("Blizzard"))
        {
            GameObject.Find("Blizzard").SetActive(true);
        }
        else
        {
            Debug.Log(objectClicked);
        }*/
    }

    private void OnDisable()
    {
        if (blizzardInstance == null) return;
        //TODO: fix error
        TrapPlacementScript tp = blizzardInstance.GetComponent<TrapPlacementScript>();
        if (false)
        {
            Destroy(tp);
        }
    }

    private void OnEnable()
    {
        //tékka á null
        blizzardInstance = null;
    }
}
