using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StateManager : MonoBehaviour
{
    bool isPaused = false;
    public ChunkManager chunk;
    public Transform leftHand;
    public Transform rightHand;
    public Text DistanceText;
    public Text timeText;
    public Text VelocityText;
    public Text NameText;
    public LayerMask clusterMask;
    public Transform playerTransform;
    public float Speed;
    public float minimumTime;
    public float maximumTime;
    public float startTime;
    private float timeGrowth;
    public float timeGrowthIncrement;
    public GameObject selected;
    public GameObject selectedDisplay;
    bool moving = false;
    public GameObject[] laserPointers;
    // Start is called before the first frame update
    void Start()
    {
        chunk.time = startTime;
        selectedDisplay.SetActive(false);
        timeGrowth = timeGrowthIncrement;
        

    }
    public void forward()
    {
        if (moving)
            return;
        timeGrowth += timeGrowthIncrement;
        if (timeGrowth == 0)
            isPaused = true;
        else
        {
            isPaused = false;
        }
    }
    public void backward()
    {
        if (moving)
            return;
        timeGrowth -= timeGrowthIncrement;
        if (timeGrowth == 0)
            isPaused = true;
        else
        {


            isPaused = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                chunk.time = startTime;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isPaused = true;
                timeGrowth = 0;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                forward();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                backward();
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
            timeText.text = "Time: "+timePercentage.ToString("F1") + "b years";
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
        //
        if(isPaused&&!moving)
        {
            
            RaycastHit Hitboi;
            foreach(GameObject laser in laserPointers)
            {
                laser.SetActive(true);
            }
            
                    
             
            if (leftHand.gameObject.active && leftHand.GetComponent<Valve.VR.InteractionSystem.Hand>().grabPinchAction.stateDown)
            {
                if (Physics.Raycast(leftHand.position, leftHand.forward, out Hitboi, Mathf.Infinity, clusterMask))
                {
                    selectCluster(Hitboi.collider.transform);
                }
            }
            if (rightHand.gameObject.active && rightHand.GetComponent<Valve.VR.InteractionSystem.Hand>().grabPinchAction.stateDown)
            {
                if (Physics.Raycast(rightHand.position, rightHand.forward, out Hitboi, Mathf.Infinity, clusterMask))
                {
                    selectCluster(Hitboi.collider.transform);
                }
            }

        }
        else
        {
            selectedDisplay.SetActive(false);
            selected = null;
            foreach (GameObject laser in laserPointers)
            {
                laser.SetActive(false);
            }
        }
        
    }
    public void Travel()
    {
        if (selected&&!moving) {
            
            StopAllCoroutines();
            StartCoroutine(moveTowards(selected.transform.position));
        }
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void quit()
    {
        Application.Quit();
    }
    private void selectCluster(Transform target)
    {
        selectedDisplay.SetActive(true);
        var d = Vector3.Distance(target.position, playerTransform.position);
        
        DistanceText.text = "Distance: " + d.ToString("F1") + " ly";
        
        VelocityText.text = "Velocity: " + (d * (1 + Random.RandomRange(-0.05f, 0.05f))).ToString("F1") + " km/s";
        NameText.text = "            to " + target.gameObject.name;
        selected = target.gameObject;
    }
    IEnumerator moveTowards(Vector3 destination )
    {
        foreach (GameObject laser in laserPointers)
        {
            laser.SetActive(false);
        }
        moving = true;
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
        moving = false;
        foreach (GameObject laser in laserPointers)
        {
            laser.SetActive(true);
        }
    }
}
