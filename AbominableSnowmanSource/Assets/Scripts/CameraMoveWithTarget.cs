using UnityEngine;
using System.Collections;

public class CameraMoveWithTarget : MonoBehaviour {

    private GameObject target;

    [SerializeField] private Vector3 offset;

	void Start () {
        target = GameObject.FindWithTag("Player");    	
	}

	void Update () {
        transform.position = new Vector3
                                (
                                    target.transform.position.x, 
                                    target.transform.position.y,
                                    transform.position.z
                                ) + offset;
	}
}
