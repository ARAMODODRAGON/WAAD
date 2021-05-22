using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTest : MonoBehaviour {

	[SerializeField] private StatGenerator m_statGenerator;

	public CharacterStats charStats;
	public WeaponBaseStats weaponBaseStats;
	public WeaponStats weaponStats;

	private void Awake() {
		charStats = m_statGenerator.GenerateCharacter();
		//weaponBaseStats = StatGenerator.GenerateWeapon();
		weaponStats = m_statGenerator.Calculate(charStats, weaponBaseStats);
	}
}
