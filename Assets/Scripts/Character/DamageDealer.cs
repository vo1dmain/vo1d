using System.Collections;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character>();

		if (character && character.Alive && dealDamageRoutine == null)
		{
			dealDamageRoutine = StartCoroutine(DealDamage(damagePerIteration));
		}
	}

	private void OnTriggerStay(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character>();

		if (character && character.Alive && dealDamageRoutine == null)
		{
			dealDamageRoutine = StartCoroutine(DealDamage(damagePerIteration));
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		character = collider.gameObject.GetComponent<Character>();

		if (character)
		{
			StopAllCoroutines();

			dealDamageRoutine = null;

			character.ReceiveDamage(0u);
		}
	}

	private IEnumerator DealDamage(uint damage)
	{
		character.ReceiveDamage(damage);

		yield return new WaitForSeconds(damagingDelay);

		dealDamageRoutine = null;
	}

	[SerializeField] private uint damagePerIteration = 5;
	[SerializeField] private float damagingDelay = 0.2f;

	private Character character;
	private Coroutine dealDamageRoutine;
}
