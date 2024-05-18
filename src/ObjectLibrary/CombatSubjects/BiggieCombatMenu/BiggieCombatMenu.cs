using Godot;
using System;

public partial class BiggieCombatMenu : CanvasLayer
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private BasePageBasePanel _nodeBasePagePanel = null;
	private ChatPageBasePanel _nodeChatPagePanel = null;
	private FightPageBasePanel _nodeFightPagePanel = null;
	private TargetingPagePanel _nodeTargetingPagePanel = null;

	private CombatSingleton _globalCombatSingleton = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeBasePagePanel = GetNode<BasePageBasePanel>("./TextBoxContainer/BasePagePanel");
		_nodeChatPagePanel = GetNode<ChatPageBasePanel>("./TextBoxContainer/ChatPagePanel");
		_nodeFightPagePanel = GetNode<FightPageBasePanel>("./TextBoxContainer/FightPagePanel");
		_nodeTargetingPagePanel = GetNode<TargetingPagePanel>("./TextBoxContainer/TargetingPagePanel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");

		_nodeBasePagePanel.IsOpen = false;
		_nodeChatPagePanel.IsOpen = false;
		_nodeFightPagePanel.IsOpen = false;
		HideActionInfoFunc();

		_nodeBasePagePanel.SelectBase += HandleBaseSelection;
		_nodeBasePagePanel.ShowActionInfo += ShowActionInfoFunc;
		_nodeBasePagePanel.HideActionInfo += HideActionInfoFunc;
		_nodeChatPagePanel.SelectChat += HandleChatSelection;
		_nodeChatPagePanel.ShowActionInfo += ShowActionInfoFunc;
		_nodeChatPagePanel.HideActionInfo += HideActionInfoFunc;
		_nodeFightPagePanel.SelectFight += HandleFightSelection;
		_nodeFightPagePanel.ShowActionInfo += ShowActionInfoFunc;
		_nodeFightPagePanel.HideActionInfo += HideActionInfoFunc;
	}

	public override void _Process(double delta)
	{
		if (_nodeTargetingPagePanel.IsTargeting
			&& _globalCombatSingleton.CombatStateMachineService.IsATargetEnemyTransition(
				_globalCombatSingleton.CombatStateMachineService.CurrentCombatState.Id
			))
		{
			_nodeTargetingPagePanel.IsTargeting = true;
		}
	}

	public void StartTurn()
	{
		_nodeBasePagePanel.IsOpen = true;
	}

	[Signal]
	public delegate void EndBiggieCombatMenuTurnEventHandler(int combatOption);

	public void EndTurn()
	{
		ResetPageState();
	}

	public void HandleBaseSelection(int selection)
	{
		////GD.Print("HandleBaseSelection");
		switch (selection)
		{
			case (int)Enumerations.Combat.BasePagePanelOptions.Fight:
				_nodeBasePagePanel.IsOpen = false;
				_nodeFightPagePanel.IsOpen = true;
				_nodeFightPagePanel.ProcessSelection();
				break;
			case (int)Enumerations.Combat.BasePagePanelOptions.Chat:
				_nodeBasePagePanel.IsOpen = false;
				_nodeChatPagePanel.IsOpen = true;
				_nodeChatPagePanel.ProcessSelection();
				break;
			case (int)Enumerations.Combat.BasePagePanelOptions.Exit:
				break;
			default:
				////GD.Print("_nodeBasePagePanel.IsOpen but not mapped");
				break;
		}

		_nodeBasePagePanel.ResetPointerOffset();
		_nodeBasePagePanel.ProcessSelection();
	}

	public void HandleFightSelection(int selection)
	{
		////GD.Print($"HandleFightSelection {selection}");
		try
		{
			switch (selection)
			{
				case (int)Enumerations.Combat.FightPagePanelOptions.Scratch:
					//GD.Print("Scratch");
					EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Scratch);
					break;
				case (int)Enumerations.Combat.FightPagePanelOptions.Bite:
					//GD.Print("Bite");
					EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Bite);
					break;
				case (int)Enumerations.Combat.FightPagePanelOptions.Back:
					_nodeBasePagePanel.IsOpen = true;
					_nodeFightPagePanel.IsOpen = false;
					_nodeBasePagePanel.ProcessSelection();
					break;
				default:
					////GD.Print("_nodeFightPagePanel.IsOpen but not mapped");
					break;
			}

			_nodeFightPagePanel.ResetPointerOffset();
		}
		catch (Exception exception)
		{
			////GD.Print($"exception {exception.Message}");
		}
	}

	public void HandleChatSelection(int selection)
	{
		////GD.Print("HandleChatSelection");
		switch (selection)
		{
			case (int)Enumerations.Combat.ChatPagePanelOptions.Ask:
				//GD.Print("Ask");
				EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Ask);
				break;
			case (int)Enumerations.Combat.ChatPagePanelOptions.Charm:
				//GD.Print("Charm");
				EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Charm);
				break;
			case (int)Enumerations.Combat.ChatPagePanelOptions.Back:
				_nodeBasePagePanel.IsOpen = true;
				_nodeChatPagePanel.IsOpen = false;
				_nodeBasePagePanel.ProcessSelection();
				break;
			default:
				////GD.Print("_nodeChatPagePanel.IsOpen but not mapped");
				break;
		}

		_nodeChatPagePanel.ResetPointerOffset();
	}

	private void ResetPageState()
	{
		_nodeBasePagePanel.ResetPointerOffset();
		_nodeChatPagePanel.ResetPointerOffset();
		_nodeFightPagePanel.ResetPointerOffset();
		_nodeBasePagePanel.IsOpen = false;
		_nodeChatPagePanel.IsOpen = false;
		_nodeFightPagePanel.IsOpen = false;
	}

	[Signal]
	public delegate void ShowActionInfoEventHandler();
	[Signal]
	public delegate void HideActionInfoEventHandler();
	private void ShowActionInfoFunc()
	{
		EmitSignal(SignalName.ShowActionInfo);
	}
	private void HideActionInfoFunc()
	{
		EmitSignal(SignalName.HideActionInfo);
	}
}
