using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boxInMiddle : MonoBehaviour
{
    float health = 100;
    public GameObject healthCanvas;
    public Image healthBar;
    GameObject gameController;
    PhotonView pv;
    AudioSource boxDestroySound;
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        pv = GetComponent<PhotonView>();
        boxDestroySound = GetComponent<AudioSource>();
    }

    [PunRPC]
    public void damage(float damagePower)
    {
        if (pv.IsMine)
        {
            health -= damagePower;
            healthBar.fillAmount = health / 100;

            if (health <= 0)
            {
                //gameController.GetComponent<GameController>().create_Sound_and_Effect(2, gameObject);
                PhotonNetwork.Instantiate("BoxOnGroundEffect", transform.position, transform.rotation, 0, null);
                boxDestroySound.Play();
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                StartCoroutine(canvasCheck());
            }
            
        }
    }
    IEnumerator canvasCheck()
    {
        if (!healthCanvas.activeInHierarchy)
        {
            healthCanvas.SetActive(true);
            yield return new WaitForSeconds(2);
            healthCanvas.SetActive(false);
        }
       
    }
   
} 

