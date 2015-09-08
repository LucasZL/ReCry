using UnityEngine;
using System.Collections;

public class CharacterMovementMultiplayer : Photon.MonoBehaviour {

    PhotonView ph;
    public float MoveSpeed;
    public float RunSpeed;
    public float JumpSpeed = 20;
    public float Gravity = 20;
    private bool isWalking = true;
    private bool isRunning = false;
    private float horizontal;
    private float vertical;
    public float speed;
    Rigidbody rigid;
	// Use this for initialization
	void Start ()
    {
        ph = PhotonView.Get(transform.parent.gameObject);
        this.rigid = this.transform.parent.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ph.isMine)
        {
            MoveCharacter();
            Run();
            Jump();
        }
	}

    void MoveCharacter()
    {
        if (isWalking)
        {
            this.speed = this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            this.speed = this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

            transform.Translate(horizontal, 0, vertical);
        }
        
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !isRunning)
        {
            isWalking = false;
            this.speed = this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            this.speed = this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * RunSpeed;
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
            
        }
    }
}
