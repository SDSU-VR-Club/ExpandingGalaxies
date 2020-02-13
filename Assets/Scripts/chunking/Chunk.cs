using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;

//TODO :: Use Jobs to increase performance
//TODO :: Just Use pooling you dummy
public class Chunk : MonoBehaviour
{   
    //Size is a function of Time
    public Vector3 size = Vector3.zero;
    public Vector3Int chunkID;

    TransformAccessArray clusters;
    Vector3[] positions;
    JobHandle clusterHandle;

    bool chunksGenerated = false;
    bool _isVisible = false;

    public bool isVisible{
        get{return _isVisible;}
        set{
            _isVisible = value;
            if(!_isVisible){
                //this.gameObject.SetActive(false);
                return;
            }
            
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

        //Should only need an Update
        UpdateChunk(time);
    }

    //Run for the scaling update
    public virtual void UpdateChunk(float time){
        this.time = time;
        size = new Vector3(time, time, time);
        
        clusterHandle.Complete();
        ChunkUpdateJob job = new ChunkUpdateJob(){
            size = time,
            positions = this.positions
        };
    }

    public virtual void ClearChunk(){
        //TODO :: Use Pooling
        //Make sure we have stuff to clean up before we do.
    }

    public virtual void GenerateChunk(){
        //Do setup for the clusters
        chunksGenerated = true;

        //This only gets run once, so get it right the first time
        //Also make sure that you set the positions array
    }

    void OnDrawGizmos(){
        Gizmos.color = new Color(1, 1, 1, .1f);
        if(isVisible)
            Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, size);
        
    }
}

public struct ChunkUpdateJob : IJobParallelForTransform{

    public float size;
    public Vector3[] positions;

    public void Execute(int index, TransformAccess transform){
        Vector3 localPos = transform.localPosition;
        localPos = positions[index] * size;
        transform.localPosition = localPos;
    }

}