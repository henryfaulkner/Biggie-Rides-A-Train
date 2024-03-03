public abstract class AbstractAttackProxy
{
	protected abstract CombatService CombatService { get; set; }

	public abstract void DealDamage(double damage);
	public abstract int GetTargetHealthPercentage();
}
