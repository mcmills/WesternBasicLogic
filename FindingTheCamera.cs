using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FindingTheCamera : MonoBehaviour
{
    CinemachineFreeLook myFreeLook;
    Transform thirdCamera;
    PhotonView playerPhotonView;


    // Start is called before the first frame update
    void Start()
    {
        // It sets the PhotonView Player
        playerPhotonView = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();
        // It checks if the player is mine
        if (playerPhotonView.IsMine)
        {
            // It sets the CinemachineFreeLook component
            myFreeLook = GetComponent<CinemachineFreeLook>();
            // It finds the Player Camera and it separates them
            thirdCamera = GameObject.Find("Player(Clone)/ThirdPersonCamera").GetComponent<Transform>().parent = null;
        }
        // It sets the player as a target
        CheckingTheCamera();
    }


    // It sets the player as a target
    void CheckingTheCamera()
    {
        myFreeLook.Follow = GameObject.Find("Player(Clone)/CameraTarget").GetComponent<Transform>();
        myFreeLook.LookAt = GameObject.Find("Player(Clone)/CameraTarget").GetComponent<Transform>();
    }
}
