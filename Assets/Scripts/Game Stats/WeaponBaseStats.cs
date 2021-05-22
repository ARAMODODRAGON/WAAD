using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponBaseStats {

	// stats

	public StatScalars damage;
	public StatScalars firerate;
	public StatScalars magsize;

	// zero initialized struct
	public static WeaponBaseStats Null {
		get {
			WeaponBaseStats ws;
			ws.damage = StatScalars.Null;
			ws.firerate = StatScalars.Null;
			ws.magsize = StatScalars.Null;
			return ws;
		}
	}

	// struct to represent the additional stats to scale with this weapon
	[System.Serializable]
	public struct StatScalars {

		public int baseValue;

		public StatScalingTier firstTier;
		public CharacterStatType firstStat;

		public StatScalingTier secondTier;
		public CharacterStatType secondStat;

		public StatScalingTier thirdTier;
		public CharacterStatType thirdStat;

		// zero initialized struct
		public static StatScalars Null {
			get {
				StatScalars ss;
				ss.baseValue = 0;
				ss.firstTier = StatScalingTier.None;
				ss.firstStat = CharacterStatType.None;
				ss.secondTier = StatScalingTier.None;
				ss.secondStat = CharacterStatType.None;
				ss.thirdTier = StatScalingTier.None;
				ss.thirdStat = CharacterStatType.None;
				return ss;
			}
		}

		// calculating based off the given player stat
		// requires 'cs' not be null
		public int Value(CharacterStats cs) {

			// add base value
			int value = baseValue;

			// calculate each stat
			value += Mathf.CeilToInt(cs[firstStat] * StatGenerator.TierValue(firstTier));
			value += Mathf.CeilToInt(cs[secondStat] * StatGenerator.TierValue(secondTier));
			value += Mathf.CeilToInt(cs[thirdStat] * StatGenerator.TierValue(thirdTier));

			// return
			return value;
		}

		// implicit cast that returns the base value
		public static implicit operator int(StatScalars ss) => ss.baseValue;

	}

}
