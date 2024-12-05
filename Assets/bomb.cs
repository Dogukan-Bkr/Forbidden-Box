using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class bomb : MonoBehaviour
{
    float damagePower;
    int playerValue;
    GameObject gameController;
    GameObject player;
    PhotonView pv;
    AudioSource bombDestroySound;

    void Start()
    {
        damagePower = 20;
        gameController = GameObject.FindWithTag("GameController");
        pv = GetComponent<PhotonView>();
        bombDestroySound = GetComponent<AudioSource>();
    }
    [PunRPC]
    public void transferTag(string tag)
    {
        
        player = GameObject.FindWithTag(tag);
        if(tag == "Player1")
        {
            playerValue = 1;
        }
        else
        {
            playerValue = 2;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("boxInMiddle"))
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("damage",RpcTarget.All,damagePower);
            player.GetComponent<player>().powerEnable();
            PhotonNetwork.Instantiate("BombOnBoxEffect", transform.position, transform.rotation, 0, null);
            bombDestroySound.Play();
            if (pv.IsMine)
            {  PhotonNetwork.Destroy(gameObject); }


        }
        if (collision.gameObject.CompareTag("Player2_Tower") || collision.gameObject.CompareTag("Player2"))
        {
            if(playerValue != 2) { gameController.GetComponent<PhotonView>().RPC("damage", RpcTarget.All, 2, damagePower); }
            
            player.GetComponent<player>().powerEnable();
            PhotonNetwork.Instantiate("BombOnBoxEffect", transform.position, transform.rotation, 0, null);
            bombDestroySound.Play();
            if (pv.IsMine)
            {PhotonNetwork.Destroy(gameObject); }
        }
        if (collision.gameObject.CompareTag("Player1_Tower") || collision.gameObject.CompareTag("Player1"))
        {

            if (playerValue != 1) { gameController.GetComponent<PhotonView>().RPC("damage", RpcTarget.All, 1, damagePower); }

            player.GetComponent<player>().powerEnable();
            PhotonNetwork.Instantiate("BombOnBoxEffect", transform.position, transform.rotation, 0, null);
            bombDestroySound.Play();
            if (pv.IsMine)
            { PhotonNetwork.Destroy(gameObject); }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            
            player.GetComponent<player>().powerEnable();
            PhotonNetwork.Instantiate("BombOnBoxEffect", transform.position, transform.rotation, 0, null);
            bombDestroySound.Play();
            if (pv.IsMine)
            { PhotonNetwork.Destroy(gameObject);}
        }
        if (collision.gameObject.CompareTag("WoodBarrier"))
        {

            player.GetComponent<player>().powerEnable();
            PhotonNetwork.Instantiate("BombOnBoxEffect", transform.position, transform.rotation, 0, null);
            bombDestroySound.Play();
            if (pv.IsMine)
            {PhotonNetwork.Destroy(gameObject);}
        }
        if (collision.gameObject.CompareTag("Prize"))
        {
            gameController.GetComponent<PhotonView>().RPC("updateHealth", RpcTarget.All, playerValue);
            PhotonNetwork.Destroy(collision.transform.gameObject);
            player.GetComponent<player>().powerEnable();
            PhotonNetwork.Instantiate("BombOnBoxEffect", transform.position, transform.rotation, 0, null);
            bombDestroySound.Play();
            if (pv.IsMine)
            { PhotonNetwork.Destroy(gameObject); }
        }
        if (collision.gameObject.CompareTag("bomb"))
        {
            player.GetComponent <player>().powerEnable();
            PhotonNetwork.Instantiate("BombOnBoxEffect", transform.position, transform.rotation, 0, null);
            bombDestroySound.Play();
            if (pv.IsMine)
            { PhotonNetwork.Destroy(gameObject); }
        }

    }
}
