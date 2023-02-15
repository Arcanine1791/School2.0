using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapEdge : MonoBehaviour
{
    bool snapped = false;
    GameObject snapparent; // the gameobject this transform will be snapped to
    Vector3 offset; // the offset of this object's position from the parent

    void Update()
    {
        if (snapped == true)
        {
            //retain this objects position in relation to the parent
            transform.position = snapparent.transform.position + offset;
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            snapped = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
            snapped = true;
            snapparent = col.gameObject;
            offset = transform.position - snapparent.transform.position; //store relation to parent
    }
}
