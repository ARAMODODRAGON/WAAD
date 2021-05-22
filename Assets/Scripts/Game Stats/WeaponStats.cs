using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponStats {

	// weapon stats

	public int damage;
	public int firerate;
	public int magsize;

	// initialize a struct with no value
	public static WeaponStats Null {
		get {
			WeaponStats ps;
			ps.damage = 0;
			ps.firerate = 0;
			ps.magsize = 0;
			return ps;
		}
	}

}
