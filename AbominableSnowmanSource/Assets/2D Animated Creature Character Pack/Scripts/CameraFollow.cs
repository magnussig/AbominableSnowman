using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public GameObject Target;
    public float DistanceLeft;
    public float DistanceRight;
    public float DistanceUp;
    public float DistanceDown;
    private float CameraDestX;
    private float CameraDestY;
	// Use this for initialization
	void Start () {
        CameraDestX = transform.position.x;
        CameraDestY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        // escape to quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        // if player is moving left
        if (PlayerScript.MovingRight == false)
        {

            if (Target.transform.position.x < transform.position.x + DistanceLeft)
            {
                CameraDestX = Target.transform.position.x - DistanceLeft;
                //Debug.Log("PanningLeft");
            }
        }

        if (PlayerScript.MovingRight == true)
        {
            if (Target.transform.position.x > transform.position.x - DistanceRight)
            {
                CameraDestX = Target.transform.position.x - DistanceRight;
                //if (CameraDestX > 493.9f) { CameraDestX = 493.9f; }
                
                //Debug.Log("PanningRight");
            }
        }
        
        // camera bounds
        if (CameraDestX > -45.0f) { CameraDestX = -45.0f; }
        if (CameraDestX < -810.0f) { CameraDestX = -810.0f; }


        // camera height
       // if (Target.transform.position.y < transform.position.y + DistanceDown)
        {
            CameraDestY = Target.transform.position.y - DistanceDown;
        //    Debug.Log("PanningDown");
        }

        // position camera
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, CameraDestX, Time.deltaTime), Mathf.Lerp(transform.position.y, CameraDestY, Time.deltaTime), transform.position.z);

	}
}
