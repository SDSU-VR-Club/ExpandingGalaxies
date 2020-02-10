using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkManager : MonoBehaviour
{
    public int renderDistance = 1;
    public int maxShellCount = 4;
    //Shell count is a function of renderDistance and chunkSize

    //TODO :: Frustrum Culling
    //Turns out you cannot use jobs here :(
            //Guess we are gonna use corutines instead

    //NOTE :: Shells are very expensive, each layer costs n^3 computing

    public int _shellCount = 1;
    public int shellCount{
        get{
            return _shellCount;
        }

        set{
            if( (_shellCount == value) || (value > maxShellCount) ){
                return;
            }
            print("shell count value was changed");
            _shellCount = Mathf.Clamp(value, 1, maxShellCount);
            StopCoroutine(generator);
            StartCoroutine(generator = GenerateChunks());
        }
    }

    public float time = 1;

    List<Chunk> chunks;

    public Vector3Int currentChunkID;

    //TODO :: Delete this part
    public Vector3Int lastChunkID;
    // Start is called before the first frame update
    void Start()
    {
        chunks = new List<Chunk>();
        StartCoroutine(generator = GenerateChunks());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateShells();
        UpdateChunks();

        Vector3Int diff = lastChunkID - currentChunkID;
        if(diff != Vector3Int.zero){
            lastChunkID = currentChunkID;
            ShiftChunk(diff);
        }
    }

    void ShiftChunk(Vector3Int direction){
        for(int i = 0; i < chunks.Count; i++){
            chunks[i].ShiftChunk(direction);
        }
    }

    void UpdateChunks(){
        for(int i = 0; i < chunks.Count; i++){
            Vector3 pos = (Vector3)(currentChunkID - chunks[i].chunkID);
            chunks[i].transform.position = this.transform.position + pos * time;
            chunks[i].UpdateChunk(time);
        }
    }

    //Caculate the number of shells needed for the desired render distance
    void CalculateShells(){
        shellCount = Mathf.CeilToInt(renderDistance/time);
    }
    IEnumerator generator;

    IEnumerator GenerateChunks(){
        //TODO :: Only trash the GameObjects that need to be deleted
        if(chunks != null){
            for(int i = 0; i < chunks.Count; i++){
                if(chunks[i] != null){
                    GameObject.DestroyImmediate(chunks[i].gameObject);
                }
            }
        }

        chunks.Clear();
        
        //Turns out you cannot use jobs here :(
            //Guess we are gonna use corutines instead
        //Generate the new chunks for our renderDistance
        for(int i = -shellCount; i <= shellCount; i++){
            for(int j = -shellCount; j <= shellCount; j++){
                for(int k = -shellCount; k <= shellCount; k++){
                    // calculate the indexes, i prefece meaning "index"
                    int ii = i + shellCount, ij = j + shellCount, ik = k + shellCount;
                    //You cannot instantiate with Job system
                    GameObject go = new GameObject();
                    Chunk chunk = go.AddComponent<Chunk>();
                    chunks.Add(chunk);
                    go.transform.position = new Vector3(i, j, k);
                    go.transform.name = "chunk (" + i + "," + j + "," + k + ")"; 
                    chunk.chunkID = new Vector3Int(i, j, k);
                    go.transform.parent = this.transform;
                    chunk.UpdateChunk(time);
                    //chunk.GenerateChunk();
                    
                } 
            }
            yield return null;
        }
        yield return null;
    }

}