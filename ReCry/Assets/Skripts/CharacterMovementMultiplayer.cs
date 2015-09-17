using UnityEngine;
using System.Collections;

public class CharacterMovementMultiplayer : Photon.MonoBehaviour {

    PhotonView ph;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;

    public float MoveSpeed = 5f;
    public float RunSpeed = 15f;
    public float MouseSensitivity = 5f;
    public float JumpHeight = 7.5f;
    public float Gravity = 20f;
    public int LookUp = -50;
    public int lookDown = 50;
    private bool isWalking = true;
    private bool isRunning = false;
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    public float speed;
    private Camera camera;
    private JumpDetection jump;
    Rigidbody rigid;

    void Start ()
    {
        ph = PhotonView.Get(this.transform.gameObject);
        this.rigid = GetComponent<Rigidbody>();

        if (ph.isMine)
        {
            this.camera = this.transform.Find("Camera").GetComponent<Camera>();
            this.jump = GetComponent<JumpDetection>();
        }
        else
        {
            this.transform.Find("Camera").gameObject.active = false;
        }
	}
	
	void Update ()
    {
        if (ph.isMine) {
			MoveCharacter ();
			Run ();
			Jump ();
			LookAround ();
		} 
		else
		{
            fraction = fraction + Time.deltaTime * 9 ;
            transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);    // set our pos between A and B
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
            if (jump.isGrounded)
            {
                this.rigid.AddForce(new Vector3(0, JumpHeight, 0), ForceMode.Impulse);
            }
        }
    }

    void LookAround()
    {
        this.mouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        this.mouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        if(this.mouseY >= lookDown)
        {
            this.mouseY = lookDown;
        }
        else if(this.mouseY <= LookUp)
        {
            this.mouseY = LookUp;
        }
        this.camera.transform.eulerAngles = new Vector3(this.mouseY, this.mouseX);
        this.transform.eulerAngles = new Vector3(0, this.mouseX);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
            onUpdatePos = transform.position;  // we interpolate from here to latestCorrectPos
            fraction = 0;                           // reset the fraction we alreay moved. see Update()

            transform.localRotation = rot;          // this sample doesn't smooth rotation
        }
    }
}
