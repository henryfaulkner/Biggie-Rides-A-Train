using Godot;
using System;
using System.Runtime.CompilerServices;

// all Biggie attack will interface via this proxy
// when delivering damage to an opponent

// During implementation, Biggie will need two open
// proxies. One for physically dealing damage and 
// another for emotionally dealing damage.
public class BiggieAttackProxy : AbstractAttackProxy
{
	public BiggieAttackProxy() { }

	public BiggieAttackProxy(CombatBiggieModel biggie, CombatParticipantModel physicalOrEmotional)
	{
		CombatService = new CombatService(biggie, physicalOrEmotional);
	}

	protected override CombatService CombatService { get; set; }

	public override void DealDamage(double damage)
	{
		CombatService.GiveDamage(damage);
	}

	public override int GetTargetCurrentHealth()
	{
		return (int)Math.Ceiling(
			CombatService.GetTargetCurrentHealth()
		);
	}

	public override int GetTargetMaxHealth()
	{
		return (int)Math.Ceiling(
			CombatService.GetTargetMaxHealth()
		);
	}

	public override bool IsTargetDefeated()
	{
		return CombatService.GetTargetCurrentHealth() <= 0;
	}

	public override int GetTargetHealthPercentage()
	{
		return (int)Math.Ceiling(
			(double)(CombatService.GetTargetCurrentHealth() / CombatService.GetTargetMaxHealth()) * 100
		);
	}
}
