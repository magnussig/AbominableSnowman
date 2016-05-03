using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    void Awake () {
        target = GameObject.FindWithTag("Player");
	}

	void Update () {
        transform.position = target.transform.position + offset;
	}
}
