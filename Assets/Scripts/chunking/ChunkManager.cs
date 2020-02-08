using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    private int _renderDistance = 1;
    
    public int renderDistance{
        get{ return _renderDistance; }

        set{
            _renderDistance = value;    
            int shellNum = renderDistance * 2 + 1;
            chunks = new Chunk[shellNum, shellNum, shellNum];
        }
    }

    public float time = 1;

    Chunk[,,] chunks;
    public Vector3Int currentChunk;

    // Start is called before the first frame update
    void Start()
    {
        int shellNum = renderDistance * 2 + 1;
        chunks = new Chunk[shellNum, shellNum, shellNum];
        GenerateChunks();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChunks();
    }

    void UpdateChunks(){
        for(int i = -renderDistance; i <= renderDistance; i++){
            for(int j = -renderDistance; j <= renderDistance; j++){
                for(int k = -renderDistance; k <= renderDistance; k++){
                    int ii = i + renderDistance, ij = j + renderDistance, ik = k + renderDistance;
                    chunks[ii, ij, ik].transform.position = new Vector3(i, j, k) * time;
                    chunks[ii, ij, ik].UpdateChunk(time);
                }
            }
        }
    }

    void GenerateChunks(){
        for(int i = -renderDistance; i <= renderDistance; i++){
            for(int j = -renderDistance; j <= renderDistance; j++){
                for(int k = -renderDistance; k <= renderDistance; k++){
                    int ii = i + renderDistance, ij = j + renderDistance, ik = k + renderDistance;

                    GameObject go = new GameObject();
                    Chunk chunk = go.AddComponent<Chunk>();
                    chunks[ii, ij, ik] = chunk;
                    go.transform.position = new Vector3(i, j, k);
                    chunk.chunk = new Vector3Int(i, j, k);
                    go.transform.parent = this.transform;
                }
            }
        }
    }
}
