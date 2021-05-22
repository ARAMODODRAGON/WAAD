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

[CreateAssetMenu(fileName = "Stat Generator", menuName = "Stat Generator")]
public class StatGenerator : ScriptableObject {

	[Header("Character Stats")]

	[SerializeField] private int m_baseMaxHitpoints;

	[Header("Weapon Base Stats")]

	[SerializeField] private int m_baseDamage;
	[SerializeField] private int m_baseFirerate;
	[SerializeField] private int m_baseMagsize;

	[Header("Tier Values")]

	[SerializeField] private float m_tierAScalar;
	[SerializeField] private float m_tierBScalar;
	[SerializeField] private float m_tierCScalar;
	[SerializeField] private float m_tierDScalar;

	// get the scalar value for the given tier
	public float TierValue(StatScalingTier tier) {
		switch (tier) {
			case StatScalingTier.None: return 0f;
			case StatScalingTier.A: return m_tierAScalar;
			case StatScalingTier.B: return m_tierBScalar;
			case StatScalingTier.C: return m_tierCScalar;
			case StatScalingTier.D: return m_tierDScalar;
			default: throw new System.IndexOutOfRangeException();
		}
	}

	// calculating based off the given player stat
	// requires 'cs' not be null
	private int StatScalarValue(WeaponBaseStats.StatScalars ss, CharacterStats cs) {

		// add base value
		int value = ss.baseValue;

		// calculate each stat
		value += Mathf.FloorToInt(cs[ss.firstStat] * TierValue(ss.firstTier));
		value += Mathf.FloorToInt(cs[ss.secondStat] * TierValue(ss.secondTier));
		value += Mathf.FloorToInt(cs[ss.thirdStat] * TierValue(ss.thirdTier));

		// return
		return value;
	}

	// generates a FinalStats struct based off the given character and weapon stats
	public WeaponStats Calculate(CharacterStats cs, WeaponBaseStats ws) {
		// create
		WeaponStats ps = WeaponStats.Null;

		// calculate stats
		ps.damage = StatScalarValue(ws.damage, cs);
		ps.firerate = StatScalarValue(ws.firerate, cs);
		ps.magsize = ws.magsize;

		// return
		return ps;
	}

	// generates a new player based on the given seed
	// not giving a seed will generate a random seed before calculating the player
	public CharacterStats GenerateCharacter(int seed = int.MaxValue) {
		// generate random seed
		if (seed == int.MaxValue) RandomizeSeed();
		// set seed
		else Random.InitState(seed);

		// create instance
		CharacterStats cs = new CharacterStats();

		// set health
		cs.maxHitpoints = Random.Range(80, 120);

		// set base stats
		cs.strength = Random.Range(1, 20);
		cs.percision = Random.Range(1, 20);
		cs.techsavvy = Random.Range(1, 20);
		cs.science = Random.Range(1, 20);
		cs.willpower = Random.Range(1, 20);
		cs.luck = Random.Range(1, 20);
		cs.stamina = Random.Range(1, 20);

		// TODO: stat distribution system
		Debug.LogWarning("No stat distributions");

		// return
		return cs;
	}

	// generates a new weapon based on the given  tier and seed
	public WeaponBaseStats GenerateWeapon(int tier = 1, int seed = int.MaxValue) {
		// generate random seed
		if (seed == int.MaxValue) RandomizeSeed();
		// set seed
		else Random.InitState(seed);

		// create instance
		WeaponBaseStats ws = new WeaponBaseStats();

		// set stats
		ws.damage.baseValue = 1;
		ws.firerate.baseValue = 1;
		ws.magsize = 1;

		// TODO: stat distribution system
		Debug.LogWarning("No stat distributions");

		// return
		return ws;
	}

	private static void RandomizeSeed() {
		Random.InitState(Time.frameCount + Random.Range(int.MinValue, int.MaxValue));
	}
}
