using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScreen : MonoBehaviour {

	[SerializeField] private StatGenerator m_statGenerator = null;
	[SerializeField] private PlayerController m_player = null;

	[Header("UI Objects")]
	[Space]
	[SerializeField] private Button m_buttonA = null;
	[SerializeField] private Button m_buttonB = null;
	[SerializeField] private Button m_buttonC = null;
	[Space]
	[SerializeField] private Text m_textA = null;
	[SerializeField] private Text m_textB = null;
	[SerializeField] private Text m_textC = null;

	// characters
	private CharacterStats m_characterA = CharacterStats.Null;
	private CharacterStats m_characterB = CharacterStats.Null;
	private CharacterStats m_characterC = CharacterStats.Null;

	private void Awake() {
		// early exit
		if (!m_statGenerator || !m_player) {
			gameObject.SetActive(false);
			return;
		}

		// temporarily disable the player
		m_player.gameObject.SetActive(false);

		// generate characters
		m_characterA = m_statGenerator.GenerateCharacter();
		m_textA.text = m_statGenerator.GenerateDescription(m_characterA);

		m_characterB = m_statGenerator.GenerateCharacter();
		m_textB.text = m_statGenerator.GenerateDescription(m_characterB);
		
		m_characterC = m_statGenerator.GenerateCharacter();
		m_textC.text = m_statGenerator.GenerateDescription(m_characterC);

		// set the callbacks
		m_buttonA.onClick.AddListener(SelectA);
		m_buttonB.onClick.AddListener(SelectB);
		m_buttonC.onClick.AddListener(SelectC);

	}

	private void SelectA() {
		SetCharacter(m_characterA);
	}

	private void SelectB() {
		SetCharacter(m_characterB);
	}

	private void SelectC() {
		SetCharacter(m_characterC);
	}

	private void SetCharacter(CharacterStats cs) {
		m_player.gameObject.SetActive(true);
		m_player.SetBaseStats(cs, m_statGenerator.BasicWeapon);
	}

}
