using UnityEngine;
using System.Collections;

public class CharacterMovementMultiplayer : Photon.MonoBehaviour {

    PhotonView ph;

	public Vector3 realPosition = Vector3.zero;
	public Vector3 positionAtLastPacket = Vector3.zero;
	public double currentTime = 0.0;
	public double currentPacketTime = 0.0;
	public double lastPacketTime = 0.0;
	public double timeToReachGoal = 0.0;

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
			timeToReachGoal = currentPacketTime - lastPacketTime;
			currentTime += Time.deltaTime;
			transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
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

        if(this.mouseY >= 50)
        {
            this.mouseY = 50;
        }
        else if(this.mouseY <= -90)
        {
            this.mouseY = -90;
        }
        this.camera.transform.eulerAngles = new Vector3(this.mouseY, this.mouseX);
        this.transform.eulerAngles = new Vector3(0, this.mouseX);
    }

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext((Vector3)transform.position);
		}
		else
		{
			currentTime = 0.0;
			positionAtLastPacket = transform.position;
			realPosition = (Vector3)stream.ReceiveNext();
			lastPacketTime = currentPacketTime;
			currentPacketTime = info.timestamp;
		}
	}
}
