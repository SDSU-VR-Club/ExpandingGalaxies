using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionTest2D : MonoBehaviour
{
    public Transform player;
    public float speed = 1;
    public GameObject starPrefab;
    public bool randomStars;
    public List<Transform> stars = new List<Transform>();


    void Start()
    {

        if(randomStars)
        for (int i = 0; i < 100; i++)
        {
            stars.Add( Instantiate(starPrefab, new Vector2(Random.Range(-5f,5f), Random.Range(-5f, 5f)), Quaternion.identity).transform );
        }
    }


    void Update()
    {
        foreach (Transform star in stars)
        {
            star.position += (star.position - player.position) * speed * Time.deltaTime;
        }
    }
}
