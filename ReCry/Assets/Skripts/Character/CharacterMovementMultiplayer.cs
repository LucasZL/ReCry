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
    public float MoveSpeed;
    public float RunSpeed = 25;
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
    private bool gamestarted = false;

    //JetPack
    private Image fuel;
    private float jetpacktank = 1;
    public float JetPackSpeed = 25f;
    private bool fuelIsEmpty = false;
    private bool moveForwards = false;
    private bool changeFuel = true;
    private bool addFuel = true;
    private float maxJetPackJump = 0.3f;
    private float maxJetPackDirection = 0.1f;
    private float maxFuel = 1;
    private int jetpackchange;

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
            RunSpeed = 25;
            this.camera = this.transform.Find("Camera").GetComponent<Camera>();
            this.jump = GetComponent<JumpDetection>();
            this.fuel = GameObject.FindWithTag("Fuel").GetComponent<Image>();
            this.fuel.fillAmount = jetpacktank;
        }
        else
        {
            this.transform.Find("Camera").gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (ph.isMine)
        {
            CheckIfCharacterMoved();
            JetPackJump();
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

    void CheckIfCharacterMoved()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            MoveCharacter();
        }
        else
        {
            isWalking = false;
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
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && jump.isGrounded)
        {
            if (jetpacktank >= maxJetPackDirection)
            {
                ChangeFuel(20, 1);
                isWalking = false;
                isRunning = true;
                this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
                this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * RunSpeed;
                transform.Translate(horizontal, 0, vertical);
                
            }
            
        }
        else
        {
            isRunning = false;
            isWalking = true;
        }
    }

    void LookAround()
    {
        this.mouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        this.mouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        if (this.mouseY >= lookDown)
        {
            this.mouseY = lookDown;
        }
        else if (this.mouseY <= LookUp)
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

    private void JetPackJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jump.isGrounded && fuelIsEmpty && Utility.isInGame)
        {
            if (jetpacktank >= maxJetPackJump)
            {
                if (isWalking)
                {
                    ChangeJetpackFuel(0.3f, 0);
                    this.rigid.AddRelativeForce(new Vector3(0, this.transform.position.y + JumpHeight, MoveSpeed * this.rigid.mass), ForceMode.Impulse);
                }
                if (isRunning)
                {
                    ChangeJetpackFuel(0.3f, 0);
                    this.rigid.AddRelativeForce(new Vector3(0, this.transform.position.y + JumpHeight, RunSpeed * this.rigid.mass), ForceMode.Impulse);
                }
                if (!isWalking)
                {
                    ChangeJetpackFuel(0.3f, 0);
                    this.rigid.AddRelativeForce(new Vector3(0, this.transform.position.y + JumpHeight, 0), ForceMode.Impulse);
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
        if (Input.GetKey(KeyCode.LeftShift) && !jump.isGrounded && fuelIsEmpty && Utility.isInGame)
        {
            if (jetpacktank >= maxJetPackDirection)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    moveForwards = true;
                    ChangeJetpackFuel(0.1f, 1);
                    this.rigid.AddRelativeForce(new Vector3(0, 0, 125 / (this.rigid.mass / 4)));
                    
                    
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveForwards = true;
                    ChangeJetpackFuel(0.1f, 1);
                    this.rigid.AddRelativeForce(new Vector3(0, 0, -125 / (this.rigid.mass / 4)));
                    
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveForwards = true;
                    ChangeJetpackFuel(0.1f, 1);
                    this.rigid.AddRelativeForce(new Vector3(-125 / (this.rigid.mass / 4), 0, 0));
                    
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveForwards = true;
                    ChangeJetpackFuel(0.1f, 1);
                    this.rigid.AddRelativeForce(new Vector3(125 / (this.rigid.mass / 4), 0, 0));
                    
                }
                if (Input.GetKey(KeyCode.LeftShift) && !moveForwards)
                {
                    ChangeJetpackFuel(0.1f, 1);
                    this.rigid.AddRelativeForce(new Vector3(0, 250, 0));
                }
                else
                {
                    moveForwards = false;
                }

            }
            else
            {
                jetpacktank = 0;
            }

        }
        else
        {
            fuelIsEmpty = true;
        }
    }

    private void FillUpJetPackFuel()
    {
        if (jetpacktank < maxFuel && jump.isGrounded && !isRunning && Utility.isInGame)
        {
            if (addFuel)
            {
                StartCoroutine(AddFuel(0.1f, 0.5f));
            }
        }
    }

    private void ChangeJetpackFuel(float change, int seconds)
    {
        if (jetpacktank >= change)
        {
            if (changeFuel)
            {
                StartCoroutine(ChangeFuel(change,seconds));
            }
        }
        else
        {
            fuelIsEmpty = true;
        }

    }

    IEnumerator ChangeFuel(float change,int seconds)
    {
        changeFuel = false;
        jetpacktank -= change;
        this.fuel.fillAmount = jetpacktank;
        yield return new WaitForSeconds(seconds);
        changeFuel = true;
    }


    IEnumerator AddFuel(float add, float seconds)
    {
        addFuel = false;
        jetpacktank += add;
        this.fuel.fillAmount = jetpacktank;
        yield return new WaitForSeconds(seconds);
        addFuel = true;
    }
}
