using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    private Camera mycamera;

    // Start is called before the first frame update
    void Start()
    {
        mycamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (mycamera.fieldOfView > 1)
            {
                mycamera.fieldOfView--;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (mycamera.fieldOfView < 100)
            {
            mycamera.fieldOfView++;
            }
        }

    }
}
