using UnityEngine;
using System.Collections;

public class TrapMenuController : MonoBehaviour {
    
    public GameObject blizzard;

    public void Clicked()
    {
        Instantiate(blizzard);
    }
}
