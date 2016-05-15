using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MuteController : MonoBehaviour {

    bool isMute;
    Sprite mutedLogo;
    Sprite unmutedLogo;

    // Use this for initialization
    void Start () {
        isMute = false;
        mutedLogo = Resources.Load<Sprite>("muted");
        unmutedLogo = Resources.Load<Sprite>("unMuted");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Mute()
    {
        isMute = !isMute;
        //mute sound
        AudioListener.volume = isMute ? 0 : 1;
        //change image
        gameObject.GetComponent<Image>().sprite = isMute ? mutedLogo : unmutedLogo;
    }
}
