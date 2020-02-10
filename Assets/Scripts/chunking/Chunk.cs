using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO :: Use Jobs to increase performance
//TODO :: Just Use pooling you dummy
public class Chunk : MonoBehaviour
{   
    //Size is a function of Time
    public Vector3 size = Vector3.zero;
    public Vector3Int chunkID;

    private float time;

    //Changes the chunk to a new index
    //TODO :: USE POOLING!!!
    public void ShiftChunk(Vector3Int direction){
        chunkID += direction;
        this.transform.name = "" + chunkID;
        GenerateChunk();
    }

    //Run for the scaling update
    public virtual void UpdateChunk(float time){
        this.time = time;
        size = new Vector3(time, time, time);
    }

    public virtual void ClearChunk(){
        //TODO :: Use Pooling
        //Make sure we have stuff to clean up before we do.
    }

    public virtual void GenerateChunk(){
        //Do setup for the clusters
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(this.transform.position, size);
    }
}
