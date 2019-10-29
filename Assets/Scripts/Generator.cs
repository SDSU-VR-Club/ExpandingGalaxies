using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates a Universe object that spawns a certain configuration of cluster objects with specified paramters
public class Generator : MonoBehaviour
{
    //controls the random elements of universe creation
    public int seed = 1;
    //universeRadiusScalar controls the size of clusters
    public float clusterRadiusScalar = 1.0f;
    //Clusters spawn in between innerRadius and outerRadius
    public float innerRadius = 10.0f;
    public float outerRadius = 200.0f;

    //the desired number of clusters to spawn
    public int clusterCount;

    //public GameObject planet;
    //object prefab to spawn in universe
    public GameObject cluster;

    [HideInInspector]
    public Universe currentUniverse;
    void Start()
    {
        Universe testUniverse = new Universe(cluster, seed, clusterRadiusScalar, innerRadius, outerRadius,clusterCount);
    }


}



//universe
public class Universe : MonoBehaviour
{
    //private Vector3 currentCord, prevCord;
    //private float clusterScaleScalar = 20;
    //private int planetAmount = 20;

    //constructor that generates all game objects within the universe (universe->clusters->(stars?) in clusters)
    public Universe(GameObject cluster, int seed, float clusterScalar, float innerRadius,float outerRadius,int clusterCount)
    {
        //Create the transform that will hold the clusters
        GameObject clusterParent = new GameObject();
        clusterParent.name = "clusters";
        //create the seed as per user input seed value ( same seed = same generation )
        Random.InitState(seed);

        //players radius surrounding them (area of no spawn for clusters)
        //Vector3 playerRadiusVector = new Vector3(playerRadius, playerRadius, playerRadius);
        //Debug.Log(playerRadiusVector);
        int failSafeCount = 0, currentSpawn = 1;

        //a failure is defined as a collider spawning inside of another collider
        //run until an arbitrary limit has been reached or the requiste number of clusters has been spawned
        while (failSafeCount < 10000&&currentSpawn<clusterCount+1)
        {
            //generate current random coord
            float randomDistance = Random.Range(innerRadius, outerRadius);
            Vector3 spawnPos = Random.insideUnitSphere.normalized *randomDistance ;
            

            //IF there are colliders within the randomly generated coord, with radius of cluster : (clusterScale / 2)
            Collider[] colliders = Physics.OverlapSphere(spawnPos, (clusterScalar / 2), layerMask: 0, queryTriggerInteraction: QueryTriggerInteraction.Collide); //get all colliders within this pos and r
            //OR, if the coordinate is inside of the "player viewing radius"
            //OR, if the previous coordinates magnitude is within 2 radius' (radiuses?? idfkgatdamn) of the current coord (FOR good measure)
            // THEN dont create clluster here
            if ((colliders.Length > 0)
                //|| (currentCord.magnitude < playerRadius)
                //|| (Mathf.Abs(currentCord.magnitude - prevCord.magnitude) < (clusterScaleScalar/2)) 
                //|| (Mathf.Abs(prevCord.magnitude - currentCord.magnitude) < (clusterScaleScalar / 2)))
                )
            {
                ++failSafeCount;
                Debug.Log("Failure: " + Physics.OverlapSphere(spawnPos, (clusterScalar / 2)).Length + " --COLLISION-- " + spawnPos);
                Debug.Log("Failure count: " + failSafeCount + "Generating new..");
            }
            //else ,generate new cluster, scale by scalar, populate with populatecluster method
            else
            {
                //create a new cluster at spawnPos with random location
                GameObject newCluster = Instantiate(cluster, spawnPos, Random.rotation,clusterParent.transform);
                newCluster.name = "Cluster" + currentSpawn;
                newCluster.transform.localScale *= clusterScalar;

                //populate generated cluster
                //Cluster.PopulateCluster(newCluster, planet, seed, planetAmount, currentCord, indexer, clusterScaleScalar);

                //prevCord = currentCord;
                Debug.Log("Cluster " + (currentSpawn) + ": " + newCluster.transform.position);
                ++currentSpawn;
            }
        }
    }
}
