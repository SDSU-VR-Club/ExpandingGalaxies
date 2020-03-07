using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class MovementLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.localPosition = -UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.CenterEye);
    }
}
