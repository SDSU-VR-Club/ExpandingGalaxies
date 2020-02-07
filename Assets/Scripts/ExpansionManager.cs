using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionManager : MonoBehaviour
{
    Transform center;
    public float speed = 1;
    public float acceleration = 0;
    public List<Transform> stars = new List<Transform>();


    void Start()
    {
        center = transform;
        stars = FindObjectOfType<Generator>().currentUniverse.clusters;
    }

    void Update()
    {
        speed += acceleration * Time.deltaTime;

        foreach (Transform star in stars)
        {
            star.position += (star.position - center.position) * speed * Time.deltaTime;
        }
    }
}
