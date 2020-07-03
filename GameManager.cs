using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviourPunCallbacks
{
    //public Camera sceneCam;
    public GameObject player;
    public Transform playerSpawnPosition;
    public Text pingrateText;

    public static GameManager instance;

    private void Awake()
    {
        // This will make sure that there is just one Game Manager
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // It optimizes the connection
        PhotonNetwork.SendRate = 25; //20
        PhotonNetwork.SerializationRate = 15; //10
        // It instantiates a player 
        PhotonNetwork.Instantiate(player.name, playerSpawnPosition.position, playerSpawnPosition.rotation);

    }

    private void Update()
    {
        // It displays the Ping
        pingrateText.text = PhotonNetwork.GetPing().ToString();
    }

    // It is call when the player left the Room
    public override void OnLeftRoom()
    {
        // It sends the player to the Main Menu
        SceneManager.LoadScene(0);
    }


    // This method will make the player leaves the room
    public void LeaveRoom()
    {
        // It makes the playe leave the Room
        PhotonNetwork.LeaveRoom();
    }
}
