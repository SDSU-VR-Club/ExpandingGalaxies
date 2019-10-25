using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalcSpeedText : MonoBehaviour
{
    public TextMeshProUGUI SpeedText;
    public float SPEED = 0.05f;

    void Start()
    {
        StartCoroutine(CalcVelocity()); // Begins the coroutine
    }
    public void ToggleText()
    {
        SpeedText.enabled = !SpeedText.enabled;
    }

    private void Update()
    {
        transform.Translate(0, 0, SPEED); // moving gameobject

    }

    IEnumerator CalcVelocity() // The CalcVelocity coroutine
    {
        while (Application.isPlaying) // While game is running
        {
            var prevPos = transform.position; // creating a variable to hold previous position coordinates
            yield return new WaitForEndOfFrame(); // 
            var currVel = (prevPos - transform.position).magnitude / Time.deltaTime;
            // calculates the speed by finding the absoluate value between previous postion
            // and current position, and then dividing by the change in time between frames

            //Debug.Log(currVel);
            SpeedText.text = currVel.ToString(); // turns number to string
        }
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