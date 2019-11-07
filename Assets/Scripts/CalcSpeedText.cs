using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalcSpeedText : MonoBehaviour
{
    public TextMeshProUGUI SpeedText;
    public float SPEED = 0.05f;
    public Transform target;
    public float scaleFactor;
    void Start()
    {
        target = ClusterTravel.player;
        SpeedText.enabled = false;
    }
    public void ToggleText()
    {
        SpeedText.enabled = !SpeedText.enabled;
    }

    private void Update()
    {

    }

    IEnumerator CalcVelocity() // The CalcVelocity coroutine
    {
        var lastDist= (transform.position - target.position).magnitude;
        while (Application.isPlaying) // While game is running
        {



            // calculates the speed by finding the absoluate value between previous postion
            // and current position, and then dividing by the change in time between frames
            var currDist = (transform.position - target.position).magnitude;
            var currVel = currDist - lastDist;
            lastDist = currDist;

            SpeedText.text = currVel.ToString("F2"); // turns number to string
            SpeedText.transform.parent.LookAt(target);
            SpeedText.transform.parent.localScale = scaleFactor * currDist*Vector3.one;
            yield return new WaitForSeconds(1); // 
        }
    }
    public void StartShowText()
    {
        print("THIS IS HAPPENING WAY TOO MUCH");
        StartCoroutine(CalcVelocity());
        SpeedText.enabled = true;
    }
    public void StopShowText()
    {
        StopAllCoroutines();
        SpeedText.enabled = false;
    }
    //void OnMouseOver()
    //{
    //    Debug.Log("Mouse is over GameObject");  
    //}
    // void OnMouseExit()
    //{
    //    Debug.Log("Mouse is no longer on GameObject");
    //}
}