using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace SpaceECS.Hybrid
{
    [RequireComponent(typeof(GameObjectEntity))]
    public class Expander : MonoBehaviour
    {

    }

    class ExpanderSystem : ComponentSystem{
        protected override void OnUpdate(){
            float deltaTime = Time.deltaTime;
            Entities.ForEach((Transform transform) =>{
                transform += Vector3.normalize(transform) * 10 * deltaTime;
            });
        }
    }
}