using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionCompBase : MonoBehaviour
{

    private LevelBase level = null;
    private GameObject target;
    public float attackRadius = 0.0f; //How far away from the player should you before you start shooting
    public int fireRate = 0; //How many bullets to shoot before stopping
    public float reloadTime = 0.0f; //How long to wait before shooting again
    public void UpdateLevel(LevelBase level_)
    {
        level = level_;
    }

    public bool IsDestinationWithinRange(Vector2 dest_)
    {
        //Make sure whatever the desitnation is, it is within the room boundaries
        if(level)
        {
            if (dest_.x >= level.min.transform.position.x && dest_.y >= level.min.transform.position.y
                && dest_.x <= level.max.transform.position.x && dest_.y <= level.max.transform.position.y)
                return true;
        }
        return false;
    }
}
