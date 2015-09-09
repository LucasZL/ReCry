using UnityEngine;
using System.Collections;

public class CharacterMovementMultiplayer : Photon.MonoBehaviour {

    PhotonView ph;
    public float MoveSpeed = 5f;
    public float RunSpeed = 15f;
    public float MouseSensitivity = 5f;
    public float JumpHeight = 7.5f;
    public float Gravity = 20f;
    private bool isWalking = true;
    private bool isRunning = false;
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    public float speed;
    private Camera camera;
    Rigidbody rigid;

    // Use this for initialization
    void Start ()
    {
        ph = PhotonView.Get(this.transform.gameObject);
        if (ph.isMine)
        {
            this.rigid = GetComponent<Rigidbody>();
            this.camera = Camera.main;
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ph.isMine)
        {
            MoveCharacter();
            Run();
            Jump();
            LookAround();
        }
	}

    void MoveCharacter()
    {
        if (isWalking)
        {
            this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

            transform.Translate(horizontal, 0, vertical);
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !isRunning)
        {
            isWalking = false;
            this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * RunSpeed;
            transform.Translate(horizontal, 0, vertical);
        }
        else
        {
            isWalking = true;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rigid.AddForce(new Vector3(0, JumpHeight, 0), ForceMode.Impulse);
        }
    }

    void LookAround()
    {
        this.mouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        this.mouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        this.camera.transform.eulerAngles = new Vector3(this.mouseY, this.mouseX);
    }
}
