using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public partial class CombatStateMachineService : Node
{
	private static readonly StringName _TRANSITIONS_FILE = new StringName("res://Core/CombatSystem/StateMachine/Transitions.json");

	public CombatStateMachineService()
	{
		CombatTransitions = ConstructAllTransitions();
		CombatStates = ConstructAllStates();
		CombatEvent += HandleCombatEvent;
	}

	[Signal]
	public delegate void CombatEventEventHandler(int eventIdIndex);

	public CombatStateModel CurrentCombatState { get; private set; }
	private List<CombatTransitionModel> CombatTransitions { get; set; }
	private List<CombatStateModel> CombatStates { get; set; }

	public void Reset()
	{
		CurrentCombatState = new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieCombatMenu);
	}

	public List<CombatTransitionModel> GetAllTransitions()
	{
		return CombatTransitions;
	}

	public List<CombatStateModel> GetAllStates()
	{
		return CombatStates;
	}

	public CombatStateModel GetStateById(Enumerations.Combat.StateMachine.States stateId)
	{
		return CombatStates.Where(x => x.Id == stateId).First();
	}

	// If I could memotize these indexes, this could be much faster 
	public List<CombatTransitionModel> GetTransitionsByStateId(Enumerations.Combat.StateMachine.States stateId)
	{
		return CombatTransitions.Where(x => x.State.Id == stateId).ToList();
	}

	public CombatStateModel GetStateByEventAndTransitions(Enumerations.Combat.StateMachine.Events eventId,
														  List<CombatTransitionModel> transitions)
	{
		var transition = transitions.Where(x => x.Event.Id == eventId).First();
		if (transition == null)
		{
			GD.Print("GetStateByEventAndTransitions Transition was not found.");
			GD.PrintErr("GetStateByEventAndTransitions Transition was not found.");
			return new CombatStateModel();
		}
		var nextStateId = transition.NextState.Id;
		if (transition == null)
		{
			GD.Print("GetStateByEventAndTransitions NextStateId was not found.");
			GD.PrintErr("GetStateByEventAndTransitions NextStateId was not found.");
			return new CombatStateModel();
		}

		var result = CombatStates.Where(x => x.Id == nextStateId).First();
		return result;
	}

	private void HandleCombatEvent(int eventIdIndex)
	{
		var eventId = (Enumerations.Combat.StateMachine.Events)eventIdIndex;
		var transitions = GetTransitionsByStateId(CurrentCombatState.Id);

		GD.Print($"BEFORE CombatStateMachineService HandleCombatEvent: {CurrentCombatState.Id} {CurrentCombatState.Name}");
		GD.Print($"EventId: {eventId}");
		GD.Print(JsonConvert.SerializeObject(transitions));
		var nextState = GetStateByEventAndTransitions(eventId, transitions);
		GD.Print($"AFTER CombatStateMachineService HandleCombatEvent: {nextState.Id} {nextState.Name}");
		CurrentCombatState = nextState;
	}

	private List<CombatTransitionModel> ConstructAllTransitions()
	{
		var result = new List<CombatTransitionModel>();
		try
		{
			using var file = FileAccess.Open(_TRANSITIONS_FILE, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			JsonConvert.DeserializeObject<List<CombatTransitionEntity>>(content)
				.ForEach(x =>
				{
					result.Add(new CombatTransitionModel(
						(Enumerations.Combat.StateMachine.States)x.StateId,
						(Enumerations.Combat.StateMachine.Events)x.EventId,
						(Enumerations.Combat.StateMachine.States)x.NextStateId));
				});
		}
		catch (Exception exception)
		{
			GD.PrintErr($"Issue parsing _TRANSITIONS_FILE: {exception.Message}");
		}
		return result;
	}

	private List<CombatStateModel> ConstructAllStates()
	{
		var result = new List<CombatStateModel>
		{
			new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieCombatMenu),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToBiggieChatAsk),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToBiggieChatCharm),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToBiggieFightScratch),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToBiggieFightBite),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieChatAsk),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieChatCharm),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieFightScratch),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieFightBite),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatAsk),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatCharm),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightScratch),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightBite),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.EnemyAttack),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToBiggieCombatMenu),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieDefeat),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.EnemyDefeatPhysical),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.EnemyDefeatEmotional),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToChatterTextBox),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.ChatterTextBox)
		};
		return result;
	}

	private class CombatTransitionEntity
	{
		public int StateId { get; set; }
		public int EventId { get; set; }
		public int NextStateId { get; set; }
	}
}

