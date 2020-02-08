using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public int renderDistance = 1;
    public int maxShellCount = 4;
    //Shell count is a function of renderDistance and chunkSize

    //TODO Frustrum Culling

    public int _shellCount = 1;
    public int shellCount{
        get{
            return _shellCount;
        }

        set{
            if(_shellCount == value){
                return;
            }
            print("shell count value was changed");
            _shellCount = Mathf.Clamp(value, 1, maxShellCount);
            GenerateChunks();

        }
    }

    public float time = 1;

    Chunk[,,] chunks;
    public Vector3Int currentChunk;

    // Start is called before the first frame update
    void Start()
    {
        GenerateChunks();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateShells();
        UpdateChunks();
    }

    void UpdateChunks(){
        for(int i = -shellCount; i <= shellCount; i++){
            for(int j = -shellCount; j <= shellCount; j++){
                for(int k = -shellCount; k <= shellCount; k++){
                    int ii = i + shellCount, ij = j + shellCount, ik = k + shellCount;
                    chunks[ii, ij, ik].transform.position = new Vector3(i, j, k) * time;
                    chunks[ii, ij, ik].UpdateChunk(time);
                }
            }
        }
    }

    //Caculate the number of shells needed for the desired render distance
    void CalculateShells(){
        shellCount = Mathf.RoundToInt(renderDistance/time);
    }

    void GenerateChunks(){
        if(chunks != null){
            //Clear any chindren we currently have
            for(int i = 0; i < chunks.GetLength(0); i++){
                for(int j = 0; j < chunks.GetLength(1); j++){
                    for(int k = 0; k < chunks.GetLength(2); k++){
                        if(chunks[i,j,k] != null) 
                            GameObject.DestroyImmediate(chunks[i, j, k].transform.gameObject);
                    }
                }
            }
        }

        chunks = new Chunk[shellCount * 2 + 1, shellCount * 2 + 1, shellCount * 2 + 1];

        //Generate the new chunks for our renderDistance
        for(int i = -shellCount; i <= shellCount; i++){
            for(int j = -shellCount; j <= shellCount; j++){
                for(int k = -shellCount; k <= shellCount; k++){
                    // calculate the indexes, i prefece meaning "index"
                    int ii = i + shellCount, ij = j + shellCount, ik = k + shellCount;

                    GameObject go = new GameObject();
                    Chunk chunk = go.AddComponent<Chunk>();
                    chunks[ii, ij, ik] = chunk;
                    go.transform.position = new Vector3(i, j, k);
                    go.transform.name = "chunk (" + i + "," + j + "," + k + ")"; 
                    chunk.chunk = new Vector3Int(i, j, k);
                    go.transform.parent = this.transform;
                }
            }
        }
    }
}
