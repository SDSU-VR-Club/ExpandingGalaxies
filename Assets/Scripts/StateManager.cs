using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
public class StateManager : MonoBehaviour
{
    bool isPaused = false;
    public ChunkManager chunk;
    public Transform leftHand;
    public Text Distance;
    public Text timeText;
    public LayerMask clusterMask;
    public Transform playerTransform;
    public float Speed;
    public float minimumTime;
    public float maximumTime;
    public float startTime;
    private float timeGrowth;
    public float timeGrowthIncrement;
    // Start is called before the first frame update
    void Start()
    {
        chunk.time = startTime;
        Distance.enabled = false;
        timeGrowth = timeGrowthIncrement;
        

    }
    
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isPaused = true;
            timeGrowth = 0;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            timeGrowth += timeGrowthIncrement;
            if (timeGrowth == 0)
                isPaused = true;
            else
            {
                isPaused = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            timeGrowth -= timeGrowthIncrement;
            if (timeGrowth == 0) 
                isPaused = true;
            else
            {
                
                
                isPaused = false;
            }
        }
        if (chunk.time >= maximumTime&&timeGrowth>0)
        {
            timeGrowth = 0;
            chunk.time = maximumTime;
        }
        if (chunk.time <=minimumTime && timeGrowth < 0)
        {
            timeGrowth = 0;
            chunk.time = minimumTime;
        }

        if (!isPaused)
        {
            chunk.time += timeGrowth*Time.smoothDeltaTime;
            float timePercentage = chunk.time / (maximumTime - minimumTime);
            timePercentage *= 100;
            timePercentage += -11f;
            timeText.text = timePercentage.ToString("F1") + " billion years";
            if (timeGrowth < 0)
            {
                RenderSettings.fogColor = Color.blue;
            }
            if (timeGrowth > 0)
            {
                RenderSettings.fogColor = Color.red;
            }
            
        }
        else
        {
            RenderSettings.fogColor = Color.clear;
        }
        //leftHand.gameObject.active && leftHand.GetComponent<Hand>().grabPinchAction.stateDown


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
        transform.parent = null;
        float distance = Vector3.Distance(destination, playerTransform.position);
        Vector3 direction = -(playerTransform.position - destination).normalized;
        while (distance > 0)
        {
            yield return new WaitForEndOfFrame();
            print("working");
            playerTransform.position += direction * Speed;
            distance -= Speed;
        }
        
        RaycastHit[] hits; 
        hits = Physics.SphereCastAll (playerTransform.position, 2, Vector3.one, 0, clusterMask);

        if (hits.Length >= 1) {
            Transform target; 
            target = hits[0].collider.transform;
            print(target.position);
            foreach (RaycastHit hit in hits){
                if ((hit.point - playerTransform.position).sqrMagnitude < (target.position - playerTransform.position).sqrMagnitude)
                target = hit.collider.transform;
            }
            playerTransform.parent = target;
        }
        
    }
}
