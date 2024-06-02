using Godot;
using System;
using System.Linq;

public partial class BiggieCombatMenu : CanvasLayer
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private BasePageBasePanel _nodeBasePagePanel = null;
	private ChatPageBasePanel _nodeChatPagePanel = null;
	private FightPageBasePanel _nodeFightPagePanel = null;
	private TargetingPagePanel _nodeTargetingPagePanel = null;
	private InfoPagePanel _nodeInfoPagePanel = null;

	private CombatSingleton _globalCombatSingleton = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeBasePagePanel = GetNode<BasePageBasePanel>("./TextBoxContainer/BasePagePanel");
		_nodeChatPagePanel = GetNode<ChatPageBasePanel>("./TextBoxContainer/ChatPagePanel");
		_nodeFightPagePanel = GetNode<FightPageBasePanel>("./TextBoxContainer/FightPagePanel");
		_nodeTargetingPagePanel = GetNode<TargetingPagePanel>("./TextBoxContainer/TargetingPagePanel");
		_nodeInfoPagePanel = GetNode<InfoPagePanel>("./TextBoxContainer/InfoPagePanel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");

		_nodeBasePagePanel.IsOpen = false;
		_nodeChatPagePanel.IsOpen = false;
		_nodeFightPagePanel.IsOpen = false;
		_nodeTargetingPagePanel.IsOpen = false;
		_nodeInfoPagePanel.IsOpen = false;
		HandleHideActionInfo();

		_nodeBasePagePanel.SelectBase += HandleBaseSelection;
		_nodeBasePagePanel.ShowActionInfo += HandleShowActionInfo;
		_nodeBasePagePanel.HideActionInfo += HandleHideActionInfo;
		_nodeChatPagePanel.SelectChat += HandleChatSelection;
		_nodeTargetingPagePanel.SelectTarget += HandleTargetingSelection;
		_nodeInfoPagePanel.SelectInfo += HandleInfoSelection;
		_nodeChatPagePanel.ShowActionInfo += HandleShowActionInfo;
		_nodeChatPagePanel.HideActionInfo += HandleHideActionInfo;
		_nodeFightPagePanel.SelectFight += HandleFightSelection;
		_nodeFightPagePanel.ShowActionInfo += HandleShowActionInfo;
		_nodeFightPagePanel.HideActionInfo += HandleHideActionInfo;
	}

	public override void _Process(double _delta)
	{
	}

	public void StartTurn()
	{
		_nodeBasePagePanel.IsOpen = true;
	}

	[Signal]
	public delegate void EndBiggieCombatMenuTurnEventHandler(int combatOption, int enemyTargetIndex);

	public void EndTurn()
	{
		ResetPageState();
	}

	public void HandleBaseSelection(int selection)
	{
		//////GD.Print("HandleBaseSelection");
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
			case (int)Enumerations.Combat.BasePagePanelOptions.Info:
				_nodeBasePagePanel.IsOpen = false;
				_nodeInfoPagePanel.IsOpen = true;
				_nodeInfoPagePanel.ProcessSelection();
				break;
			case (int)Enumerations.Combat.BasePagePanelOptions.Exit:
				break;
			default:
				//////GD.Print("_nodeBasePagePanel.IsOpen but not mapped");
				break;
		}

		_nodeBasePagePanel.ResetPointerOffset();
		_nodeBasePagePanel.ProcessSelection();
	}

	public void HandleFightSelection(int selection)
	{
		//////GD.Print($"HandleFightSelection {selection}");
		try
		{
			switch (selection)
			{
				case (int)Enumerations.Combat.FightPagePanelOptions.Scratch:
					if (_globalCombatSingleton.EnemyTargetList.Count == 1)
					{
						EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Scratch, 0);
					}
					else if (_globalCombatSingleton.EnemyTargetList.Count > 1)
					{
						_nodeTargetingPagePanel.IsOpen = true;
						_nodeFightPagePanel.IsOpen = false;
						HandleHideActionInfo();
						_nodeTargetingPagePanel.ProcessSelection(Enumerations.Combat.CombatOptions.Scratch);
						_nodeTargetingPagePanel.ShowSelectionPanels();
					}
					else
					{
						//GD.Print("There are no enemy targets.");
					}
					break;
				case (int)Enumerations.Combat.FightPagePanelOptions.Bite:
					if (_globalCombatSingleton.EnemyTargetList.Count == 1)
					{
						////GD.Print("Bite");
						EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Bite, 0);
					}
					else if (_globalCombatSingleton.EnemyTargetList.Count > 1)
					{
						_nodeTargetingPagePanel.IsOpen = true;
						_nodeFightPagePanel.IsOpen = false;
						HandleHideActionInfo();
						_nodeTargetingPagePanel.ProcessSelection(Enumerations.Combat.CombatOptions.Bite);
						_nodeTargetingPagePanel.ShowSelectionPanels();
					}
					else
					{
						//GD.Print("There are no enemy targets.");
					}
					break;
				case (int)Enumerations.Combat.FightPagePanelOptions.Back:
					_nodeBasePagePanel.IsOpen = true;
					_nodeFightPagePanel.IsOpen = false;
					_nodeBasePagePanel.ProcessSelection();
					break;
				default:
					//////GD.Print("_nodeFightPagePanel.IsOpen but not mapped");
					break;
			}

			_nodeFightPagePanel.ResetPointerOffset();
		}
		catch (Exception exception)
		{
			//////GD.Print($"exception {exception.Message}");
		}
	}

	public void HandleChatSelection(int selection)
	{
		//////GD.Print("HandleChatSelection");
		switch (selection)
		{
			case (int)Enumerations.Combat.ChatPagePanelOptions.Ask:
				if (_globalCombatSingleton.EnemyTargetList.Count == 1)
				{
					////GD.Print("Ask");
					EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Ask, 0);
				}
				else if (_globalCombatSingleton.EnemyTargetList.Count > 1)
				{
					_nodeTargetingPagePanel.IsOpen = true;
					_nodeChatPagePanel.IsOpen = false;
					HandleHideActionInfo();
					_nodeTargetingPagePanel.ProcessSelection(Enumerations.Combat.CombatOptions.Ask);
					_nodeTargetingPagePanel.ShowSelectionPanels();
				}
				else
				{
					//GD.Print("There are no enemy targets.");
				}
				break;
			case (int)Enumerations.Combat.ChatPagePanelOptions.Charm:
				if (_globalCombatSingleton.EnemyTargetList.Count == 1)
				{
					////GD.Print("Charm");
					EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Charm, 0);
				}
				else if (_globalCombatSingleton.EnemyTargetList.Count > 1)
				{
					_nodeTargetingPagePanel.IsOpen = true;
					_nodeChatPagePanel.IsOpen = false;
					HandleHideActionInfo();
					_nodeTargetingPagePanel.ProcessSelection(Enumerations.Combat.CombatOptions.Charm);
					_nodeTargetingPagePanel.ShowSelectionPanels();
				}
				else
				{
					//GD.Print("There are no enemy targets.");
				}
				break;
			case (int)Enumerations.Combat.ChatPagePanelOptions.Back:
				_nodeBasePagePanel.IsOpen = true;
				_nodeChatPagePanel.IsOpen = false;
				_nodeBasePagePanel.ProcessSelection();
				break;
			default:
				//////GD.Print("_nodeChatPagePanel.IsOpen but not mapped");
				break;
		}

		_nodeChatPagePanel.ResetPointerOffset();
	}

	public void HandleTargetingSelection(int combatOption, int enemyTargetIndex)
	{
		// Selection is Back
		if (enemyTargetIndex == -1)
		{
			HandleTargetingSelectionBack();
			return;
		}

		//GD.Print($"HandleTargetingSelection enemyTargetIndex {enemyTargetIndex}");
		//GD.Print($"HandleTargetingSelection combatOption {combatOption}");
		switch (combatOption)
		{
			case (int)Enumerations.Combat.CombatOptions.Ask:
				EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Ask, enemyTargetIndex);
				_nodeTargetingPagePanel.HideSelectionPanels();
				break;
			case (int)Enumerations.Combat.CombatOptions.Charm:
				EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Charm, enemyTargetIndex);
				_nodeTargetingPagePanel.HideSelectionPanels();
				break;
			case (int)Enumerations.Combat.CombatOptions.Scratch:
				EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Scratch, enemyTargetIndex);
				_nodeTargetingPagePanel.HideSelectionPanels();
				break;
			case (int)Enumerations.Combat.CombatOptions.Bite:
				EmitSignal(SignalName.EndBiggieCombatMenuTurn, (int)Enumerations.Combat.CombatOptions.Bite, enemyTargetIndex);
				_nodeTargetingPagePanel.HideSelectionPanels();
				break;
			case (int)Enumerations.Combat.CombatOptions.Info:
				HandleTargetingSelectionBack();
				break;
			default:
				break;
		}

		_nodeTargetingPagePanel.ResetPointerOffset();
	}

	public void HandleTargetingSelectionBack()
	{
		//GD.Print("CombatOption BACK");
		// this will represent the BACK button
		_nodeTargetingPagePanel.IsOpen = false;
		_nodeTargetingPagePanel.HideSelectionPanels();
		_nodeBasePagePanel.IsOpen = true;
		_nodeBasePagePanel.ProcessSelection();
	}

	public void HandleInfoSelection()
	{
		_nodeInfoPagePanel.IsOpen = false;
		_nodeInfoPagePanel.HideSelectionPanels();
		_nodeBasePagePanel.IsOpen = true;
		_nodeBasePagePanel.ProcessSelection();
	}

	private void ResetPageState()
	{
		_nodeBasePagePanel.ResetPointerOffset();
		_nodeChatPagePanel.ResetPointerOffset();
		_nodeFightPagePanel.ResetPointerOffset();
		_nodeTargetingPagePanel.ResetPointerOffset();
		_nodeBasePagePanel.IsOpen = false;
		_nodeChatPagePanel.IsOpen = false;
		_nodeFightPagePanel.IsOpen = false;
		_nodeTargetingPagePanel.IsOpen = false;
	}

	[Signal]
	public delegate void ShowActionInfoEventHandler();
	[Signal]
	public delegate void HideActionInfoEventHandler();
	private void HandleShowActionInfo()
	{
		EmitSignal(SignalName.ShowActionInfo);
	}
	private void HandleHideActionInfo()
	{
		EmitSignal(SignalName.HideActionInfo);
	}
}
