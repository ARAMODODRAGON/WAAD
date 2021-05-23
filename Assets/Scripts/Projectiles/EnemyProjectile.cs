using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectileBase
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null && col != this)
        {
            if (col.CompareTag("Player"))
            {
                PlayerController pc_ = col.GetComponent<PlayerController>();
                if (pc_)
                {
                    //TODO
                    //Inflict damage
                }
            }
        }
    }
}
