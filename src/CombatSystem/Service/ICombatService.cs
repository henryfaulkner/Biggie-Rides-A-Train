public interface ICombatService
{
	public double GetSourceCurrentHealth();
	public double GetSourceMaxHealth();
	public double GetTargetCurrentHealth();
	public double GetTargetMaxHealth();
	public void TakeDamage(double damage);
	public void GiveDamage(double damage);
	public bool SourceBelowZero();
	public bool TargetBelowZero();
}
