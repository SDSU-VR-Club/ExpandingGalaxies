using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPlayer : MonoBehaviour
{

    public ChunkManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<ChunkManager>();
        this.transform.parent = manager.transform;
    }

    // Update is called once per frame
    void Update()
    {
        LoopPosition();
    }

    //In order to loop the position correctly we need to stay positive
    //The reason why is that the % operator will always return 0 when
    //When we reach our max value. Since we have a width of Time, max value is time/2
    //  So instead of
    //      time/2 => 0 and -time/2 => 0
    //  We want
    //      time/2 => -time/2 and -time/2 => time/2

    //We can do this by changing our bounds
    //  So instead of having 
    //      -time/2 <-> time/2
    //  we would have
    //      0 <-> time

    //This would give us the ability to use the % operator
    //  So 
    //      0 % time is => 0
    //  and 
    //      time % time => 0
    
    private Vector3 last;
    void LoopPosition(){
        Vector3 pos = this.transform.localPosition;
        pos = new Vector3(pos.x + manager.time/2, pos.y + manager.time/2, pos.z + manager.time/2); //Change the bounds
        pos = new Vector3(mod(pos.x, manager.time), mod(pos.y, manager.time), mod(pos.z, manager.time)); //Using % operator
        pos = new Vector3(pos.x - manager.time/2, pos.y - manager.time/2, pos.z - manager.time/2); //Change back the bounds
        
        //Check if our diff is around the size of "time" with the sign being directionality
        Vector3Int diff = Vector3Int.RoundToInt((pos - last) / manager.time);
        if(diff != Vector3Int.zero){
            manager.ShiftChunk(diff);
        }

        this.transform.localPosition = pos;
        last = pos;
    }
    
    //Source https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
    float mod(float x, float m) {
        float r = x%m;
        return r<0 ? r+m : r;
    }
}
