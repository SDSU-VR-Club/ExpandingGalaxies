using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkManager : MonoBehaviour
{
    public enum GenerationMethod {Cuboidal, Spherical}

    public GenerationMethod generationMethod;
    public int renderDistance = 1;
    public int maxShellCount = 4;

    //TODO :: Have a use for frustrum culling lol

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


    // Start is called before the first frame update
    void Start()
    {
        chunks = new Dictionary<Vector3Int, Chunk>();
        StartCoroutine(generator = Generator(generationMethod));
        
        //Need the camera for the frustum culling
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateShells();
        UpdateChunks();
        FrustrumCulling();

        //TODO :: Remove for ease of tesing only
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

    //Lets the chunks know if they are visible, so we don't have to do work on them
    void FrustrumCulling(){
        //TODO :: Calculate which are visible instead of if they are visible
        //TODO :: this lets us only have the GameObjects exist that we can see
        //TODO :: instead of having them exist just not enabled

        foreach(Chunk chunk in chunks.Values){
            chunk.isVisible = IsVisibleFrom(new Bounds(chunk.transform.position, chunk.size), cam);
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

    //TODO :: Only generate the chunks that we actually can see
    //TODO :: Also only have the minimum number of chunks needed from our FOV
    IEnumerator FrustrumChunkGenerator(){
        yield return null;
    }


    //Source https://wiki.unity3d.com/index.php/IsVisibleFrom
    //Really handy, seriously check it out
    public static bool IsVisibleFrom(Bounds bounds, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, bounds);
	}

}