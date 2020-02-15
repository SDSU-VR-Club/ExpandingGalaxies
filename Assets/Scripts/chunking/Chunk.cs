using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Jobs;
//using UnityEngine.Jobs;

//using Unity.Burst;

//TODO :: Use Jobs to increase performance
//TODO :: Just Use pooling you dummy
public class Chunk : MonoBehaviour
{   
    //Size is a function of Time
    public Vector3 size = Vector3.zero;
    public Vector3Int chunkID;
    public GameObject clusterPrefab;
    //TransformAccessArray clustersAccess;
    List<Transform> clusters;
    //JobHandle clusterHandle;

    bool _isVisible = false;

    public bool isVisible{
        get{return _isVisible;}
        set{
            _isVisible = value;
            if(!_isVisible){
                //this.gameObject.SetActive(false);
                return;
            }
        }
    }
    private float time;

    void Awake(){
        clusters = new List<Transform>();
        //clustersAccess = new TransformAccessArray(0, -1);       
    }

    //Changes the chunk to a new index
    //TODO :: USE POOLING!!!
    public void ShiftChunk(Vector3Int direction){
        chunkID += direction;
        this.transform.name = "" + chunkID;
        GenerateChunk();
        //Should only need an Update
        UpdateChunk(time);
    }

    //DO NOT CHANGE THIS FUNCTION!!
    public virtual void UpdateChunk(float time){
        this.time = time;
        size = new Vector3(time, time, time);
        GenerateChunk();
        //Jobs code
        /*clusterHandle.Complete();
        ChunkUpdateJob job = new ChunkUpdateJob(){
            size = time,
            positions = this.positions
        };
        clusterHandle = job.Schedule(clustersAccess);
        JobHandle.ScheduleBatchedJobs();*/


    }

    //This gets called when you shift chunks
    public virtual void GenerateChunk(){
        Random.InitState(chunkID.GetHashCode());
        //positions = new NativeArray<Vector3>(clusters.Count, Allocator.Persistent);
        for(int i = 0; i < clusters.Count; i++){
            clusters[i].position = this.transform.position + new Vector3(time * Random.Range(-.5f, .5f), time * Random.Range(-.5f, .5f), time * Random.Range(-.5f, .5f));
        }
    }

    public virtual void ClusterSetup(){
        int numClusters = NumClusters(chunkID);
        for(int i = 0; i < numClusters; i++){
            clusters.Add(Instantiate(clusterPrefab).transform);
            clusters[i].parent = this.transform;
        }
        //clustersAccess.capacity = clusters.Count;
        //clustersAccess.SetTransforms(clusters.ToArray());
        GenerateChunk();
    }

    public static int NumClusters(Vector3Int ChunkID){
        //TODO implement
        return 15;
    }

    void OnDrawGizmos(){
        Gizmos.color = new Color(1, 1, 1, .1f);
        if(isVisible)
            Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, size);
        
    }
}

/*[BurstCompile]
public struct ChunkUpdateJob : IJobParallelForTransform{

    public float size;

    public void Execute(int index, TransformAccess transform){
        Vector3 localPos = transform.position;
        //localPos = positions[index] * size;
        transform.localPosition = localPos;
    }

}*/