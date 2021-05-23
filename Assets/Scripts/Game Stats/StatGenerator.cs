using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatScalingTier : byte {
	None = byte.MaxValue,
	A = 0, B, C, D
}

// represents the stats contained within the character base stats class
public enum CharacterStatType : byte {
	None = byte.MaxValue,
	MaxHitpoints = 0,
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

	//[System.Serializable]
	//struct DualInt {
	//	public int min;
	//	public int max;
	//	public int RandomRange() {
	//		return Random.Range(min, max);
	//	}
	//}

	[Header("Character Stats")]

	[SerializeField] private Vector2Int m_characterStatDistribution;
	[SerializeField] private Vector2Int m_characterStatDistributeRange;
	[SerializeField] private Vector2Int m_baseMaxHitpoints;
	[SerializeField] private Vector2Int m_baseStrength;
	[SerializeField] private Vector2Int m_basePercision;
	[SerializeField] private Vector2Int m_baseTechsavvy;
	[SerializeField] private Vector2Int m_baseScience;
	[SerializeField] private Vector2Int m_baseWillpower;
	[SerializeField] private Vector2Int m_baseLuck;
	[SerializeField] private Vector2Int m_baseStamina;

	[Header("Weapon Base Stats")]

	//[SerializeField] private Vector2Int m_weaponStatDistribution;
	//[SerializeField] private Vector2Int m_weaponStatDistributeRange;
	[SerializeField] private Vector2Int m_baseDamage;
	[SerializeField] private Vector2Int m_baseFirerate;
	[SerializeField] private Vector2Int m_baseMagsize;
	[SerializeField] private int m_magIterSize;

	[Header("Tier Values")]

	[SerializeField] private float m_tierAScalar;
	[SerializeField] private float m_tierBScalar;
	[SerializeField] private float m_tierCScalar;
	[SerializeField] private float m_tierDScalar;

	[Space]
	[SerializeField] private WeaponBaseStats m_basicWeapon = WeaponBaseStats.Null;
	public WeaponBaseStats BasicWeapon => m_basicWeapon;

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

	// random range shortcut
	private int RandomRange(Vector2Int range) {
		return Random.Range(range.x, range.y);
	}

	// generates a new player based on the given seed
	// not giving a seed will generate a random seed before calculating the player
	public CharacterStats GenerateCharacter(int seed = int.MaxValue) {
		// generate random seed
		if (seed == int.MaxValue) RandomizeSeed();
		// set seed
		else Random.InitState(seed);

		// create instance
		CharacterStats cs = CharacterStats.Null;

		// set health
		cs.maxHitpoints = RandomRange(m_baseMaxHitpoints);

		// set base stats
		cs.strength = RandomRange(m_baseStrength);
		cs.percision = RandomRange(m_basePercision);
		cs.techsavvy = RandomRange(m_baseTechsavvy);
		cs.science = RandomRange(m_baseScience);
		cs.willpower = RandomRange(m_baseWillpower);
		cs.luck = RandomRange(m_baseLuck);
		cs.stamina = RandomRange(m_baseStamina);

		// stat distribution system
		int begin = 1;
		int end = cs.Count;

		int points = RandomRange(m_characterStatDistribution);
		int average = points / (end - begin);
		int remaining = points % (end - begin);
		points -= remaining;

		// start distributing points
		for (int i = begin; (i < end) && (points > 0); i++) {
			// add random range of points to stat
			int addedpoints = average + RandomRange(m_characterStatDistributeRange);
			if (addedpoints < 0) addedpoints = 0;
			if (addedpoints > points) addedpoints = points;
			cs[i] = cs[i] + addedpoints;
			if ((i + 1) == end) cs[i] = cs[i] + remaining;

			// remove points and recalculate average
			points -= addedpoints;
			average = points / (end - i);
		}

		// return
		return cs;
	}

	// generates a new weapon based on the given  tier and seed
	public WeaponBaseStats GenerateWeapon(int tier = int.MaxValue, int seed = int.MaxValue) {
		// randomize tier
		if (tier == int.MaxValue) {
			tier = Random.Range(1, 4);
		}

		// generate random seed
		if (seed == int.MaxValue) RandomizeSeed();
		// set seed
		else Random.InitState(seed);

		// create instance
		WeaponBaseStats ws = WeaponBaseStats.Null;

		// set stats
		ws.firerate.baseValue = RandomRange(m_baseFirerate);
		int damage = RandomRange(m_baseDamage) / 2;
		if (damage == 0) damage++;
		ws.damage.baseValue = damage + Mathf.FloorToInt(damage *
			(1f - ((float)ws.firerate.baseValue / (float)m_baseFirerate.y)));

		Vector2Int magsize = m_baseMagsize;
		magsize.x /= m_magIterSize;
		magsize.y /= m_magIterSize;
		ws.magsize = RandomRange(magsize) * m_magIterSize;

		// stat distribution system
		//int points = RandomRange(m_weaponStatDistribution);
		//int first = (points / 2) + RandomRange(m_weaponStatDistributeRange);
		//if (first > points) first = points;
		//if (first < 0) first = 0;
		//ws.damage.baseValue = ws.damage.baseValue + first;
		//ws.firerate.baseValue = ws.firerate.baseValue + (points - first);

		// tiers
		while (tier > 0) {
			int random = Random.Range(0, 2);

			if (random == 0) {
				CreateTierValue(ref ws.damage);
			} else {
				CreateTierValue(ref ws.firerate);
			}

			tier--;
		}

		// return
		return ws;
	}

	private void CreateTierValue(ref WeaponBaseStats.StatScalars ss) {
		StatScalingTier sst = (StatScalingTier)Random.Range((int)StatScalingTier.A, (int)StatScalingTier.D);
		CharacterStatType cst = (CharacterStatType)Random.Range((int)CharacterStatType.Strength, (int)CharacterStatType.Stamina);

		// add first tier
		if (ss.firstTier == StatScalingTier.None) {
			ss.firstTier = sst;
			ss.firstStat = cst;
		}
		// add second tier
		else if (ss.secondTier == StatScalingTier.None) {
			ss.secondTier = sst;
			ss.secondStat = cst;
		}
		// add third tier
		else if (ss.thirdTier == StatScalingTier.None) {
			ss.thirdTier = sst;
			ss.thirdStat = cst;
		}
		// cant add fourth tier
	}

	public string GenerateDescription(CharacterStats cs) {
		string desc;
		for (int i = (int)CharacterStatType.MaxHitpoints; i < cs.Count; i++) {
			desc += $"{(CharacterStatType)i}: {cs[i]}\n";
		}
		return desc;
	}

	public static string GetShortendName(CharacterStatType cst) {
		switch (cst) {
			case CharacterStatType.MaxHitpoints: return "MHP";
			case CharacterStatType.Strength: return "STR";
			case CharacterStatType.Percision: return "PRC";
			case CharacterStatType.Techsavvy: return "TCS";
			case CharacterStatType.Science: return "SCI";
			case CharacterStatType.Willpower: return "WIL";
			case CharacterStatType.Luck: return "LCK";
			case CharacterStatType.Stamina: return "STA";
			default: break;
		}
		return null;
	}

	private static void RandomizeSeed() {
		Random.InitState(Time.frameCount + Random.Range(int.MinValue, int.MaxValue));
	}
}
