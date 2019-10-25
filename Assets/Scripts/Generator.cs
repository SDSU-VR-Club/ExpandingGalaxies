using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generator
public class Generator : MonoBehaviour
{
    public int seed = 1;

    public float universeRadiusScalar = 1.0f;

    public float playerRadius = 10.0f;

    public GameObject planet;
    public GameObject cluster;

    void Start()
    {
        Universe testUniverse = new Universe(cluster, planet, seed, universeRadiusScalar, playerRadius);
    }

    // Update is called once per frame
    void Update()
    {
    }

}

//cluster
public class Cluster : MonoBehaviour
{
    private static Vector3 currentCord, prevCord;

    public static void PopulateCluster(GameObject cluster, GameObject planet, int seed, int amountOfPlanets, Vector3 clusterNoiseValue, int j, float clusterScalar)
    {
        //initialize the psuedorando-generator with user seed
        Random.InitState(seed + j);

        //create first planet inside cluster
        currentCord = clusterNoiseValue + (Random.insideUnitSphere * (clusterScalar / 2));
        GameObject newPlanet = Instantiate(planet, cluster.transform, true);
        newPlanet.transform.position = currentCord;

        //populate cluster without colliding planets 
        currentCord = clusterNoiseValue + (Random.insideUnitSphere * (clusterScalar / 2));
        while(amountOfPlanets >= 0)
        {
            Collider[] colliders = Physics.OverlapSphere(currentCord, (planet.transform.localScale.x / 2), layerMask: 0, queryTriggerInteraction: QueryTriggerInteraction.Collide);
            if (colliders.Length == 0)
            {
                newPlanet = Instantiate(planet, cluster.transform, true);
                newPlanet.transform.position = currentCord;
                amountOfPlanets--;
                currentCord = clusterNoiseValue + (Random.insideUnitSphere * (clusterScalar / 2));
            }
        }
    }
}

//universe
public class Universe : MonoBehaviour
{
    private Vector3 currentCord, prevCord;
    private float clusterScaleScalar = 20;
    private int planetAmount = 20;

    //constructor that generates all game objects within the universe (universe->clusters->(stars?) in clusters)
    public Universe(GameObject cluster, GameObject planet, int seed, float uniScalar, float playerRadius)
    {
        //create the seed as per user input seed value ( same seed = same generation )
        Random.InitState(seed);

        //players radius surrounding them (area of no spawn for clusters)
        Vector3 playerRadiusVector = new Vector3(playerRadius, playerRadius, playerRadius);
        Debug.Log(playerRadiusVector);
        int failureCount = 0, indexer = 1;

        //a failure is defined as a collider spawning inside of another collider
        while (failureCount < 50)
        {
            //generate current random coord
            this.currentCord = Random.insideUnitSphere * uniScalar;
            Debug.Log(this.currentCord);

            //IF there are colliders within the randomly generated coord, with radius of cluster : (clusterScale / 2)
            Collider[] colliders = Physics.OverlapSphere(this.currentCord, (clusterScaleScalar / 2), layerMask: 0, queryTriggerInteraction: QueryTriggerInteraction.Collide); //get all colliders within this pos and r
            //OR, if the coordinate is inside of the "player viewing radius"
            //OR, if the previous coordinates magnitude is within 2 radius' (radiuses?? idfkgatdamn) of the current coord (FOR good measure)
            // THEN dont create clluster here
            if ((colliders.Length > 0)
                || (currentCord.magnitude < playerRadiusVector.magnitude) 
                || (Mathf.Abs(currentCord.magnitude - prevCord.magnitude) < (clusterScaleScalar/2)) 
                || (Mathf.Abs(prevCord.magnitude - currentCord.magnitude) < (clusterScaleScalar / 2)))
            {
                ++failureCount;
                Debug.Log("Failure: " + Physics.OverlapSphere(currentCord, (clusterScaleScalar / 2)).Length + " --COLLISION-- " + currentCord);
                Debug.Log("Failure count: " + failureCount + "Generating new..");
            }
            //else ,generate new cluster, scale by scalar, populate with populatecluster method
            else
            {
                GameObject newCluster = Instantiate(cluster, currentCord, Quaternion.identity);
                newCluster.name = "Cluster" + (indexer);
                newCluster.transform.localScale *= clusterScaleScalar;

                //populate generated cluster
                Cluster.PopulateCluster(newCluster, planet, seed, planetAmount, currentCord, indexer, clusterScaleScalar);

                prevCord = currentCord;
                Debug.Log("Cluster " + (indexer) + ": " + newCluster.transform.position);
                ++indexer;
            }
        }
    }
}
