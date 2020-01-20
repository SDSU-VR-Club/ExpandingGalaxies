using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarterPRPExample : MonoBehaviour
{
    public Transform player;
    CarterPRE PRE;
    float chunkWidth;

    [SerializeField]
    float distanceToLoad = 16, distanceToDestroy = 16.5f;

    List<Vector3Int> loadedPositions;
    private void Awake()
    {

    }

    void Start()
    {
        PRE = CarterPRE.pseudoRandomExpansion;
        chunkWidth = PRE.chunkWidth;



        loadedPositions = new List<Vector3Int>();
    }

    void Update()
    {
        player.position+=player.position * PRE.expansionRate;
        if (Input.GetKeyDown(KeyCode.C))
        {
            print(PRE.activeStarCount);
        }

        getPositionsInRadius(distanceToLoad * chunkWidth*PRE.expansionFactor);

        for (int i = 0; i < loadedPositions.Count; i++)
        {
            if (Vector3.Distance(transform.position, loadedPositions[i] * (int)chunkWidth) > distanceToDestroy * (chunkWidth * PRE.expansionFactor) / 2)
            {
                PRE.DestroyChunk(loadedPositions[i]);
                loadedPositions.RemoveAt(i);
            }
        }

    }

    Vector3Int[] getPositionsInRadius(float rangeWidth)
    {
        int xmin = Mathf.FloorToInt((transform.position.x - rangeWidth/2)/ (chunkWidth * PRE.expansionFactor));
        int xmax = Mathf.CeilToInt((transform.position.x + rangeWidth/2) / (chunkWidth * PRE.expansionFactor));

        int ymin = Mathf.FloorToInt((transform.position.y - rangeWidth/2) / (chunkWidth * PRE.expansionFactor));
        int ymax = Mathf.CeilToInt((transform.position.y + rangeWidth/2) / (chunkWidth * PRE.expansionFactor));

        int zmin = Mathf.FloorToInt((transform.position.z - rangeWidth/2) / (chunkWidth * PRE.expansionFactor));
        int zmax = Mathf.CeilToInt((transform.position.z + rangeWidth / 2) / (chunkWidth * PRE.expansionFactor));

        List<Vector3Int> vectors = new List<Vector3Int>();
        for (int z = zmin; z <= zmax; z++)
        {
            for (int y = ymin; y <= ymax; y++)
            {
                for (int x = xmin; x <= xmax; x++)
                {
                    if (PRE.LoadChunk(x, y, z) != null)
                        loadedPositions.Add(new Vector3Int(x, y, z));
                }
            }
        }

        Vector3Int[] test = null;
        return test;
    }

}
