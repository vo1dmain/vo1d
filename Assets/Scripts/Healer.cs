using System.Collections;
using UnityEngine;

public class Healer : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character.ICharacterController>();
		
		if (character != null && healingRoutine == null)
		{
			healingRoutine = StartCoroutine(Heal(healPerIteration));
		}
	}

	private void OnTriggerStay(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character.ICharacterController>();

		if (character != null && healingRoutine == null)
		{
			healingRoutine = StartCoroutine(Heal(healPerIteration));
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character.ICharacterController>();

		if (character != null)
		{
			StopAllCoroutines();

			healingRoutine = null;

			character.ReceiveHeal(0u);
		}
	}

	private IEnumerator Heal(uint heal)
	{
		character.ReceiveHeal(heal);

		yield return new WaitForSeconds(healingDelay);

		healingRoutine = null;
	}

	[SerializeField] private uint healPerIteration = 1;
	[SerializeField] private float healingDelay = 0.2f;

	private Character.ICharacterController character;
	private Coroutine healingRoutine;
}
