using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI connectionStateLabel;
    private string _nickname, _gameversion, _roomName;

    // Start is called before the first frame update
    void Start()
    {
        _nickname = "User_" + Random.Range(1000, 9999).ToString();
        _gameversion = "0.0.0";
        _roomName = "art_gallery_0";
        ConnectToServer();
        UpdateConnectionState("Connecting to the server...");
    }

    private void UpdateConnectionState(string value)
    {
        connectionStateLabel.text = value;
    }

    private void ConnectToServer()
    {
        //PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.NickName = _nickname;
        //PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.GameVersion = _gameversion;
        PhotonNetwork.ConnectUsingSettings();
    }

    private void JoinDefaultRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        //PhotonNetwork.JoinOrCreateRoom(MasterManager.GameSettings.RoomName, roomOptions, TypedLobby.Default);
        PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }

    private void LoadGameScene()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
            //SceneLoader.Load(SceneLoader.Scene.ArtGallery_v3);
        }
    }

    public override void OnConnectedToMaster()
    {
        JoinDefaultRoom();
        UpdateConnectionState("Setting up the gallery...");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from the game server due to: " + cause.ToString());
    }

    public override void OnJoinedRoom()
    {
        LoadGameScene();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Room creation failed due to: " + message);
    }
}