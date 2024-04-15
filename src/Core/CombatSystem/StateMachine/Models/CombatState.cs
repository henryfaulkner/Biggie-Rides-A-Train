using System;
using System.Collections.Generic;

public class CombatStateModel
{
	public CombatStateModel()
	{
		CurrentEventHandler = null;
	}
	public CombatStateModel(Enumerations.Combat.StateMachine.States stateId)
	{
		Id = stateId;
		// add Description to Enum, give them name priority
		Name = Enumerations.Combat.StateMachine.States.GetName(stateId);

		CurrentEventHandler = null;
	}

	public Enumerations.Combat.StateMachine.States Id { get; set; }
	public string Name { get; set; }


	// occurs once State is changed to  
	// return true when Event is finished being handled 
	public Queue<Func<bool>> EventHandlers { get; set; }

	public void AddEventHandler(Func<bool> eventHandler)
	{
		EventHandlers.Enqueue(eventHandler);
	}

	// return true when all Events are finished being handled 
	private Func<bool> CurrentEventHandler { get; set; }
	public bool ExecuteEventHandlers()
	{
		if (EventHandlers.Count == 0 && CurrentEventHandler == null) return true;
		if (CurrentEventHandler == null) CurrentEventHandler = EventHandlers.Dequeue();
		var isEventFinished = CurrentEventHandler();
		if (isEventFinished) CurrentEventHandler = null;
		return false;
	}
}
