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
    bool chunksGenerated = false;
    bool _isVisible = false;
    public bool isVisible{
        get{return _isVisible;}
        set{
            _isVisible = value;
            if(_isVisible && !chunksGenerated){
                GenerateChunk(); //Only run if the chunks have not been generated and we are visible now
            }
        }
    }

    private float time;

    //Changes the chunk to a new index
    //TODO :: USE POOLING!!!
    public void ShiftChunk(Vector3Int direction){
        chunkID += direction;
        this.transform.name = "" + chunkID;
        chunksGenerated = false;
        if(isVisible)
            GenerateChunk(); //Only update the chunks we can see right now
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
        chunksGenerated = true;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        if(!isVisible)
            Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, size);
    }
}
