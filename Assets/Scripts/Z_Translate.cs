using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Z_Translate : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * Time.deltaTime);
        transform.Translate(Input.GetAxis("Horizontal") * Vector3.right * Time.deltaTime);
    }
}