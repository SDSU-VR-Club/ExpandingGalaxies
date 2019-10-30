using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterTravel : MonoBehaviour
{
    public float travelSpeed;
    Collider lastCol;
    public Transform player;
    public Camera BackupCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (false)
        {
            RaycastHit HitBoi;
            
            if (Physics.Raycast(transform.position,transform.forward, out HitBoi,Mathf.Infinity))
            {

                followCluster(HitBoi);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit HitBoi;
            print("click");
            
            Ray LASERBOI = BackupCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(LASERBOI, out HitBoi))
            {
                print(HitBoi.collider.gameObject.name);
                followCluster(HitBoi);
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
    }
}
