using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    public class timeSlider : MonoBehaviour
    {
        public LinearMapping linearMapping;
        public Text timeScaleText;

        private float currentLinearMapping = float.NaN;
        private int framesUnchanged = 0;


        //-------------------------------------------------
        void Awake()
        {
            
            if (linearMapping == null)
            {
                linearMapping = GetComponent<LinearMapping>();
            }
        }


        //-------------------------------------------------
        void Update()
        {
            if (currentLinearMapping != linearMapping.value)
            {
                currentLinearMapping = linearMapping.value;
                timeScaleText.text = string.Format("{0:N2}", linearMapping.value);
                //Time.timeScale = linearMapping.value;
                FindObjectOfType<ExpansionManager>().speed = linearMapping.value * 2 - 1;
                framesUnchanged = 0;
            }
            else
            {
                framesUnchanged++;
                if (framesUnchanged > 2)
                {
                    //animator.enabled = false;
                    ;
                }
            }
        }
    }
}
