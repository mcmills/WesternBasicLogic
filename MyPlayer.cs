using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    public float MoveSpeed = 3f;
    public float smoothRotationTime = 0.25f;
    public bool enableMobileInputs = false;
    public float JumpForce;
    GameObject crossHairPrefab;
    FixedJoystick joystick;
    public Transform rayOrigin;

    //Sound
    public AudioSource shootSound;
    public AudioSource runSound;

    //Health
    public GameObject healthBar;
    public Image fillImage;
    public float playerHealth = 1f;
    public float damage = 0.01f;

    ParticleSystem muzzle;
    float currentSpeed;
    float speedVelocity;
    float currentVeclocity;
    //Vector3 crossHairVel;
    public bool fire;

    Transform cameraTransform;


    Animator anim;


    private void Awake()
    {
        // It sets the player health to full
        playerHealth = 1f;
        // If this is my player
        if (photonView.IsMine)
        {
            // It finds an object named Fixed Joystick and it get a component named FixedJoystick
            joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
            // It loads the CrosshairCanvas from the Resources folder as a Game Object
            crossHairPrefab = Resources.Load("CrosshairCanvas") as GameObject;
            // It finds an object named MainCamera
            cameraTransform = GameObject.Find("MainCamera").transform;
        }
        // It finds an object named GunMuzzle in other object names SciFiRifle(Clone) and It gets the PartucleSystem component
        muzzle = rayOrigin.Find("SciFiRifle(Clone)/GunMuzzle").GetComponent<ParticleSystem>();

    }


    private void Start()
    {
        // If this is my character
        if (photonView.IsMine)
        {
            // It sets the fire bool to false
            fire = false;
            // It gets the Animator conponent from this object
            anim = GetComponent<Animator>();
            // It finds an object named FireBtn, it gets the FireBtnScript component and it sets it to this object
            GameObject.Find("FireBtn").GetComponent<FireBtnScript>().SetPlayer(this);
            // It finds an object named JumpBtn, it gets the FixedButton component and it sets it to this object
            GameObject.Find("JumpBtn").GetComponent<FixedButton>().SetPlayer(this);
            // It instantiates a crossHairPrefab --
            crossHairPrefab = Instantiate(crossHairPrefab);
            // It makes the healthBar visible
            healthBar.SetActive(true);
        }
        // If this is not my character
        else
        {
            // It disables the BetterJump component
            GetComponent<BetterJump>().enabled = false;
        }
        
    }


    void Update()
    {
        // If this is my character
        if (photonView.IsMine)
        {
            // It makes this method run
            LocalPlayerUpdate();
        }
    }


    void LocalPlayerUpdate()
    {
        // It get the input of the space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        // It creates a variable names input
        Vector2 input = Vector2.zero;
        // It checks if the player is using a phone or a pc 
        input = Myinputs(input);
        // It creates a variable names inputDir
        Vector2 inputDir = input.normalized;
        // It rotates the player with the camera
        RotateWCamera(inputDir);
        // It is firing
        if (fire)
        {
            // It makes the character rotate with the camera 
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            // It rotates the character
            transform.eulerAngles = Vector3.up * rotation;
        }
        // It creates a targetSpeed with a given value (MoveSpeed) by inputDir magnitude
        float tragetSpeed = MoveSpeed * inputDir.magnitude;
        // It smooths the speed
        currentSpeed = Mathf.SmoothDamp(currentSpeed, tragetSpeed, ref speedVelocity, 0.1f);
        // It controls the Running Animation
        RunningAnim(inputDir);
        // It is not firing
        if (!fire)
        {
            // It moves the character
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        }
    }


    // It plays the MuzzleFlash
    public void MuzzleFlash()
    {
       
        muzzle.Play();
    }


    private void LateUpdate()
    {
        // If this is my character
        if (photonView.IsMine)
        {
            // It calls the PositionCrossHair method
            PositionCrossHair();
        }
    }


    // It positions the Cross Hair
    void PositionCrossHair()
    {
        // Structure used to get information back from a raycast
        RaycastHit hit;
        // The ray cames from the camera
        Ray ray = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));
        // The ray just interacts with objects in the Default layer 
        int layer_mask = LayerMask.GetMask("Default");
        // It creates a ray
        if(Physics.Raycast(ray, out hit, 100f, layer_mask))
        {
            // It sets the start position from the ray
            Vector3 start = cameraTransform.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            // The Cross Hair follows the ray position
            crossHairPrefab.transform.position = ray.GetPoint(10);
            // It makes the Cross Hair look  the camera always
            crossHairPrefab.transform.LookAt(cameraTransform);
        }
    }


    // It is call when the player wants to fire
    public void Fire()
    {
        // Set the fire bool to true
        fire = true;
        // Start the Fire animation
        anim.SetTrigger("Fire");
        // Structure used to get information back from a raycast
        RaycastHit hit;
        // It creates a ray
        if(Physics.Raycast(rayOrigin.position, cameraTransform.forward, out hit, 25f))
        {
            // It gets the PhotonView component from the object hit
            PhotonView pv = hit.transform.GetComponent<PhotonView>();
            // It true when pv isn't null, the character isn't mine and the object hit has the Player tag
            if (pv != null && !hit.transform.GetComponent<PhotonView>().IsMine && hit.transform.tag == "Player")
            {
                // It damages the other character
                hit.transform.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllBuffered, damage);
            }
        }
        // It plays the Shoot Sound
        shootSound.Play();
        // The MuzzleFlash method starts
        MuzzleFlash();
    }


    // It is call when the character stops firing 
    public void FireUp()
    {
        // It sets fire bool to false
        fire = false;
        // It stops tje muzzle
        muzzle.Stop();
    }


    // It is call when the player wants to Jump
    public void Jump()
    {
        // It starts the Jump Animation
        anim.SetTrigger("Jump");
        // It gets the Rigidbody component
        Rigidbody rb = GetComponent<Rigidbody>();
        // It sets the velocity to 0
        rb.velocity = Vector3.zero;
        // It sets the angular Velocity to 0
        rb.angularVelocity = Vector3.zero;
        // It adds Force to the character up
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }


    // This method is call remotely when the character gets hurt
    [PunRPC]
    public void GetDamage(float amount)
    {
        // It sets the amount value minus the playerHealth value
        playerHealth -= amount;
        // If this is my character
        if (photonView.IsMine)
        {
            // It sets the playerHealth value to the fillAmount value
            fillImage.fillAmount = playerHealth;
            // If playerHealth value is less than O
            if(playerHealth <= 0f)
            {
                // Die
                Die();
            }
        }
    }


    // This method synchronized variables that constantly change
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If we are sending information
        if (stream.IsWriting)
        {
            // It sends the fire value
            stream.SendNext(fire);
        }
        // If it isn't the local player, it means it is the server
        else
        {
            // If fire is true
            if ((bool)stream.ReceiveNext())
            {
                // It calls the MuzzleFlash method
                MuzzleFlash();
            }
            // If fire is false
            else
            {
                // It calls the FireUp method
                FireUp();
            }
        }
    }


    // The player dies
    void Die()
    {
        // If this is my character
        if (photonView.IsMine)
        {
            // It calls the LeaveRoom method
            GameManager.instance.LeaveRoom();
        }
    }


    // It checks if the player is using a phone or a pc
    Vector2 Myinputs(Vector2 input)
    {
        // enableMobileInputs is true
        if (enableMobileInputs)
        {
            // It sets the joystick values to input variable
            input = new Vector2(joystick.input.x, joystick.input.y);
        }
        // enableMobileInputs is false
        else
        {
            // It sets the GetAzisRaw values to input variable
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        return input;
    }


    // It rotates the player with the camera
    void RotateWCamera(Vector2 inputDirRWC)
    {
        // inputDir is not zero, it makes sure that the character rotation follows the camera rotation if it's moving
        if (inputDirRWC != Vector2.zero)
        {
            // It makes the character rotate with the camera
            float rotation = Mathf.Atan2(inputDirRWC.x, inputDirRWC.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            // It smooths the character rotation
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref currentVeclocity, smoothRotationTime);
            // The runSound is not playing
            if (!runSound.isPlaying)
            {
                // It plays the runSound
                runSound.Play();
            }
        }
        // inputDir is zero
        else
        {
            // It stops the runSound
            runSound.Stop();
        }
    }


    // It controls the Running Animation
    void RunningAnim(Vector2 inputDirRuAni)
    {
        // The inputDir magnitude is greater than 0
        if (inputDirRuAni.magnitude > 0f)
        {
            // It sets the Running animation to true
            anim.SetBool("Running", true);
        }
        // The inputDir magnitude is equal than 0
        else if (inputDirRuAni.magnitude == 0f)
        {
            // It sets the Running animation to false
            anim.SetBool("Running", false);
        }
    }
}
