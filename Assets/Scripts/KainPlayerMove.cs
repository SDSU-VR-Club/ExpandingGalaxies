using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;

public class KainPlayerMove : MonoBehaviour
{
    public Transform rightHand;
    public float speed = 5;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += rightHand.forward * speed * Time.deltaTime;
        }
    }
}
