using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

namespace ECS.Jobs{
    
    public struct ExpanderJob : IJobParallelForTransform{

        public float deltaTime;
        public Vector3 direction;

        public void Execute(int index, TransformAccess transform){
            Vector3 pos = transform.position;
            pos += transform.position.normalized * 2f * deltaTime;

            transform.position = pos;
        }
    }

    public class ExpanderManager : MonoBehaviour{
        
        ExpanderJob expanderJobs;
        TransformAccessArray transforms;
        JobHandle expanderHandle;
        public Generator gen;

        void Start(){
            transforms = new TransformAccessArray(0, -1);
            SetStars();
        }

        void Update(){
            expanderHandle.Complete();
            

            ExpanderJob job = new ExpanderJob(){
                deltaTime = Time.deltaTime,
                direction = Vector3.up
            };

            expanderHandle = job.Schedule(transforms);
            JobHandle.ScheduleBatchedJobs();
        }

        void SetStars(){
            expanderHandle.Complete();
            transforms.capacity = gen.currentUniverse.clusters.Count;
            transforms.SetTransforms(gen.currentUniverse.clusters.ToArray());
        }

    }
}

