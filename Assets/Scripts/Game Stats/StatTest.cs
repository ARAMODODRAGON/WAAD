using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTest : MonoBehaviour {

	public CharacterStats charStats;
	public WeaponBaseStats weaponBaseStats;
	public WeaponStats weaponStats;

	private void Awake() {
		charStats = StatGenerator.GenerateCharacter();
		weaponBaseStats = StatGenerator.GenerateWeapon();
		weaponStats = StatGenerator.Calculate(charStats, weaponBaseStats);
	}
}
