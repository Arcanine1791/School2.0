using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class ShapeSelection : MonoBehaviour
{
    public Grabber _grabber;
    public GameObject grabbedObject;
    public Blocks blocks;
    public Vector3 pos;


    private void Update()
    {
        if (InputBridge.Instance.AButtonDown || Input.GetKeyDown(KeyCode.C))
        {
            // ChangeLocation();
        }
        //if (_grabber.onGrabEvent != null)
        //    CreatSnapShot();
    }
    public void CreatSnapShot()
    {
        grabbedObject = _grabber.HeldGrabbable.gameObject;
        if (_grabber.HeldGrabbable.gameObject.tag == "Selecter")
        {
            pos = _grabber.HeldGrabbable.initialLocation;
            //Invoke("PlaceAfterTime", 2f);
        }
    }

    void PlaceAfterTime()
    {
        Debug.Log("hihihi");
        Instantiate(grabbedObject, pos, Quaternion.identity);
    }

    public void HighlightThePossibleSnaps(string holdedObjTag)
    {
        switch (holdedObjTag)
        {
            case "Walls":
                break;
            case "Doors":
                break;
        }
    }
}
