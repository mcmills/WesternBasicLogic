using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("--- UI Screens---")]
    public GameObject roomUI;
    public GameObject connectUI;

    [Header("---UI Text---")]
    public Text statusText;
    public Text connectingText;

    [Header("---UI InputFields---")]
    public InputField createRoom;
    public InputField joinRoom;

    private void Awake()
    {
        // It connects to Photon as configured in the PhotonServerSettings file
        PhotonNetwork.ConnectUsingSettings();
    }

    // It is call when the game connects to the Master
    public override void OnConnectedToMaster()
    {
        // It modifies the connectingText
        connectingText.text = "Joining Lobby...";
        // It sets a default Lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // It is call when the game joins to a Lobby
    public override void OnJoinedLobby()
    {
        // It makes the connectUI invisible
        connectUI.SetActive(false);
        // It makes the roomUI visible
        roomUI.SetActive(true);
        // It modifies the statusText
        statusText.text = "Joined To Lobby";
    }
    // It is call when the game connects to a Room
    public override void OnJoinedRoom()
    {
        // Photon loads the level 1
        PhotonNetwork.LoadLevel(1);
    }
    // It is call when the game disconnects
    public override void OnDisconnected(DisconnectCause cause)
    {
        // It makes the connectUI visible
        connectUI.SetActive(true);
        // It modifies the connectingText + cause
        connectingText.text = "Disconnected... " + cause.ToString();
        // It makes the roomUI invisible
        roomUI.SetActive(false);
    }
    // It is call when joinRandom room failed
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // It creates a Random name for a room
        int roomName = Random.Range(0, 10000);
        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        // It creates a room with a Random name
        PhotonNetwork.CreateRoom(roomName.ToString(), roomOptions, TypedLobby.Default, null);
    }

    #region ButtonClicks
    // It is call when someone press the Create Button
    public void Onclick_CreateBtn()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        // It creates a Room with a given name
        PhotonNetwork.CreateRoom(createRoom.text, roomOptions, TypedLobby.Default, null);
    }
    // It is call when someone press the Join Button
    public void Onclick_JoinBtn()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        // It Join an existing Room of it creates a Room if there is none with a given name
        PhotonNetwork.JoinOrCreateRoom(joinRoom.text, roomOptions, TypedLobby.Default);
    }
    // It is call when someone press the Play Now Button
    public void OnClick_PlayNow()
    {
        // It tries to Join a Random Room
        PhotonNetwork.JoinRandomRoom();
        // It modifies the statusText
        statusText.text = "Creating Room... Please Wait...";
    }
    #endregion

}
