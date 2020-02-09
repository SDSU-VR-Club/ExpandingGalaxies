using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO :: Use Jobs to increase performance
public class Chunk : MonoBehaviour
{
    public int seed;
    Transform[] clusters;
    
    //Size is a function of Time
    public Vector3 size = Vector3.zero;
    public Vector3Int chunk;

    private float time;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){

    }

    //Changes the chunk to a new index
    public void ShiftChunk(Vector3Int direction){
        chunk += direction;
        GenerateChunk();
    }

    //Run for the scaling update
    public void UpdateChunk(float time){
        this.time = time;
        //Use Preferred RNG method here
        size = new Vector3(time, time, time);
    }

    void ClearChunk(){
        //TODO :: Use Pooling
        for(int i = 0; i < clusters.Length; i++){
            
        }
        //Make sure we have stuff to clean up before we do.
    }

    void GenerateChunk(){
        ClearChunk();

        //Do setup for the clusters
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(this.transform.position, size);
    }
}
