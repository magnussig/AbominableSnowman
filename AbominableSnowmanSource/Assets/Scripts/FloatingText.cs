using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingText : MonoBehaviour {

    private Animator anim;
    private Text popupText;

	void OnEnable () {
        anim = GetComponentInChildren<Animator>();

        AnimatorClipInfo clipInfo = anim.GetCurrentAnimatorClipInfo(0)[0];
        Destroy(gameObject, clipInfo.clip.length);
        popupText = anim.GetComponent<Text>();
	}

    public void SetText(string text) {
        popupText.text = text;
    }
}
