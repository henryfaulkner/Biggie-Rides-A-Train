public abstract class AbstractAttackProxy
{
    protected abstract CombatService CombatService { get; set; }

    protected abstract void DealDamage(double damage);
}