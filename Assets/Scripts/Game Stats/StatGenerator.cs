using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatScalingTier : byte {
	None,
	A, B, C, D
}

// represents the stats contained within the character base stats class
public enum CharacterStatType : byte {
	None,
	MaxHitpoints,
	Strength,
	Percision,
	Techsavvy,
	Science,
	Willpower,
	Luck,
	Stamina
}

public static class StatGenerator {

	// get the scalar value for the given tier
	public static float TierValue(StatScalingTier tier) {
		switch (tier) {
			case StatScalingTier.None: return 0f;
			case StatScalingTier.A: return 0f;
			case StatScalingTier.B: return 0f;
			case StatScalingTier.C: return 0f;
			case StatScalingTier.D: return 0f;
			default: throw new System.IndexOutOfRangeException();
		}
	}

	// generates a FinalStats struct based off the given character and weapon stats
	public static WeaponStats Calculate(CharacterStats cs, WeaponBaseStats ws) {
		// create
		WeaponStats ps = WeaponStats.Null;

		// calculate stats
		ps.damage = ws.damage.Value(cs);
		ps.firerate = ws.firerate.Value(cs);
		ps.magsize = ws.magsize.Value(cs);

		// return
		return ps;
	}

	// generates a new player based on the given seed
	// not giving a seed will generate a random seed before calculating the player
	public static CharacterStats GeneratePlayer(int seed = int.MaxValue) {
		// generate random seed
		if (seed == int.MaxValue) RandomizeSeed();
		// set seed
		else Random.InitState(seed);

		// create instance
		CharacterStats cs = new CharacterStats();

		// set health
		cs.maxHitpoints = Random.Range(80, 120);

		// set base stats
		cs.strength = 1;
		cs.percision = 1;
		cs.techsavvy = 1;
		cs.science = 1;
		cs.willpower = 1;
		cs.luck = 1;
		cs.stamina = 1;

		// TODO: stat distribution system
		Debug.LogWarning("No stat distributions");

		// return
		return cs;
	}

	// generates a new weapon based on the given  tier and seed
	public static WeaponBaseStats GenerateWeapon(int tier = 1, int seed = int.MaxValue) {
		// generate random seed
		if (seed == int.MaxValue) RandomizeSeed();
		// set seed
		else Random.InitState(seed);

		// create instance
		WeaponBaseStats ws = new WeaponBaseStats();

		// set stats
		ws.damage.baseValue = 1;
		ws.firerate.baseValue = 1;
		ws.magsize.baseValue = 1;

		// TODO: stat distribution system
		Debug.LogWarning("No stat distributions");

		// return
		return ws;
	}

	private static void RandomizeSeed() {
		Random.InitState(Time.frameCount + Random.Range(int.MinValue, int.MaxValue));
	}
}


//// singleton pattern for scriptable objects
//public static StatGenerator Instance {
//	get {
//		if (!s_instance) s_instance = FindObjectOfType<StatGenerator>();
//		if (!s_instance) s_instance = CreateInstance<StatGenerator>();
//		return s_instance;
//	}
//}
//private static StatGenerator s_instance = null;
