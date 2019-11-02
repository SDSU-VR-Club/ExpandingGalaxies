using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;
public class ClusterTravel : MonoBehaviour
{
    public float travelSpeed;
    Collider lastCol;
    public Transform player;
    public Camera BackupCam;
    public Transform leftHand, rightHand;
    public LineRenderer leftRenderer, rightRenderer;
    public Color highlightColor;
    GameObject leftHighlighted;
    GameObject rightHighlighted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (leftHand.gameObject.active&&leftHand.GetComponent<Hand>().grabPinchAction.stateDown)
        {
            RaycastHit HitBoi;
            
            if (Physics.Raycast(leftHand.position,leftHand.forward, out HitBoi,Mathf.Infinity))
            {

                followCluster(HitBoi);
            }
        }
        else if(leftHand.gameObject.active)
        {
            RaycastHit HitBoi;

            if (Physics.Raycast(leftHand.position, leftHand.forward, out HitBoi, Mathf.Infinity))
            {
                HitBoi.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                if(leftHighlighted&&leftHighlighted!=HitBoi.collider.gameObject)
                    leftHighlighted.GetComponent<MeshRenderer>().material.color = Color.white;
                leftHighlighted = HitBoi.collider.gameObject;
                leftRenderer.SetColors(highlightColor, highlightColor);

            }
            else
            {
                leftRenderer.SetColors(Color.white, Color.white);
            }
        }
        if (rightHand.gameObject.active && rightHand.GetComponent<Hand>().grabPinchAction.stateDown)
        {
            RaycastHit HitBoi;

            if (Physics.Raycast(rightHand.position, rightHand.forward, out HitBoi, Mathf.Infinity))
            {

                followCluster(HitBoi);
            }
        }
        else if (rightHand.gameObject.active)
        {
            RaycastHit HitBoi;

            if (Physics.Raycast(rightHand.position, rightHand.forward, out HitBoi, Mathf.Infinity))
            {
                HitBoi.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                if(rightHighlighted && rightHighlighted != HitBoi.collider.gameObject)
                    rightHighlighted.GetComponent<MeshRenderer>().material.color = Color.white;
                rightHighlighted = HitBoi.collider.gameObject;
                rightRenderer.SetColors(highlightColor, highlightColor);

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

            if (Physics.Raycast(LASERBOI, out HitBoi))
            {
                //print(HitBoi.collider.gameObject.name);
                followCluster(HitBoi);
            }
        }
        else if(BackupCam.gameObject.active)
        {
            RaycastHit HitBoi;

            Ray LASERBOI = BackupCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(LASERBOI, out HitBoi))
            {
                HitBoi.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                if(rightHighlighted && rightHighlighted != HitBoi.collider.gameObject)
                    rightHighlighted.GetComponent<MeshRenderer>().material.color = Color.white;
                rightHighlighted = HitBoi.collider.gameObject;
                rightRenderer.SetColors(highlightColor, highlightColor);

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
    }
   
}
