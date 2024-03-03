using Godot;
using System;

// all opponent attacks will interface via this proxy
// when delivering damage to biggie :(
public class OpponentAttackProxy : AbstractAttackProxy
{
	public OpponentAttackProxy() { }

	public OpponentAttackProxy(CombatParticipantModel opponent, CombatBiggieModel biggie)
	{
		CombatService = new CombatService(opponent, biggie);
	}

	protected override CombatService CombatService { get; set; }

	public override void DealDamage(double damage)
	{
		CombatService.GiveDamage(damage);
	}
	
	public override int GetTargetHealthPercentage()
	{
		return (int)Math.Ceiling(
			(double)(CombatService.GetTargetCurrentHealth() / CombatService.GetTargetMaxHealth()) * 100
		);
	}
}
