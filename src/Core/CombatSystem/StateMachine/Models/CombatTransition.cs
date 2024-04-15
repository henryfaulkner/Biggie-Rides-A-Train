using System;

public class CombatTransitionModel
{
	public CombatTransitionModel()
	{
		State = null;
		Event = null;
		NextState = null;
	}

	public CombatTransitionModel(Enumerations.Combat.StateMachine.States stateId, Enumerations.Combat.StateMachine.Events eventId, Enumerations.Combat.StateMachine.States nextStateId)
	{
		State = new CombatStateModel(stateId);
		Event = new CombatEventModel(eventId);
		NextState = new CombatStateModel(nextStateId); //MapStateAndEventToNextState(stateId, eventId);
	}


	public CombatStateModel State { get; set; }
	public CombatEventModel Event { get; set; }
	public CombatStateModel NextState { get; set; }

	// public CombatStateModel MapStateAndEventToNextState(Enumerations.Combat.StateMachine.States stateId, Enumerations.Combat.StateMachine.Events eventId)
	// {
	// 	return MapStateAndEventToNextState(new CombatStateModel(stateId), new CombatEventModel(eventId));
	// }

	public CombatStateModel MapStateAndEventToNextState(CombatStateModel state, CombatEventModel e)

	{
		return new CombatStateModel();
	}
}


