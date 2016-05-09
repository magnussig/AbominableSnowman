using UnityEngine;
using System.Collections;

public class TrapMenuController : MonoBehaviour {
    
    public GameObject blizzard;
    public void Clicked()
    {
        blizzard.SetActive(true);
        Debug.Log("haaalkdsjflkajdsflkajslkfajlkdsa");
        GameObject.Find("Enemy");
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
}
