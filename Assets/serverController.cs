using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class serverController : MonoBehaviourPunCallbacks
{
    GameObject serverInformation;
    GameObject save_Username;
    GameObject start_Game;
    GameObject create_Lobby;
    public bool tryButton;
    void Start()
    {
        serverInformation = GameObject.FindWithTag("server_Information");
        save_Username = GameObject.FindWithTag("save_Username");
        start_Game = GameObject.FindWithTag("start_Game");
        create_Lobby = GameObject.FindWithTag("create_Lobby");
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }
    public override void OnConnectedToMaster()
    {
        serverInformation.GetComponent<TextMeshProUGUI>().text = "Connected to Server";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        serverInformation.GetComponent<TextMeshProUGUI>().text = "Connected to Lobby";
        if (!PlayerPrefs.HasKey("userName"))
        {
            save_Username.GetComponent<Button>().interactable = true;
        }
        else
        {
            start_Game.GetComponent<Button>().interactable = true;
            create_Lobby.GetComponent<Button>().interactable = true;
        }
        
    }

    public void startGame()
    {
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.JoinRandomRoom();
    }
    public void createLobby()
    {
        PhotonNetwork.LoadLevel(1);
        string lobyName = Random.Range(0, 897456).ToString();
        PhotonNetwork.JoinOrCreateRoom(lobyName, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        InvokeRepeating("checkInformation", 0, 1f);
        GameObject obj = PhotonNetwork.Instantiate("player",Vector3.zero, Quaternion.identity,0,null);
        obj.GetComponent<PhotonView>().Owner.NickName = PlayerPrefs.GetString("userName");
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            obj.gameObject.tag = "Player2";
            GameObject.FindWithTag("GameController").GetComponent<PhotonView>().RPC("rnStart", RpcTarget.All);
        }
        
    }
    public override void OnLeftRoom() 
    {
        if (tryButton)
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();
            //  Debug.Log("Sen Çýktýn");
            PlayerPrefs.SetInt("totalGame", PlayerPrefs.GetInt("totalGame") + 1);
            PlayerPrefs.SetInt("defeat", PlayerPrefs.GetInt("defeat") + 1);
        }
    }
    public override void OnLeftLobby()
    {

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        if (tryButton)
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();
            PlayerPrefs.SetInt("totalGame", PlayerPrefs.GetInt("totalGame") + 1);
            PlayerPrefs.SetInt("victory", PlayerPrefs.GetInt("victory") + 1);
            PlayerPrefs.SetInt("totalPoint", PlayerPrefs.GetInt("totalPoint") + 150);
        }

        InvokeRepeating("checkInformation", 0, 1f);
        
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        serverInformation.GetComponent<TextMeshProUGUI>().text = "ERROR: JOIN ROOM FAILED";
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        serverInformation.GetComponent<TextMeshProUGUI>().text = "ERROR: JOIN RANDOM ROOM FAILED";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        serverInformation.GetComponent<TextMeshProUGUI>().text = "ERROR: CREATE ROOM FAILED";

    }

    void checkInformation()
    {
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            GameObject.FindWithTag("playerWaiting").SetActive(false);
            GameObject.FindWithTag("Player1_Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Player2_Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
            CancelInvoke("checkInformation");
        }
        else
        {
            GameObject.FindWithTag("playerWaiting").SetActive(true);
            GameObject.FindWithTag("Player1_Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Player2_Name").GetComponent<TextMeshProUGUI>().text = "..........";
        }
    }

}
