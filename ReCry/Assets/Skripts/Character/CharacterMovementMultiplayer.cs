//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst, Lucas Zacharias-Langhans on 14.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CharacterMovementMultiplayer : Photon.MonoBehaviour
{


    //Used for Photon
    PhotonView ph;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;

    //BasicStats
    public float MoveSpeed = 5f;
    public float RunSpeed = 15f;
    public float MouseSensitivity = 5f;
    public float JumpHeight = 12.5f;
    public int LookUp = -50;
    public int lookDown = 50;
    private bool isWalking = true;
    public bool isRunning = false;
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    public float speed;

    //JetPack
    private Text fuel;
    private int jetpacktank = 100;
    public float JetPackSpeed = 25f;
    private bool fuelIsEmpty = false;

    //Camera Movement and JumpController
    private Camera camera;
    private JumpDetection jump;

    Rigidbody rigid;

    void Start()
    {
        ph = PhotonView.Get(this.transform.gameObject);
        this.rigid = GetComponent<Rigidbody>();

        if (ph.isMine)
        {
            this.camera = this.transform.Find("Camera").GetComponent<Camera>();
            this.jump = GetComponent<JumpDetection>();
            this.fuel = GameObject.FindWithTag("Fuel").GetComponent<Text>() as Text;
            this.fuel.text = jetpacktank.ToString();
        }
        else
        {
            this.transform.Find("Camera").gameObject.active = false;
        }
    }

    void Update()
    {
        if (ph.isMine)
        {
            MoveCharacter();
            Jetpack();
            Run();
            LookAround();
        }
        else
        {
            //PhotonNetwork
            fraction = fraction + Time.deltaTime * 9;
            transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);
        }
    }

    void FixedUpdate()
    {
        if (ph.isMine)
        {
            JetPackForward();
            FillUpJetPackFuel();
        }

    }

    void MoveCharacter()
    {
        if (isWalking && jump.isGrounded)
        {
            this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

            transform.Translate(horizontal, 0, vertical);
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && jump.isGrounded && jetpacktank > 0)
        {
            isWalking = false;
            isRunning = true;
            this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * RunSpeed;
            transform.Translate(horizontal, 0, vertical);
            jetpacktank--;
            this.fuel.text = jetpacktank.ToString();
        }
        else
        {
            isWalking = true;
        }
    }

    void LookAround()
    {
        this.mouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        this.mouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

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

    private void Jetpack()
    {
        Debug.Log(jump.isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && jump.isGrounded)
        {
            if (jetpacktank > 0)
            {
                if (isWalking)
                {
                    this.rigid.AddRelativeForce(new Vector3(0, this.transform.position.y + JumpHeight, MoveSpeed * this.rigid.mass), ForceMode.Impulse);
                    jetpacktank--;
                    this.fuel.text = jetpacktank.ToString();
                }
                if (isRunning)
                {
                    this.rigid.AddRelativeForce(new Vector3(0, this.transform.position.y + JumpHeight, RunSpeed * this.rigid.mass), ForceMode.Impulse);
                    jetpacktank--;
                    this.fuel.text = jetpacktank.ToString();
                }
               
            }
            else
            {
                fuelIsEmpty = true;
            }

        }
    }

    private void JetPackForward()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !jump.isGrounded && fuelIsEmpty)
        {
            if (jetpacktank > 0)
            {
                this.rigid.AddForce(new Vector3(0, 0, 0));
                jetpacktank--;
                this.fuel.text = jetpacktank.ToString();
            }

        }
        else
        {
            fuelIsEmpty = true;
        }
    }

    private void FillUpJetPackFuel()
    {
        if (jetpacktank < 100 && jump.isGrounded && !isRunning)
        {
            jetpacktank++;
            this.fuel.text = jetpacktank.ToString();
        }
    }
}
