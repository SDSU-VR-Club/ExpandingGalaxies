﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;
public class ClusterTravel : MonoBehaviour
{
    public float travelSpeed;
    Collider lastCol;
    public static Transform player;
    public Transform startingPlayer;
    public Camera BackupCam;
    public Transform leftHand, rightHand;
    public LineRenderer leftRenderer, rightRenderer;
    public Color highlightColor;
    GameObject leftHighlighted;
    GameObject rightHighlighted;
    public LayerMask clusterMask;
    // Start is called before the first frame update
    void Start()
    {
        player = startingPlayer;
    }

    // Update is called once per frame
    void Update()
    {

        if (leftHand.gameObject.active&&leftHand.GetComponent<Hand>().grabPinchAction.stateDown)
        {
            RaycastHit HitBoi;
            
            if (Physics.Raycast(leftHand.position,leftHand.forward, out HitBoi,Mathf.Infinity,clusterMask))
            {

                followCluster(HitBoi);
            }
        }
        else if(leftHand.gameObject.active)
        {
            RaycastHit HitBoi;

            if (Physics.Raycast(leftHand.position, leftHand.forward, out HitBoi, Mathf.Infinity,clusterMask))
            {
                if (HitBoi.collider.gameObject != leftHighlighted)
                {
                    HitBoi.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    if (leftHighlighted && leftHighlighted != HitBoi.collider.gameObject)
                    {
                        leftHighlighted.GetComponent<MeshRenderer>().material.color = Color.white;
                        leftHighlighted.GetComponent<CalcSpeedText>().StopShowText();
                    }
                    leftHighlighted = HitBoi.collider.gameObject;
                    leftHighlighted.GetComponent<CalcSpeedText>().StartShowText();
                    leftRenderer.SetColors(highlightColor, highlightColor);
                }
            }
            else
            {
                leftRenderer.SetColors(Color.white, Color.white);
            }
        }
        if (rightHand.gameObject.active && rightHand.GetComponent<Hand>().grabPinchAction.stateDown)
        {
            RaycastHit HitBoi;

            if (Physics.Raycast(rightHand.position, rightHand.forward, out HitBoi, Mathf.Infinity,clusterMask))
            {

                followCluster(HitBoi);
            }
        }
        else if (rightHand.gameObject.active)
        {
            RaycastHit HitBoi;

            if (Physics.Raycast(rightHand.position, rightHand.forward, out HitBoi, Mathf.Infinity,clusterMask))
            {
                if (HitBoi.collider.gameObject != rightHighlighted)
                {
                    HitBoi.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    if (rightHighlighted && rightHighlighted != HitBoi.collider.gameObject)
                    {
                        rightHighlighted.GetComponent<MeshRenderer>().material.color = Color.white;
                        rightHighlighted.GetComponent<CalcSpeedText>().StopShowText();
                    }
                    rightHighlighted = HitBoi.collider.gameObject;
                    rightHighlighted.GetComponent<CalcSpeedText>().StartShowText();
                    rightRenderer.SetColors(highlightColor, highlightColor);
                }
            }
            else
            {
                rightRenderer.SetColors(Color.white, Color.white);
            }
        
        }
        if (BackupCam.enabled&& Input.GetMouseButtonDown(0))
        {
            RaycastHit HitBoi;
            
            Ray LASERBOI = BackupCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(LASERBOI, out HitBoi,clusterMask))
            {
                //print(HitBoi.collider.gameObject.name);
                followCluster(HitBoi);
            }
        }
        else if(BackupCam.gameObject.active)
        {
            RaycastHit HitBoi;

            Ray LASERBOI = BackupCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(LASERBOI, out HitBoi,clusterMask))
            {
                if (HitBoi.collider.gameObject != rightHighlighted)
                {
                    HitBoi.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    if (rightHighlighted && rightHighlighted != HitBoi.collider.gameObject)
                    {
                        rightHighlighted.GetComponent<MeshRenderer>().material.color = Color.white;
                        rightHighlighted.GetComponent<CalcSpeedText>().StopShowText();
                    }
                    rightHighlighted = HitBoi.collider.gameObject;
                    rightHighlighted.GetComponent<CalcSpeedText>().StartShowText();
                    rightRenderer.SetColors(highlightColor, highlightColor);
                }
            }
            else
            {
                rightRenderer.SetColors(Color.white, Color.white);
            }
        }
        player.localPosition = Vector3.Lerp(player.localPosition, Vector3.zero, travelSpeed);
    }
    void followCluster(RaycastHit HitBoi)
    {
        if (lastCol)
            lastCol.enabled = true;
        HitBoi.collider.enabled = false;
        lastCol = HitBoi.collider;
        player.parent = HitBoi.collider.transform;
        AudioScript.instance.PlaySFX();
        HitBoi.collider.gameObject.GetComponent<CalcSpeedText>().StartShowText();
    }
   
}
