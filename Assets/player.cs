using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public GameObject bomb;
    public GameObject bombPoint;
    public ParticleSystem bombEffect;
    public AudioSource bombSound;
    float throwWay;

    [Header("POWERBAR SETTINGS")]
    Image PowerBar;
    float powerValue;
    bool powerBarLastLimit = false;
    Coroutine powerCycle;
    

    PhotonView pv;
    bool fireActivity = false;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            PowerBar = GameObject.FindWithTag("PowerBar").GetComponent<Image>();

            if (PhotonNetwork.IsMasterClient)
            {
                gameObject.tag = "Player1";
                transform.position = GameObject.FindWithTag("PlayerPoint_1").transform.position;
                transform.rotation = GameObject.FindWithTag("PlayerPoint_1").transform.rotation;

                throwWay = 2f;
            }
            else
            {
                gameObject.tag = "Player2";
                transform.position = GameObject.FindWithTag("PlayerPoint_2").transform.position;
                transform.rotation = GameObject.FindWithTag("PlayerPoint_2").transform.rotation;

                throwWay = -2f;
            }
        }
        InvokeRepeating("checkStartGame", 0, .5f);
       
    }
    public void checkStartGame()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            if (pv.IsMine)
            {

                powerCycle = StartCoroutine(PowerBarSystem());
                CancelInvoke("checkStartGame");

            }

        }
        else
        {
            StopAllCoroutines();
        }
       
    }
    IEnumerator PowerBarSystem()
    {
        PowerBar.fillAmount = 0;
        powerBarLastLimit = false;
        fireActivity = true;
        while (true)
        {
            if (PowerBar.fillAmount < 1 && !powerBarLastLimit)
            {
                powerValue = 0.01f;
                PowerBar.fillAmount += powerValue;
                yield return new WaitForSeconds(0.0001f * Time.deltaTime);
            }
            else
            {
                powerBarLastLimit = true;
                powerValue = 0.01f;
                PowerBar.fillAmount -= powerValue;
                yield return new WaitForSeconds(0.0001f * Time.deltaTime);
                if(PowerBar.fillAmount == 0)
                {
                    powerBarLastLimit= false;
                }
            }

        }
    }
    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                PhotonNetwork.Instantiate("bombEffect", bombPoint.transform.position, bombPoint.transform.rotation, 0, null);
                bombSound.Play();
                GameObject playerBomb = PhotonNetwork.Instantiate("bomb", bombPoint.transform.position, bombPoint.transform.rotation, 0,null);

                playerBomb.GetComponent<PhotonView>().RPC("transferTag", RpcTarget.All, gameObject.tag);
                Rigidbody2D rb = playerBomb.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(throwWay, 0f) * PowerBar.fillAmount * 10f, ForceMode2D.Impulse);
                fireActivity = false;
                StopCoroutine(powerCycle);
            }
        }
        
    }
    public void powerEnable()
    {

        powerCycle = StartCoroutine(PowerBarSystem());
    }

    public void result(int value)
    {

        if (pv.IsMine)
        {

            if (PhotonNetwork.IsMasterClient)
            {

                if (value == 1)
                {
                    PlayerPrefs.SetInt("totalGame", PlayerPrefs.GetInt("totalGame") + 1);
                    PlayerPrefs.SetInt("victory", PlayerPrefs.GetInt("victory") + 1);
                    PlayerPrefs.SetInt("totalPoint", PlayerPrefs.GetInt("totalPoint") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("totalGame", PlayerPrefs.GetInt("totalGame") + 1);
                    PlayerPrefs.SetInt("defeat", PlayerPrefs.GetInt("defeat") + 1);

                }

            }
            else
            {


                if (value == 2)
                {
                    PlayerPrefs.SetInt("totalGame", PlayerPrefs.GetInt("totalGame") + 1);
                    PlayerPrefs.SetInt("victory", PlayerPrefs.GetInt("victory") + 1);
                    PlayerPrefs.SetInt("totalPoint", PlayerPrefs.GetInt("totalPoint") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("totalGame", PlayerPrefs.GetInt("totalGame") + 1);
                    PlayerPrefs.SetInt("defeat", PlayerPrefs.GetInt("defeat") + 1);
                }
            }
        }
        Time.timeScale = 0;
    }
}
