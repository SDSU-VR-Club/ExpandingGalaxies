using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
public class StateManager : MonoBehaviour
{
    bool isPaused = false;
    public ChunkManager chunk;
    public Transform leftHand;
    public Text Distance;
    public LayerMask clusterMask;
    public Transform playerTransform;
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        Distance.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            chunk.time += Time.smoothDeltaTime;

        }
        if (Input.GetKeyDown(KeyCode.Space)||leftHand.gameObject.active && leftHand.GetComponent<Hand>().grabPinchAction.stateDown)
        {
            isPaused = !isPaused;
        }

        if (isPaused && Input.GetKeyDown(KeyCode.D))
        {
            RaycastHit HitBoi;
            if (Physics.Raycast(leftHand.position, leftHand.forward, out HitBoi, Mathf.Infinity, clusterMask))
            {
                Distance.text = HitBoi.distance.ToString();
            }
        }

        if(isPaused)
        {
            Distance.enabled = true;
            RaycastHit Hitboi;
            if (Input.GetKeyDown(KeyCode.R) && Physics.Raycast(leftHand.position, leftHand.forward, out Hitboi, Mathf.Infinity, clusterMask))
            {
                //playerTransform.position = Hitboi.point;
                print("working");
                StopAllCoroutines();
                StartCoroutine(moveTowards(Hitboi.point));
            }
        }
        else
        {
            Distance.enabled = false;
        }
        
    } 
    IEnumerator moveTowards(Vector3 destination )
    {
        float distance = Vector3.Distance(destination, playerTransform.position);
        Vector3 direction = -(playerTransform.position - destination).normalized;
        while (distance > 0)
        {
            yield return new WaitForEndOfFrame();
            print("working");
            playerTransform.position += direction * Speed;
            distance -= Speed;
        }
        
    }
}
