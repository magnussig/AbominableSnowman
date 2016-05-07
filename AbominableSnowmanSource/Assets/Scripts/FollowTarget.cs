using UnityEngine;

public class FollowTarget : MonoBehaviour {

    private GameObject target;
    private Vector3 offset;


	void Update () {
        transform.position = target.transform.position + offset;
	}

    public void AssignTarget(GameObject target, Vector3 offset) {
        this.target = target;
        this.offset = offset;
    } 
}
