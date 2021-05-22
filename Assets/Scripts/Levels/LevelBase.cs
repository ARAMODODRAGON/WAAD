using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    //Referenced by the AI to know the limits of a room
    public GameObject min;
    public GameObject max;
    //Typically the center of the room
    public GameObject cameraPosition;

    public bool IsDestinationWithinRange(Vector2 dest_)
    {
        //Make sure whatever the desitnation is, it is within the room boundaries
            if (dest_.x >= min.transform.position.x && dest_.y >= min.transform.position.y
                && dest_.x <= max.transform.position.x && dest_.y <= max.transform.position.y)
                return true;
        
        return false;
    }
}


