using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    public Transform button;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                Physics.Raycast(ray, out hit ,500f);

                button.GetComponent<Text>().text = touch.position.ToString() + "\n" + hit.collider.name;
            }
        }


        /*if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit ciaone;

            Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);
            Physics.Raycast(ray, out ciaone, 500f);

            if (ciaone.collider != null)
                button.GetComponent<Text>().text = Input.mousePosition.ToString() + "\n" + ciaone.collider.name;
        }*/
    }


}
