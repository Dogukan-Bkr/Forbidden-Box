using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuController : MonoBehaviour
{
    public GameObject firstPanel;
    public GameObject secondPanel;
    public InputField userName;
    public Text currentUsername;
    public TextMeshProUGUI[] statistic;
    public Text serverInformation;
    GameObject start_Game;
    GameObject create_Lobby;
    void Start()
    {
        
        if (!PlayerPrefs.HasKey("userName"))
        {
            PlayerPrefs.SetInt("totalGame", 0);
            PlayerPrefs.SetInt("defeat", 0);
            PlayerPrefs.SetInt("victory", 0);
            PlayerPrefs.SetInt("totalPoint", 0);
            firstPanel.SetActive(true);
            writeValue();
        }
        else
        {
            secondPanel.SetActive(true);
            currentUsername.text = PlayerPrefs.GetString("userName");
            writeValue();
        }
    }

   public void saveUsername()
    {
        PlayerPrefs.SetString("userName", userName.text);
        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
        currentUsername.text = userName.text;
        start_Game = GameObject.FindWithTag("start_Game");
        create_Lobby = GameObject.FindWithTag("create_Lobby");
        start_Game.GetComponent<Button>().interactable = true;
        create_Lobby.GetComponent<Button>().interactable = true;
    }

    void writeValue()
    {
        statistic[0].text = PlayerPrefs.GetInt("totalGame").ToString();
        statistic[1].text = PlayerPrefs.GetInt("defeat").ToString();
        statistic[2].text = PlayerPrefs.GetInt("victory").ToString();
        statistic[3].text = PlayerPrefs.GetInt("totalPoint").ToString();
    }

}
