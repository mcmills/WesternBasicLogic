using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CursorManager : MonoBehaviour
{
    PhotonView playerPhotonView;
    // Start is called before the first frame update
    void Start()
    {
        // It sets the PhotonView Player
        playerPhotonView = GameObject.Find("Player(Clone)").GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        // It checks if the player is mine
        if (playerPhotonView.IsMine)
        {
            // Lock the cursor
            LockTCursor(false);
        }
    }
    // It controls the Cursor state
    void LockTCursor(bool cursorState)
    {
        // If it is true the cursor is enable
        if (cursorState)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        // Otherwise the cursor is disable
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
