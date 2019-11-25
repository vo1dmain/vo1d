using TMPro;
using UnityEngine;

public class Ammobar : MonoBehaviour
{
	private void Awake()
	{
		HUDPanel = GetComponent<TextMeshProUGUI>();
		currentAmmoCount = player.Ammo;
		HUDPanel.text = $"Ammo {player.Ammo}";
	}

	private void Update()
	{
		if (currentAmmoCount != player.Ammo)
		{
			currentAmmoCount = player.Ammo;

			HUDPanel.text = $"Ammo {player.Ammo}";
		}
	}

	[SerializeField] private TextMeshProUGUI HUDPanel;
	[SerializeField] private Character player;

	private uint currentAmmoCount;
}
