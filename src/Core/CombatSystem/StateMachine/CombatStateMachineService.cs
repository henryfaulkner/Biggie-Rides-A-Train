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
		return CombatStates.Where(x => x.Id == stateId).FirstOrDefault();
	}

	// If I could memotize these indexes, this could be much faster 
	public List<CombatTransitionModel> GetTransitionsByStateId(Enumerations.Combat.StateMachine.States stateId)
	{
		return CombatTransitions.Where(x => x.State.Id == stateId).ToList();
	}

	public CombatStateModel GetStateByEventAndTransitions(Enumerations.Combat.StateMachine.Events eventId,
														  List<CombatTransitionModel> transitions)
	{
		var transition = CombatTransitions.Where(x => x.Event.Id == eventId).FirstOrDefault();
		if (transition == null)
		{
			GD.PrintErr("GetStateByEventAndTransitions Transition was not found.");
			return new CombatStateModel();
		}
		var nextStateId = transition.NextState.Id;
		if (transition == null)
		{
			GD.PrintErr("GetStateByEventAndTransitions NextStateId was not found.");
			return new CombatStateModel();
		}

		return CombatStates.Where(x => x.Id == nextStateId).FirstOrDefault();
	}

	private void HandleCombatEvent(int eventIdIndex)
	{
		var eventId = (Enumerations.Combat.StateMachine.Events)eventIdIndex;
		var transitions = GetTransitionsByStateId(CurrentCombatState.Id);
		var nextState = GetStateByEventAndTransitions(eventId, transitions);
		CurrentCombatState = nextState;
	}

	private List<CombatTransitionModel> ConstructAllTransitions()
	{
		using var file = FileAccess.Open(_TRANSITIONS_FILE, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		return JsonConvert.DeserializeObject<List<CombatTransitionModel>>(content);
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
			new CombatStateModel(Enumerations.Combat.StateMachine.States.ChatterTextBox)
		};
		return result;
	}
}

