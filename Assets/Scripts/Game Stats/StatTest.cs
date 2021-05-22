using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTest : MonoBehaviour {

	[SerializeField] private StatGenerator m_statGenerator;

	public bool regenerate = false;
	[Space]
	public bool lockSeed;
	public int seed;
	public CharacterStats charStats;
	[Space]
	public bool randomizeWeaponTier;
	[Range(1, 3)] public int weaponTier = 1;
	public WeaponBaseStats weaponBaseStats;
	[Space]
	public WeaponStats weaponStats;

	private void Calc() {
		int s = int.MaxValue;
		if (lockSeed) s = seed;
		else s = seed = Time.frameCount + Random.Range(int.MinValue, int.MaxValue);
		charStats = m_statGenerator.GenerateCharacter(s);
		int tier = weaponTier;
		if (randomizeWeaponTier) tier = weaponTier = Random.Range(1, 4);
		weaponBaseStats = m_statGenerator.GenerateWeapon(tier, s);
		weaponStats = m_statGenerator.Calculate(charStats, weaponBaseStats);
	}

	private void Awake() {
		Calc();
		regenerate = false;
	}

	private void Update() {
		if (regenerate) {
			regenerate = false;
			Calc();
		}
	}
}
