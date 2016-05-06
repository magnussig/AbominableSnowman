using UnityEngine;

public class FollowTarget : MonoBehaviour {

    private GameObject target;
    [SerializeField] private Vector3 offset;

    void Awake () {
        target = GameObject.FindWithTag("Player");
	}

	void Update () {
        transform.position = target.transform.position + offset;
	}
}
