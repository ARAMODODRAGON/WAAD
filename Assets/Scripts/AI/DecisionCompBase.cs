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


}
