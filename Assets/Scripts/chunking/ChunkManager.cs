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
            
            if(value > maxShellCount){
                value = maxShellCount;    
            }

            if( (_shellCount == value) ){
                return;
            }

            print("shell count value was changed");
            _shellCount = Mathf.Clamp(value, 1, maxShellCount);
            StopCoroutine(generator);
            StartCoroutine(generator = GenerateChunks());
        }
    }

    public float time = 1;

    Dictionary<Vector3Int, Chunk> chunks; //RelativePosition

    public Vector3Int currentChunkID;

    //TODO :: Delete this part
    public Vector3Int lastChunkID;
    // Start is called before the first frame update
    void Start()
    {
        chunks = new Dictionary<Vector3Int, Chunk>();
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
        foreach(Chunk chunk in chunks.Values){
            chunk.ShiftChunk(direction);
        }   
    }

    void UpdateChunks(){
        foreach(Chunk chunk in chunks.Values){
            Vector3 pos = (Vector3)(currentChunkID - chunk.chunkID);
            chunk.transform.position = this.transform.position + pos * time;
            chunk.UpdateChunk(time);
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
            foreach(Chunk chunk in chunks.Values){
                if(chunk != null){
                    chunk.gameObject.SetActive(false);
                }
            }
        }

        //Generate the new chunks for our renderDistance
        for(int i = -shellCount; i <= shellCount; i++){
            for(int j = -shellCount; j <= shellCount; j++){
                for(int k = -shellCount; k <= shellCount; k++){
                    Vector3Int relativePosition = new Vector3Int(i, j, k);
                    if(chunks.ContainsKey(relativePosition)){
                        //Since we already have it, just re-enable it
                        chunks[relativePosition].gameObject.SetActive(true);
                        continue;
                    }

                    //This is the setup for a new chunk
                    GameObject go = new GameObject();
                    Chunk chunk = go.AddComponent<Chunk>();
                    chunks.Add(relativePosition, chunk);
                    go.transform.position = relativePosition;
                    go.transform.name = "" + relativePosition + currentChunkID; 
                    chunk.chunkID = relativePosition + currentChunkID;
                    go.transform.parent = this.transform;
                    chunk.UpdateChunk(time);
                    //chunk.GenerateChunk();
                } 
            }
        }
        yield return null;
    }

}