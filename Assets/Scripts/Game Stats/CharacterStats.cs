using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharacterStats {

	// effective stats

	public int maxHitpoints;

	// ineffective stats

	[Space(3f)]
	public int strength;
	public int percision;
	public int techsavvy;
	public int science;
	public int willpower;
	public int luck;
	public int stamina;

	// zero initialized struct
	public static CharacterStats Null {
		get {
			CharacterStats cs;
			cs.maxHitpoints = 0;
			cs.strength = 0;
			cs.percision = 0;
			cs.techsavvy = 0;
			cs.science = 0;
			cs.willpower = 0;
			cs.luck = 0;
			cs.stamina = 0;
			return cs;
		}
	}

	// the number of stats
	public int Count => 8;

	// index accessor
	// accesses based off the enum order
	public int this[int index] {
		get => this[(CharacterStatType)index];
		set => this[(CharacterStatType)index] = value;
	}

	// index accessor based off enum
	public int this[CharacterStatType index] {
		get {
			switch (index) {
				case CharacterStatType.None: return -1;
				case CharacterStatType.MaxHitpoints: return maxHitpoints;
				case CharacterStatType.Strength: return strength;
				case CharacterStatType.Percision: return percision;
				case CharacterStatType.Techsavvy: return techsavvy;
				case CharacterStatType.Science: return science;
				case CharacterStatType.Willpower: return willpower;
				case CharacterStatType.Luck: return luck;
				case CharacterStatType.Stamina: return stamina;
				default: throw new System.IndexOutOfRangeException();
			}
		}
		set {
			switch (index) {
				case CharacterStatType.None: break;
				case CharacterStatType.MaxHitpoints: maxHitpoints = value; break;
				case CharacterStatType.Strength: strength = value; break;
				case CharacterStatType.Percision: percision = value; break;
				case CharacterStatType.Techsavvy: techsavvy = value; break;
				case CharacterStatType.Science: science = value; break;
				case CharacterStatType.Willpower: willpower = value; break;
				case CharacterStatType.Luck: luck = value; break;
				case CharacterStatType.Stamina: stamina = value; break;
				default: throw new System.IndexOutOfRangeException();
			}
		}
	}

}
