public interface ICombatService
{
	public double GetSourceHealth();
	public double GetTargetHealth();
	public void TakeDamage(double damage);
	public void GiveDamage(double damage);
	public bool SourceBelowZero();
	public bool TargetBelowZero();
}