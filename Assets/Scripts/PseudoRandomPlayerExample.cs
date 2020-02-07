using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoRandomPlayerExample : MonoBehaviour
{
    PseudoRandomExpansion PRE;
    float chunkWidth;

    [SerializeField]
    float distanceToLoad = 16, distanceToDestroy = 16.5f;

    List<Vector3Int> loadedPositions;
    private void Awake()
    {

    }

    void Start()
    {
        PRE = PseudoRandomExpansion.pseudoRandomExpansion;
        chunkWidth = PRE.chunkWidth;



        loadedPositions = new List<Vector3Int>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            print(PRE.activeStarCount);
        }

        getPositionsInRadius(distanceToLoad * chunkWidth);

        for (int i = 0; i < loadedPositions.Count; i++)
        {
            if (Vector3.Distance(transform.position, loadedPositions[i] * (int)chunkWidth) > distanceToDestroy * chunkWidth/2)
            {
                PRE.DestroyChunk(loadedPositions[i]);
                loadedPositions.RemoveAt(i);
            }
        }

    }

    Vector3Int[] getPositionsInRadius(float rangeWidth)
    {
        int xmin = Mathf.FloorToInt((transform.position.x - rangeWidth/2)/chunkWidth);
        int xmax = Mathf.CeilToInt((transform.position.x + rangeWidth/2) / chunkWidth);

        int ymin = Mathf.FloorToInt((transform.position.y - rangeWidth/2) / chunkWidth);
        int ymax = Mathf.CeilToInt((transform.position.y + rangeWidth/2) / chunkWidth);

        int zmin = Mathf.FloorToInt((transform.position.z - rangeWidth/2) / chunkWidth);
        int zmax = Mathf.CeilToInt((transform.position.z + rangeWidth / 2) / chunkWidth);

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
