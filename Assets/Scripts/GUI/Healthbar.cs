using TMPro;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
	private void Awake()
	{
		HUDPanel = GetComponent<TextMeshProUGUI>();
		HUDPanel.text = $"HP {currentCharacter.HealthPoints}";
		currentCharacter.HealthChanged += UpdateGUI;
	}

	private void UpdateGUI()
	{
		HUDPanel.text = $"HP {currentCharacter.HealthPoints}";
	}

	[SerializeField] private TextMeshProUGUI HUDPanel;
	[SerializeField] private Character.CharacterModel currentCharacter;
}
