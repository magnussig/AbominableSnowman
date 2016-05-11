using UnityEngine;
using System.Collections;


public class UIManager : MonoBehaviour {

    private Animator anim;

	void Start () {
        anim = GetComponentInChildren<Animator>();
    }

	void Update () {
        anim.SetBool("In", true);
	}
}
