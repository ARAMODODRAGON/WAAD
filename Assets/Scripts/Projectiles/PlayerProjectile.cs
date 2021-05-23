using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : ProjectileBase
{
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy") && col != null && col != this)
		{
			EnemyBase em = col.GetComponent<EnemyBase>();
			if (em != null)
			{
				em.TakeDamage(damage);
			}
		}
	}
}
