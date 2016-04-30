using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public static bool MovingRight;

    public float speed; // walk speed
    public float RunSpeed; // run speed multiplier
    public float jumpSpeed;
    public float gravity;
    public GameObject PlayerObj;
    private Vector3 moveDirection = Vector3.zero;
    public Animator anim;
    public GameObject InstructionsCanvas;
    public int JumpDelay; // the delay before character starts jumping up
    private int JumpCounter;
    public int SelChar;

	// Use this for initialization
	void Start () {
        MovingRight =false;
        StartCoroutine(StartLoader());
	}

    IEnumerator StartLoader() // a delay is needed to ensure that PlayerScript3D has tagged all of the children
    {
        yield return new WaitForSeconds(0.2f);
        ChangeChar(SelChar);
    }
	
	// Update is called once per frame
    void Update()
    {
        // make instructions disappear if you press a key
        if (Input.anyKey && InstructionsCanvas.activeSelf)
        {
            InstructionsCanvas.SetActive(false);
        }

        if (MovingRight == true)
        {
            if (Input.GetAxis("Horizontal")<0)
            {
                PlayerObj.transform.rotation = Quaternion.Euler(-90, 0, 0);
                MovingRight = false;
            }
        }

        if (MovingRight == false)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                PlayerObj.transform.rotation = Quaternion.Euler(-90, 180, 0);
                MovingRight = true;
            }
        }


        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;

            if (Input.GetAxis("Horizontal") != 0.0f)
            { anim.SetFloat("MoveSpeed", speed); }
            else { anim.SetFloat("MoveSpeed", 0.0f); }

            // if running then multiply by runspeed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection *= RunSpeed;
                anim.SetFloat("MoveSpeed", speed*RunSpeed);
            }
            //Jumping
            if (Input.GetButton("Jump"))
            { 
                JumpCounter = 1;
                anim.SetBool("JumpingUp", true);
            }
        }

        if (JumpCounter > 0) // if you started to jump then increase JumpCounter
            JumpCounter += 1;
    

        if (JumpCounter == JumpDelay) // if you have been jumping and delay is now over, start moving up
        {       
            moveDirection.y = jumpSpeed;   
        }

        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;

        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);

        anim.SetFloat("JumpSpeed", moveDirection.y); // set jumpspeed on anim controller


        if (moveDirection.y < 0 && JumpCounter>JumpDelay)
        {
            anim.SetBool("JumpingUp", false);
            JumpCounter = 0;
        }

        // player bounds
        if (transform.position.x < -922)
        {
            transform.position = new Vector3(-922, transform.position.y, transform.position.z);
        }
        if (transform.position.x >72)
        {
            transform.position = new Vector3(72, transform.position.y, transform.position.z);
        }

        // change character
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Goat
        {
            SelChar = 1;
            ChangeChar(SelChar);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Blue Buck
        {
            SelChar = 2;
            ChangeChar(SelChar);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Yeti
        {
            SelChar = 3;
            ChangeChar(SelChar);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) // Jersey Devil
        {
            SelChar = 4;
            ChangeChar(SelChar);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) // Tasmanian Tiger
        {
            SelChar = 5;
            ChangeChar(SelChar);
        }

    }

    private void ChangeChar(int CharNum)
    {
        HideObject("Goat");
        HideObject("BlueBuck");
        HideObject("Yeti");
        HideObject("JerseyDevil");
        HideObject("TasmanianTiger");

        if(CharNum==1)
        {
            ShowObject("Goat");
            anim = GameObject.Find("Goat").GetComponent<Animator>();
        }
        if (CharNum == 2)
        {
            ShowObject("BlueBuck");
            anim = GameObject.Find("BlueBuck").GetComponent<Animator>();
        }
        if (CharNum == 3)
        {
            ShowObject("Yeti");
            anim = GameObject.Find("Yeti").GetComponent<Animator>();
        }
        if (CharNum == 4)
        {
            ShowObject("JerseyDevil");
            anim = GameObject.Find("JerseyDevil").GetComponent<Animator>();
        }
        if (CharNum == 5)
        {
            ShowObject("TasmanianTiger");
            anim = GameObject.Find("TasmanianTiger").GetComponent<Animator>();
        }

    }

    private void HideObject(string ObjName)
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach(var r in renderers) 
        {
            if (r.CompareTag(ObjName)) 
            {
                r.enabled = false;
            }
        }
    }

    private void ShowObject(string ObjName)
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            if (r.CompareTag(ObjName))
            {
                r.enabled = true;
            }
        }
    }


}
