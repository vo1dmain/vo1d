using System.Collections;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character.ICharacterController>();

		if (character != null && dealDamageRoutine == null)
		{
			dealDamageRoutine = StartCoroutine(DealDamage());
		}
	}

	private void OnTriggerStay(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character.ICharacterController>();

		if (character != null && dealDamageRoutine == null)
		{
			dealDamageRoutine = StartCoroutine(DealDamage());
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character.ICharacterController>();

		if (character != null)
		{
			StopAllCoroutines();

			dealDamageRoutine = null;
		}
	}

	private IEnumerator DealDamage()
	{
		character.ReceiveDamage(damagePerIteration);

		yield return new WaitForSeconds(damageDelay);

		dealDamageRoutine = null;
	}

	[SerializeField] private uint damagePerIteration = 5;
	[SerializeField] private float damageDelay = 0.2f;

	private Character.ICharacterController character;
	private Coroutine dealDamageRoutine;
}
