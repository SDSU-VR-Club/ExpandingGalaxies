using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        while (amountOfPlanets >= 0)
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
