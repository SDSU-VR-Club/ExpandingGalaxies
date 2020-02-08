using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public int seed;
    Transform[] clusters;
    
    //Size is a function of Time
    public Vector3 size = Vector3.zero;
    
    public Vector3Int chunk;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){

    }

    public void UpdateChunk(float time){
        //Use Preferred RNG method here
        size = new Vector3(time, time, time);
    }

    void GenerateChunk(){
        //Do setup for the clusters
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(this.transform.position, size);
    }
}
