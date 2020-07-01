using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public PhotonView playerPhotonView;
    public float yAxis;
    public float xAxis;
    public float rotationSensitivity = 8f;
    public bool enableMobileInputs = false;
    public float offset;
    FixedTouchField touchField;


    float rotationMin = -40f;
    float rotationMax = 80f;

    float smoothTime = 0.12f;

    Transform target;

    Vector3 targetRotation;

    Vector3 currentVel;

    private void Awake()
    {
        // If this is my player
        if (playerPhotonView.IsMine)
        {
            // It moves this object out of its parent
            transform.parent = null;
            // It finds the touchField object and it gets the FixedTouchField
            touchField = GameObject.Find("TouchPanel").GetComponent<FixedTouchField>();
            // It finds the player and it sets the player to the target of this camera
            target = GetLocalPlayer().transform.GetChild(3);
        }
        else
        {
            // It sets this object false if this isn't my player
            this.gameObject.SetActive(false);
        }
        
    }

    private void Start()
    {
        // If enableMobileInputs is true
        if (enableMobileInputs)
        {
            // It sets the rotationSensitivity to 0.2f
            rotationSensitivity = 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If enableMobileInputs is true
        if (enableMobileInputs)
        {
            // It gets the camera movement from the touchField
            yAxis += touchField.TouchDist.x * rotationSensitivity;
            xAxis -= touchField.TouchDist.y * rotationSensitivity;
        }
        else
        {
            // It gets the camera movement from the mouse
            yAxis += Input.GetAxis("Mouse X") * rotationSensitivity;
            xAxis -= Input.GetAxis("Mouse Y") * rotationSensitivity;
        }
        // It clamps the rotation of the camera
        xAxis = Mathf.Clamp(xAxis, rotationMin, rotationMax);

        // It smooths the rotations of the camera
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(xAxis, yAxis), ref currentVel, smoothTime);
        // It sets the rotation to the camera
        transform.eulerAngles = targetRotation;

        // It gets the offset of the camera
        Vector3 _offset = target.position - transform.forward * offset;
        // It sets the Y offset of the camera
        _offset.y = 0.8f;
        // It sets the offset of the camera
        transform.position = _offset;
    }
    // This method finds our player
    GameObject GetLocalPlayer()
    {
        // It finds any object with the tag Player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // It takes every character with the tag Player
        foreach(GameObject player in players)
        {
            // It finds wich character is my player
            if (player.GetComponent<PhotonView>().IsMine)
            {
                // It sends out player
                return player;
            }
        }
        // It sends null
        return null;
    }

}
