using Godot;
using System;

public partial class BiggieCombatTextBox : CanvasLayer
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private BasePageBasePanel _nodeBasePagePanel = null;
	private ChatPageBasePanel _nodeChatPagePanel = null;
	private FightPageBasePanel _nodeFightPagePanel = null;

	private CombatSingleton _globalCombatSingleton = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeBasePagePanel = GetNode<BasePageBasePanel>("./TextBoxContainer/BasePagePanel");
		_nodeChatPagePanel = GetNode<ChatPageBasePanel>("./TextBoxContainer/ChatPagePanel");
		_nodeFightPagePanel = GetNode<FightPageBasePanel>("./TextBoxContainer/FightPagePanel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");

		_nodeBasePagePanel.IsOpen = true;
		_nodeChatPagePanel.IsOpen = false;
		_nodeFightPagePanel.IsOpen = false;

		_nodeBasePagePanel.SelectBase += HandleBaseSelection;
		_nodeChatPagePanel.SelectChat += HandleChatSelection;
		_nodeFightPagePanel.SelectFight += HandleFightSelection;
	}

	public override void _Process(double delta)
	{
		//if (Input.IsActionJustPressed(_INTERACT_INPUT))
		//{
		////GD.Print("Interact Input");
		//HandleInteraction();
		//}
	}

	public void StartTurn()
	{

	}

	[Signal]
	public delegate void ProjectPhysicalDamageEventHandler();
	[Signal]
	public delegate void EndBiggieTurnEventHandler();

	public void EndTurn()
	{
		ResetPageState();
	}

	public void HandleBaseSelection(int selection)
	{
		//GD.Print("HandleBaseSelection");
		switch (selection)
		{
			case (int)Enumerations.BasePagePanelOptions.Fight:
				_nodeBasePagePanel.IsOpen = false;
				_nodeFightPagePanel.IsOpen = true;
				_nodeFightPagePanel.ProcessSelection();
				break;
			case (int)Enumerations.BasePagePanelOptions.Chat:
				_nodeBasePagePanel.IsOpen = false;
				_nodeChatPagePanel.IsOpen = true;
				_nodeChatPagePanel.ProcessSelection();
				break;
			case (int)Enumerations.BasePagePanelOptions.Exit:
				break;
			default:
				//GD.Print("_nodeBasePagePanel.IsOpen but not mapped");
				break;
		}

		_nodeBasePagePanel.ResetPointerOffset();
	}

	public void HandleFightSelection(int selection)
	{
		//GD.Print($"HandleFightSelection {selection}");
		try
		{
			switch (selection)
			{
				case (int)Enumerations.FightPagePanelOptions.Scratch:
					GD.Print("Scratch");
					DealPhysicalDamage(1);
					EmitSignal(SignalName.EndBiggieTurn);
					break;
				case (int)Enumerations.FightPagePanelOptions.Bite:
					GD.Print("Bite");
					DealPhysicalDamage(2);
					EmitSignal(SignalName.EndBiggieTurn);
					break;
				case (int)Enumerations.FightPagePanelOptions.Back:
					_nodeBasePagePanel.IsOpen = true;
					_nodeFightPagePanel.IsOpen = false;
					_nodeBasePagePanel.ProcessSelection();
					break;
				default:
					//GD.Print("_nodeFightPagePanel.IsOpen but not mapped");
					break;
			}

			_nodeFightPagePanel.ResetPointerOffset();
		}
		catch (Exception exception)
		{
			//GD.Print($"exception {exception.Message}");
		}
	}

	public void HandleChatSelection(int selection)
	{
		//GD.Print("HandleChatSelection");
		switch (selection)
		{
			case (int)Enumerations.ChatPagePanelOptions.Ask:
				GD.Print("Ask");
				DealEmotionalDamage(1);
				EmitSignal(SignalName.EndBiggieTurn);
				break;
			case (int)Enumerations.ChatPagePanelOptions.Charm:
				GD.Print("Charm");
				DealEmotionalDamage(2);
				EmitSignal(SignalName.EndBiggieTurn);
				break;
			case (int)Enumerations.ChatPagePanelOptions.Back:
				_nodeBasePagePanel.IsOpen = true;
				_nodeChatPagePanel.IsOpen = false;
				_nodeBasePagePanel.ProcessSelection();
				break;
			default:
				//GD.Print("_nodeChatPagePanel.IsOpen but not mapped");
				break;
		}

		_nodeChatPagePanel.ResetPointerOffset();
	}

	private void ResetPageState()
	{
		_nodeBasePagePanel.ResetPointerOffset();
		_nodeChatPagePanel.ResetPointerOffset();
		_nodeFightPagePanel.ResetPointerOffset();
		_nodeBasePagePanel.IsOpen = true;
		_nodeChatPagePanel.IsOpen = false;
		_nodeFightPagePanel.IsOpen = false;
	}

	public void DealPhysicalDamage(int damage)
	{
		GD.Print("DealPhysicalDamage");
		_globalCombatSingleton.BiggiePhysicalAttackProxy.DealDamage(damage);
		GD.Print($"enemy health: {_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetHealthPercentage()}");
		EmitSignal(SignalName.ProjectPhysicalDamage);
	}

	public void DealEmotionalDamage(int damage)
	{
		_globalCombatSingleton.BiggieEmotionalAttackProxy.DealDamage(damage);
	}
}
