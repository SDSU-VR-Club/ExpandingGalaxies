using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit HitBoi;
            Debug.Log("C L I C K");
            Ray LASERBOI = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(LASERBOI, out HitBoi))
            {
                Debug.Log("BANG (hit) " + HitBoi.collider.gameObject.name);
                HitBoi.collider.gameObject.GetComponent<CalcSpeedText>().ToggleText();
            }
        }
    }
}
