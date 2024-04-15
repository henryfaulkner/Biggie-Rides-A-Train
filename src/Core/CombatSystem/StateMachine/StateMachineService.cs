using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class StateMachineService
{
	private static readonly StringName _TRANSITIONS_FILE = new StringName("res://Core/CombatSystem/StateMachine/Transitions.json");

	public StateMachineService()
	{
		CombatTransitions = ConstructAllTransitions();
		CombatStates = ConstructAllStates();
	}

	public CombatStateModel CurrentCombatState { get; set; }
	private List<CombatTransitionModel> CombatTransitions { get; set; }
	private List<CombatStateModel> CombatStates { get; set; }

	public List<CombatTransitionModel> GetAllCombatTransitions()
	{
		return CombatTransitions;
	}

	public List<CombatStateModel> GetAllCombatStates()
	{
		return CombatStates;
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
			new CombatStateModel(Enumerations.Combat.StateMachine.States.TransitionToCombatMenu),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.BiggieDefeat),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.EnemyDefeatPhysical),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.EnemyDefeatEmotional),
			new CombatStateModel(Enumerations.Combat.StateMachine.States.ChatterTextBox)
		};
		return result;
	}
}

