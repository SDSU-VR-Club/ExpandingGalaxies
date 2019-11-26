using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace ECS.Hybrid
{
    public class Expander : MonoBehaviour
    {}

    class ExpanderSystem : ComponentSystem{
        protected override void OnUpdate(){
            float deltaTime = Time.deltaTime;
            Entities.ForEach((Expander e, Transform transform) =>{
                transform.position += transform.position.normalized * 2 * deltaTime;
            });
        }
    }
}
