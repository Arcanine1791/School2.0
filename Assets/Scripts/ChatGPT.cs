using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPT : MonoBehaviour
{
    public float snapvalue = 1f;

    private Vector3 screenPoint;
    private Vector3 offset;

    public static bool snap;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (snap)
        {
            float snapInverse = 1 / snapvalue;


            pos.x = Mathf.Round(transform.position.x * snapInverse) / snapInverse;
            pos.y = Mathf.Round(transform.position.y * snapInverse) / snapInverse;
            pos.z = Mathf.Round(transform.position.z * snapInverse) / snapInverse;

            transform.position = pos;
        }
    }


    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Cursor.visible = false;

    }

    void OnMouseDrag()
    {
        //transform.SetParent(null);

        Vector3 curScreenPoint = new(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        //Vector3 partnerPos = Camera.main.WorldToViewportPoint(partnerGO.transform.position);
        Vector3 myPos = Camera.main.WorldToViewportPoint(transform.position);
        //dist = Vector2.Distance(partnerPos, myPos);
    }

    void OnMouseUp()
    {
        snap = false;
        Cursor.visible = true;
    }

        private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            snap = true;
            Debug.Log(collision.gameObject);
        }
    }
}
