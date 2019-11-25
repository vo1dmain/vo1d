using TMPro;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
	private void Awake()
	{
		HUDPanel = GetComponent<TextMeshProUGUI>();
		currentHealth = player.HealthPoints;
		HUDPanel.text = $"HP {player.HealthPoints}";
	}

	private void Update()
	{
		if (currentHealth != player.HealthPoints)
		{
			currentHealth = player.HealthPoints;

			HUDPanel.text = $"HP {player.HealthPoints}";
		}
	}

	[SerializeField] private TextMeshProUGUI HUDPanel;
	[SerializeField] private Character player;

	private uint currentHealth;
}
