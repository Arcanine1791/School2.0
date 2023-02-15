using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Snappings : MonoBehaviour
{
    public GameObject MiniTarget, MainTarget, player;
    void Update()
    {
        if(InputBridge.Instance.AButtonDown) {
        
            player.transform.localScale= MiniTarget.transform.localScale;
            player.transform.position= MiniTarget.transform.position;
        }
        if (InputBridge.Instance.BButtonDown)
        {
            player.transform.localScale = MainTarget.transform.localScale;
            player.transform.position= MainTarget.transform.localPosition;
        }
    }
}
