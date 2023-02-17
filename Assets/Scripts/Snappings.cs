using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class Snappings : MonoBehaviour
{
    public GameObject MiniTarget, MainTarget, player;
    public GameObject torch;
    public bool torchEquip;
    public CharacterController cc;
    void Update()
    {
        if (InputBridge.Instance.AButtonDown)
        {
            cc.enabled = false;
            player.transform.position = MiniTarget.transform.position;
            player.transform.localScale = MiniTarget.transform.localScale;
           

        }
        if (InputBridge.Instance.BButtonDown)
        {
            player.transform.localScale = MainTarget.transform.localScale;
            player.transform.position = MainTarget.transform.position;
            cc.enabled = true;
        }
        if (InputBridge.Instance.XButtonDown)
        {
            torchEquip = !torchEquip;
           torch.SetActive(torchEquip);
        }
    }

}
