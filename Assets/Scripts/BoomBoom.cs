using UnityEngine;
using BNG;

public class BoomBoom : MonoBehaviour
{
    public Transform XRRig;
    Vector3 APos; 
    public Transform BPos, lightPos;
    public GameObject lightt;
    private void Awake()
    {
        APos = XRRig.position;
    }
    private void Update()
    {
        if (InputBridge.Instance.AButtonDown)
            BigBoom();
        if (InputBridge.Instance.BButtonDown)
            Revere();
    }
    public void BigBoom()
    {
        transform.localScale = new Vector3(5, 5, 5);
        XRRig.position = BPos.position;
        //Vector3 rr = BPos.position + new Vector3 (0, 0.3f, -0.6f);
        Instantiate(lightt,lightPos.position,lightPos.rotation,lightPos);
    }
    public void Revere()
    {
        transform.localScale = new Vector3(1, 1, 1);
        XRRig.position = APos;
    }
    public void LiteOn()
    {
        lightPos.gameObject.SetActive(true);
    }
    public void LiteOff()
    {
        lightPos.gameObject.SetActive(true);
    }
}
