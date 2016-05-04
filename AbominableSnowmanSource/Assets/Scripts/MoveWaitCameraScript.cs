using UnityEngine;

public class MoveWaitCameraScript : MonoBehaviour {

    private Camera otherCamera;

	void Start () {
        otherCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	void Update () {
	}

    void OnEnable() {
        //transform.position = otherCamera.gameObject.transform.position;
        transform.LookAt(GameObject.FindGameObjectWithTag("Environment").transform);
    }
}
