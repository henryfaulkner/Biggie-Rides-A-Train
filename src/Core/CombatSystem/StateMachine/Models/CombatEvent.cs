using System;
using System.ComponentModel;

public class CombatEventModel
{
	public CombatEventModel() { }
	public CombatEventModel(Enumerations.Combat.StateMachine.Events eventId)
	{
		Id = eventId;
		// add Description to Enum, give them name priority
		Name = Enumerations.Combat.StateMachine.Events.GetName(eventId);
	}

	public Enumerations.Combat.StateMachine.Events Id { get; set; }
	public string Name { get; set; }
}
