using UnityEngine;
using System.Collections;

public class TrapMenuController : MonoBehaviour {
    
    public GameObject blizzard;
    private TrapPlacementScript tp;

    public void Clicked()
    {
        tp = Instantiate(blizzard).GetComponent<TrapPlacementScript>();
        
    }
}
