﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoRandomExpansion : MonoBehaviour
{
    public static PseudoRandomExpansion pseudoRandomExpansion;

    [SerializeField]
    GameObject star;

    public int chunkStarCount = 1;
    public float chunkWidth = 100;

    Queue<GameObject> starPool;
    [SerializeField]
    int maxStarCount = 10000;

    GameObject starContainer;
    Dictionary<Vector3Int, GameObject[]> chunkDictonary;
    private void Awake()
    {
        starPool = new Queue<GameObject>();
        pseudoRandomExpansion = this;
        chunkDictonary = new Dictionary<Vector3Int, GameObject[]>();
        starContainer = new GameObject("StarCointainer");
    }
    void Start()
    {
        CreatePool();
    }


    public GameObject[] LoadChunk(int x, int y, int z)
    {
        return LoadChunk(new Vector3Int(x, y, z));
    }
    public GameObject[] LoadChunk(Vector3Int pos)
    {
        if(chunkDictonary.ContainsKey(pos))
        {
            //print("Chunk already loaded, ignoring");
            return null;
        }

        Random.InitState(pos.GetHashCode());

        GameObject[] starsInChunk = new GameObject[chunkStarCount];
        for (int i = 0; i < chunkStarCount; i++)
        {
            SpawnStarFromPool(RandomInCube(chunkWidth) + (Vector3)pos * chunkWidth);
            //starsInChunk[i] = Instantiate(star, RandomInCube(chunkWidth) + (Vector3)pos * chunkWidth, Quaternion.identity);
            //starsInChunk[i].transform.parent = starContainer.transform;
        }
        chunkDictonary.Add(pos, starsInChunk);

        return starsInChunk;
    }


    public void DestroyChunk(int x, int y, int z)
    {
        DestroyChunk(new Vector3Int(x, y, z));
    }
    public void DestroyChunk(Vector3Int pos)
    {
        if (!chunkDictonary.ContainsKey(pos))
        {
            //print("Chunk dosnt exist, ignoring");
            return;
        }

        GameObject[] currChunk = chunkDictonary[pos];
        foreach (GameObject star in currChunk)
        {
            //star.SetActive(false);
            //Destroy(star);
        }
        chunkDictonary.Remove(pos);
    }

    public void ToggleChunk(int x, int y, int z)
    {
        ToggleChunk(new Vector3Int(x, y, z));
    }
    public void ToggleChunk(Vector3Int pos)
    {
        if (chunkDictonary.ContainsKey(pos))
        {
            DestroyChunk(pos);
        }
        else
        {
            LoadChunk(pos);
        }
    }


    Vector3 RandomInCube(float width)
    {
        return RandomVector3(-width/2, width/2, -width/2, width/2, -width/2, width/2);
    }

    Vector3 RandomVector3(float xmin, float xmax, float ymin, float ymax, float zmin, float zmax)
    {
        Vector3 v3;
        v3.x = Random.Range(xmin, xmax);
        v3.y = Random.Range(ymin, ymax);
        v3.z = Random.Range(zmin, zmax);
        return v3;
    }


    void CreatePool()
    {
        for (int i = 0; i < maxStarCount; i++)
        {
            GameObject obj = Instantiate(star);
            obj.SetActive(false);
            starPool.Enqueue(obj);
        }
    }

    GameObject SpawnStarFromPool(Vector3 position)
    {
        GameObject toSpawn = starPool.Dequeue();
        toSpawn.SetActive(true);
        toSpawn.transform.position = position;

        starPool.Enqueue(toSpawn);

        return toSpawn;
    }

}
