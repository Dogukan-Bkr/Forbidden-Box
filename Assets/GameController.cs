using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [Header("PLAYER SETTINGS")]
    public Image Player1_HealthBar;
    public Image Player2_HealthBar;
    float player1_Health = 100;
    float player2_Health = 100;
    PhotonView pv;

    bool isStart;
    int limit;
    float cd;
    int number;
    public GameObject[] points;

    GameObject Player1;
    GameObject Player2;
    bool endofGame;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        isStart = false;
        limit = 5;
        cd = 15f;
    }

    IEnumerator healthBox()
    {
        number = 0;
        while (true && isStart)
        {   
            if(limit == number) { isStart = false;}
            yield return new WaitForSeconds(cd);
            int value = Random.Range(0, 5);
            PhotonNetwork.Instantiate("Prize", points[value].transform.position, points[value].transform.rotation, 0, null);
            number++;
        }
    }
    [PunRPC]
    public void rnStart()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            isStart = true;
            StartCoroutine(healthBox()); 
        }
        
    }

    [PunRPC]
    public void damage(int condition, float damagePower)
    {

        switch (condition)
        {

            case 1:
                if (PhotonNetwork.IsMasterClient)
                {
                    player1_Health -= damagePower;
                    Player1_HealthBar.fillAmount = player1_Health / 100;
                    if (player1_Health <= 0)
                    {
                        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                        {
                            if (obj.gameObject.CompareTag("endOfGamePanel"))
                            {
                                obj.gameObject.SetActive(true);
                                GameObject.FindWithTag("endGameInformation").GetComponent<TextMeshProUGUI>().text = "Player2 WON";
                            }
                        }
                        winner(2);
                    }
                }
                break;
            case 2:
                if (PhotonNetwork.IsMasterClient) 
                { 
                    player2_Health -= damagePower;
                    Player2_HealthBar.fillAmount = player2_Health / 100;
                    if (player2_Health <= 0)
                    {
                        foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                        {
                            if (obj.gameObject.CompareTag("endOfGamePanel"))
                            {
                                obj.gameObject.SetActive(true);
                                GameObject.FindWithTag("endGameInformation").GetComponent<TextMeshProUGUI>().text = "Player1 WON";
                            }
                        }
                        winner(1);
                    }
                }
                break;

        }

    }

    public void mainMenu()
    {
        GameObject.FindWithTag("serverController").GetComponent<serverController>().tryButton = true;
        PhotonNetwork.LoadLevel(0);
    }

    public void exit()
    {
        PhotonNetwork.LoadLevel(0);
    }

    void winner(int value)
    {
        if (!endofGame)
        {
            GameObject.FindWithTag("Oyuncu_1").GetComponent<player>().result(value);
            GameObject.FindWithTag("Oyuncu_2").GetComponent<player>().result(value);
            endofGame = true;
        }
    }


    [PunRPC]
    public void updateHealth(int chosePlayer)
    {
        switch (chosePlayer)
        {

            case 1:
                player1_Health += 70;
                if (player1_Health > 100)
                {
                    player1_Health = 100;
                    Player1_HealthBar.fillAmount = player1_Health / 100;
                }
                else { Player1_HealthBar.fillAmount = player1_Health / 100; }
                
                
                break;
            case 2:
                player2_Health += 70;
                if (player2_Health > 100)
                {
                    player2_Health = 100;
                    Player2_HealthBar.fillAmount = player2_Health / 100;
                }
                else { Player2_HealthBar.fillAmount = player2_Health / 100; }
                break;

        }
    }

}
