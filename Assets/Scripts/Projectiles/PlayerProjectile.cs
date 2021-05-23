using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : ProjectileBase
{
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col != null && col != this)
		{
			if (col.CompareTag("Enemy"))
			{
				EnemyBase em = col.GetComponent<EnemyBase>();
				if (em != null)
				{
					em.TakeDamage(damage);
					Destroy(gameObject);
				}
			}
		}
	}
}
