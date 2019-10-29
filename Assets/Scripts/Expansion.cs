using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expansion : MonoBehaviour
{
    Transform player;
    public float speed = 1;
    public float acceleration = 0;
    public GameObject starPrefab;
    public float randomRange = 10;
    public bool randomStars;
    public int starCount = 100;
    public List<Transform> stars = new List<Transform>();


    void Start()
    {
        player = transform;

        if (randomStars)
            for (int i = 0; i < starCount; i++)
            {
                stars.Add(Instantiate(starPrefab, new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange)), Quaternion.identity).transform);
            }
    }


    void Update()
    {
        speed += acceleration * Time.deltaTime;

        foreach (Transform star in stars)
        {
            star.position += (star.position - player.position) * speed * Time.deltaTime;
        }
    }
}
