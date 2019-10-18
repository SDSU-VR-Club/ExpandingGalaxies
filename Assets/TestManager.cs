using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public Transform cube;
    public bool moveBackward;

    // Start is called before the first frame update
    void Start()
    {
        moveBackward = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveBackward && cube.position.z < 6)
        {
            cube.Translate(cube.position.x, cube.position.y, cube.position.z + 0.5f);
        }
        else if(moveBackward && cube.position.z >= 6)
        {
            moveBackward = false;
        }
        else if(!moveBackward && cube.position.z > 3)
        {
            cube.Translate(cube.position.x, cube.position.y, cube.position.z - 0.5f);
        }
        else if (!moveBackward && cube.position.z <= 3)
        {
            moveBackward = true;
        }
    }
}
