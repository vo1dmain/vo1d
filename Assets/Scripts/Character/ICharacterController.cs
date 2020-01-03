namespace Character
{
	public interface ICharacterController
	{
		void ReceiveDamage(uint dmg);
		void ReceiveHeal(uint heal);
	}
}
