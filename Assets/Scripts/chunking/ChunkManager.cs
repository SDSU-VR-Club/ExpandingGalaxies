using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public class ChunkManager : MonoBehaviour
{
    public enum GenerationMethod {Cuboidal, Spherical, DumbFrustrumCulling}

    public GenerationMethod generationMethod;
    public int renderDistance = 1;
    public int maxShellCount = 4;

    //TODO :: Have a use for frustrum culling lol
    HashSet<Vector3Int> visibleChunks;

    private int _shellCount = 1; //NOTE :: Shells are very expensive, each layer costs n^3 computing power
    
    private int shellCount{
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
            StartCoroutine(generator = Generator(generationMethod));
        }
    }

    private Camera cam;
    public float time = 1;

    Dictionary<Vector3Int, Chunk> chunks; //The Key is the relative position

    //Lets us know where we currently are
    public Vector3Int currentChunkID;

    //TODO :: Delete this part, testing purposes only
    Vector3Int lastChunkID;

    Transform chunksParent;

    // Start is called before the first frame update
    void Start()
    {
        chunksParent = new GameObject().transform;
        chunksParent.parent = this.transform;
        chunksParent.name = "Chunks";
        chunks = new Dictionary<Vector3Int, Chunk>();
        StartCoroutine(generator = Generator(generationMethod));
        
        //Need the camera for the frustum culling
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        CalculateShells();
        if(generationMethod == GenerationMethod.DumbFrustrumCulling)
            DumbFrustrumCulling();
        UpdateChunks();

        //TODO :: Remove for ease of tesing only
        Vector3Int diff = lastChunkID - currentChunkID;
        if(diff != Vector3Int.zero){
            lastChunkID = currentChunkID;
            ShiftChunk(diff);
        }
    }

    public void ShiftChunk(Vector3Int direction){
        currentChunkID += direction;
        lastChunkID = currentChunkID;
        foreach(Chunk chunk in chunks.Values){
            chunk.ShiftChunk(direction);
        }   
    }

    //Make this a Job
    void UpdateChunks(){
        if(generationMethod == GenerationMethod.DumbFrustrumCulling){
        
            foreach(Vector3Int cpos in visibleChunks){
                Vector3 pos = (Vector3)(currentChunkID - chunks[cpos].chunkID);
                chunks[cpos].transform.position = this.transform.position + pos * time;
                chunks[cpos].UpdateChunk(time);
            }
        } else {
            foreach(Chunk chunk in chunks.Values){
                if(!chunk.isVisible){
                    continue;
                }
                Vector3 pos = (Vector3)(currentChunkID - chunk.chunkID);
                chunk.transform.position = this.transform.position + pos * time;
                chunk.UpdateChunk(time);
            }
        }
    }

    //Lets the chunks know if they are visible, so we don't have to do work on them
    void DumbFrustrumCulling(){
        //TODO :: Calculate which should be visible instead of if they are visible
        //TODO :: this lets us only have the GameObjects exist that we can see
        //TODO :: instead of having them exist just not enabled

        visibleChunks = FrustrumChunkFinder();

        foreach(Chunk chunk in chunks.Values){
            chunk.isVisible = false;
        }

        foreach(Vector3Int pos in visibleChunks){
            if(!chunks.ContainsKey(pos)){
                chunks.Add(pos, GenerateChunk(-pos));
            }
            chunks[pos].isVisible = true;
            //chunks[pos].isVisible = IsVisibleFrom(new Bounds(this.transform.position + (Vector3)(pos) * time, new Vector3(time, time, time)), cam);
        }
    }

    //Caculate the number of shells needed for the desired render distance
    void CalculateShells(){
        shellCount = Mathf.CeilToInt(renderDistance/time);
    }

    IEnumerator generator;
    IEnumerator Generator(GenerationMethod method){
        switch(method){
            case GenerationMethod.Cuboidal:
            yield return StartCoroutine(CubicChunkGenerator());
            break;

            case GenerationMethod.Spherical:
            yield return StartCoroutine(SphericalChunkGenerator());
            break;
        }
    }

    Chunk GenerateChunk(Vector3Int relativePosition){
        //This is the setup for a new chunk
        GameObject go = new GameObject();
        Chunk chunk = go.AddComponent<Chunk>();
        //chunks.Add(relativePosition, chunk);
        go.transform.position = relativePosition;
        go.transform.name = "" + relativePosition + currentChunkID; 
        chunk.chunkID = relativePosition + currentChunkID;
        go.transform.parent = chunksParent.transform;
        chunk.UpdateChunk(time);
        //chunk.GenerateChunk();
        return chunk;
    }

    //Generates chunks arount (0,0,0) in the shape of a cube
    IEnumerator CubicChunkGenerator(){
        if(chunks != null){
            foreach(Chunk chunk in chunks.Values){
                if(chunk != null){
                    chunk.gameObject.SetActive(false); //Save those objects for later
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
                    Chunk chunk = GenerateChunk(relativePosition);
                    chunks.Add(relativePosition, chunk);
                    chunk.isVisible = true;
                    
                } 
            }
        }
        yield return null;
    }

    IEnumerator SphericalChunkGenerator(){
        if(chunks != null){
            foreach(Chunk chunk in chunks.Values){
                if(chunk != null){
                    chunk.gameObject.SetActive(false); //Save those objects for later
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

                    if(Vector3.Distance(Vector3.zero, (Vector3)relativePosition) > renderDistance)
                        continue;

                    //This is the setup for a new chunk
                    Chunk chunk = GenerateChunk(relativePosition);
                    chunks.Add(relativePosition, chunk);
                    chunk.isVisible = true;
                } 
            }
        }
        yield return null;
    }

    //TODO :: Only generate the chunks that we actually can see
    //TODO :: Also only have the minimum number of chunks needed from our FOV
    
    HashSet<Vector3Int> FrustrumChunkFinder(){
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        Vector3 up = cam.transform.up;
        //Iterate from (0,0,0) in the direction of forward
        //at step 1 check the next 9 if they are in the fov range
        HashSet<Vector3Int> visible = new HashSet<Vector3Int>();
        for(float i = 0; i <= shellCount; i+=.5f){
            Vector3Int center = new Vector3Int(Mathf.RoundToInt(forward.x * i), Mathf.RoundToInt(forward.y * i), Mathf.RoundToInt(forward.z * i));
            //This is the center

            for(float x = -i*2; x <= i*2; x+=.5f){
                for(float y = -i*2; y <= i*2; y+=.5f){
                    visible.Add(Vector3Int.RoundToInt(center + right * x + up * y));
                }
            }
        }

        return visible;
    }

    Vector3Int CeilFloorVector3(Vector3 v){
        return new Vector3Int(CeilFloorToInt(v.x), CeilFloorToInt(v.y), CeilFloorToInt(v.z));
    }

    int CeilFloorToInt(float v){
        if(v <= 0) { //If Negative floor
            return Mathf.FloorToInt(v);
        }
        return Mathf.CeilToInt(v);
    }

    //https://gist.github.com/coastwise/5951291
    public float horizontalFOV(){
        float vFOVInRads = cam.fieldOfView * Mathf.Deg2Rad;
        float hFOVInRads = 2 * Mathf.Atan( Mathf.Tan(vFOVInRads / 2) * Camera.main.aspect);
        return hFOVInRads * Mathf.Rad2Deg;
    }

    //Source https://wiki.unity3d.com/index.php/IsVisibleFrom
    //Really handy, seriously check it out
    public static bool IsVisibleFrom(Bounds bounds, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, bounds);
	}

}