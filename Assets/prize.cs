using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prize : MonoBehaviour
{
    PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        StartCoroutine(destroyPrize());
    }

    IEnumerator destroyPrize()
    {
        yield return new WaitForSeconds(10f);
        if (pv.IsMine)
            PhotonNetwork.Destroy(gameObject);

    }
}
