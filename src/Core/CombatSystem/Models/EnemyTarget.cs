using Godot;

public class EnemyTarget
{
	public int Id { get; set; }
	public string Name { get; set; }
	public Panel TargetPanel { get; set; }
	public IEnemyAppearance Appearance { get; set; }
	public CombatParticipantModel EnemyEmotional { get; set; }
	public CombatParticipantModel EnemyPhysical { get; set; }
	public BiggieAttackProxy BiggiePhysicalAttackProxy { get; set; }
	public BiggieAttackProxy BiggieEmotionalAttackProxy { get; set; }
}
