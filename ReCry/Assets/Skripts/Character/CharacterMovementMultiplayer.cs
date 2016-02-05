//
//  CharacterMovementMultiplayer.cs
//  ReCry
//  
//  Created by Kevin Holst, Lucas Zacharias-Langhans(LookAround) on 14.09.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CharacterMovementMultiplayer : Photon.MonoBehaviour
{
    //Used for Photon
    PhotonView ph;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;

    //BasicStats
    public float MoveSpeed;
    public float RunSpeed;
    public float MaxHeight;
    public float JumpHeight;
    public int JetPackDirectionSpeed;
    public int JetPackHeight;
    public int LookUp = -50;
    public int lookDown = 50;
    public bool isWalking;
    public bool isRunning;
    private float mouseX;
    private float mouseY;
    private float horizontal;
    private float vertical;
    private Vector3 lastvelocity;
    public bool IsGrounded = true;
    private CapsuleCollider collider;
    CharacterStats stats;
    PlayerIndicatorUpdater indicatorupdater;

    //JetPack
    public Image fuel;
    public float jetpacktank = 1f;
    public float JetPackSpeed = 25f;
    private bool moveForwards = false;
    private bool changeFuel = true;
    private bool addFuel = true;
    private float maxJetPackJump = 0.3f;
    private float maxJetPackDirection = 0.1f;
    private float maxFuel = 1f;

    //Camera Movement and JumpController
    private Camera camera;

    Rigidbody rigid;

    void Start()
    {
        ph = PhotonView.Get(this.transform.gameObject);
        this.rigid = GetComponent<Rigidbody>();

        if (ph.isMine)
        {
            this.indicatorupdater = GameObject.FindObjectOfType<PlayerIndicatorUpdater>();
            this.stats = GetComponent<CharacterStats>();
            this.camera = this.transform.Find("Camera").GetComponent<Camera>();
            this.fuel = GameObject.FindWithTag("Fuel").GetComponent<Image>();
            this.collider = GetComponent<CapsuleCollider>();
            this.fuel.fillAmount = jetpacktank;
            indicatorupdater.PlayerTransform = this.transform;
            IsGrounded = true;
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
        if (ph.isMine && !Utility.IsInGame)
        {
            JetPackJump();
            JetPackForward();
            FillUpJetPackFuel();
            CheckIfGrounded();
            OnCrashWithGround();
            lastvelocity = rigid.velocity;
        }

    }

    private void OnCrashWithGround()
    {
        if (Vector3.Distance(lastvelocity, this.rigid.velocity) > 50)
        {
            stats.Armor -= 80;
        }
    }

    void CheckIfCharacterMoved()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !Utility.IsInGame)
        {
            isWalking = true;
            MoveCharacter();
        }
        else
        {
            isWalking = false;
        }
    }

    void MoveCharacter()
    {
        if (isWalking && IsGrounded)
        {
            this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
            this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;

            this.rigid.MovePosition(this.transform.position + this.transform.rotation * new Vector3(this.horizontal, 0, this.vertical));
        }
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !Utility.IsInGame && IsGrounded)
        {
            if (jetpacktank >= maxJetPackDirection)
            {
                ChangeJetpackFuel(0.1f, 1);
                isWalking = false;
                isRunning = true;
                this.horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
                this.vertical = Input.GetAxis("Vertical") * Time.deltaTime * RunSpeed;
                this.rigid.MovePosition(this.transform.position + this.transform.rotation * new Vector3(this.horizontal, 0, this.vertical));

            }
            else
            {
                isRunning = false;
                isWalking = true;
            }
        }
        else
        {
            isRunning = false;
        }
    }

    void LookAround()
    {
        if (!Utility.IsInGame)
        {
            this.mouseX += Input.GetAxis("Mouse X") * Utility.MouseSensitivity;
            this.mouseY -= Input.GetAxis("Mouse Y") * Utility.MouseSensitivity;

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
    }


    //PhotonNetwork
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
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded && !Utility.IsInGame)
        {
            if (jetpacktank >= maxJetPackJump)
            {
                if (isWalking)
                {
                    ChangeJetpackFuel(0.3f, 0);
                    this.rigid.AddRelativeForce(new Vector3(0,JumpHeight, MoveSpeed * this.rigid.mass), ForceMode.Impulse);
                }
                if (isRunning)
                {
                    ChangeJetpackFuel(0.3f, 0);
                    this.rigid.AddRelativeForce(new Vector3(0,JumpHeight, RunSpeed * this.rigid.mass), ForceMode.Impulse);
                }
                if (!isWalking)
                {
                    ChangeJetpackFuel(0.3f, 0);
                    this.rigid.AddRelativeForce(new Vector3(0, this.transform.position.y + JumpHeight, 0), ForceMode.Impulse);
                }
            }
        }
    }

    private void JetPackForward()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !IsGrounded)
        {
            if (this.transform.position.y <= MaxHeight)
            {
                if (jetpacktank >= maxJetPackDirection)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        moveForwards = true;
                        ChangeJetpackFuel(0.2f, 1);
                        this.rigid.AddRelativeForce(new Vector3(0, 0, JetPackDirectionSpeed));


                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        moveForwards = true;
                        ChangeJetpackFuel(0.2f, 1);
                        this.rigid.AddRelativeForce(new Vector3(0, 0, -JetPackDirectionSpeed));

                    }
                    if (Input.GetKey(KeyCode.A))
                    {
                        moveForwards = true;
                        ChangeJetpackFuel(0.2f, 1);
                        this.rigid.AddRelativeForce(new Vector3(-JetPackDirectionSpeed, 0, 0));

                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        moveForwards = true;
                        ChangeJetpackFuel(0.2f, 1);
                        this.rigid.AddRelativeForce(new Vector3(JetPackDirectionSpeed, 0, 0));

                    }
                    if (Input.GetKey(KeyCode.LeftShift) && !moveForwards)
                    {
                        ChangeJetpackFuel(0.1f, 1);
                        this.rigid.AddRelativeForce(new Vector3(0, JetPackHeight, 0));
                    }
                    else
                    {
                        moveForwards = false;
                    }

                }
                else
                {
                    jetpacktank = 0;
                    this.fuel.fillAmount = jetpacktank;
                }
            }
            else if (this.transform.position.y >= MaxHeight)
            {
                this.rigid.AddRelativeForce(-Physics.gravity * rigid.mass * Time.deltaTime);

                ChangeJetpackFuel(0.1f, 1);
            }
        }
    }

    private void FillUpJetPackFuel()
    {
        if (jetpacktank < maxFuel && IsGrounded && !isRunning)
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
                StartCoroutine(ChangeFuel(change, seconds));
            }
        }

    }


    private void CheckIfGrounded()
    {
        RaycastHit groundcheck;
        Vector3 startpoint = this.transform.position + collider.center + Vector3.down * (-collider.height * 2f);
        Vector3 endpoint = startpoint + (Vector3.down * collider.height * 1.25f);
        if (Physics.CapsuleCast(startpoint, endpoint, collider.radius, Vector3.down, out groundcheck, 3f))
        {
            if (groundcheck.transform.gameObject.tag == "Env" ||
                groundcheck.transform.gameObject.tag == "BigPrefab" ||
                groundcheck.transform.gameObject.tag == "SmallPrefab" ||
                groundcheck.transform.gameObject.tag == "Bridge" ||
                groundcheck.transform.gameObject.tag == "Trigger" ||
                groundcheck.transform.gameObject.tag == "island_wood" ||
                groundcheck.transform.gameObject.tag == "island_sand")
            {
                IsGrounded = true;
            }

        }
        else
        {
            IsGrounded = false;
        }
    }

    IEnumerator ChangeFuel(float change, int seconds)
    {
        changeFuel = false;
        jetpacktank -= change;
        this.fuel.fillAmount = jetpacktank;
        if (this.fuel.fillAmount < 0.1f)
        {
            this.fuel.fillAmount = 0.1f;
        }
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
