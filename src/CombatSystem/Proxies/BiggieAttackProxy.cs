using Godot;
using System;

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

    protected override void DealDamage(double damage)
    {
        CombatService.GiveDamage(damage);
    }
}