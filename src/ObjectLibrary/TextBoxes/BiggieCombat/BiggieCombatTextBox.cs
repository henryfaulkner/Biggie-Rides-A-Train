using Godot;
using System;

public partial class BiggieCombatTextBox : CanvasLayer
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private CanvasLayer _nodeSelf = null;
	private BasePageBasePanel _nodeBasePagePanel = null;
	private ChatPageBasePanel _nodeChatPagePanel = null;
	private FightPageBasePanel _nodeFightPagePanel = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeBasePagePanel = GetNode<BasePageBasePanel>("./TextBoxContainer/BasePagePanel");
		_nodeChatPagePanel = GetNode<ChatPageBasePanel>("./TextBoxContainer/ChatPagePanel");
		_nodeFightPagePanel = GetNode<FightPageBasePanel>("./TextBoxContainer/FightPagePanel");

		_nodeBasePagePanel.IsOpen = true;
		_nodeChatPagePanel.IsOpen = false;
		_nodeFightPagePanel.IsOpen = false;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			GD.Print("Interact Input");
			HandleInteraction();
		}
	}

	public void HandleInteraction()
	{
		if (_nodeBasePagePanel.IsOpen)
		{
			switch (_nodeBasePagePanel.SelectionHelperInstance.GetSelectedOptionId())
			{
				case (int)Enumerations.BasePagePanelOptions.Fight:
					_nodeBasePagePanel.IsOpen = false;
					_nodeFightPagePanel.IsOpen = true;
					break;
				case (int)Enumerations.BasePagePanelOptions.Chat:
					_nodeBasePagePanel.IsOpen = false;
					_nodeChatPagePanel.IsOpen = true;
					break;
				case (int)Enumerations.BasePagePanelOptions.Exit:
					break;
				default:
					GD.Print("_nodeBasePagePanel.IsOpen but not mapped");
					break;
			}
		}
		else if (_nodeChatPagePanel.IsOpen)
		{
			switch (_nodeChatPagePanel.SelectionHelperInstance.GetSelectedOptionId())
			{
				case (int)Enumerations.ChatPagePanelOptions.Ask:
					break;
				case (int)Enumerations.ChatPagePanelOptions.Charm:
					break;
				case (int)Enumerations.ChatPagePanelOptions.Back:
					_nodeBasePagePanel.IsOpen = true;
					_nodeChatPagePanel.IsOpen = false;
					break;
				default:
					GD.Print("_nodeChatPagePanel.IsOpen but not mapped");
					break;
			}
		}
		else if (_nodeFightPagePanel.IsOpen)
		{
			switch (_nodeFightPagePanel.SelectionHelperInstance.GetSelectedOptionId())
			{
				case (int)Enumerations.FightPagePanelOptions.Scratch:
					break;
				case (int)Enumerations.FightPagePanelOptions.Bite:
					break;
				case (int)Enumerations.FightPagePanelOptions.Back:
					_nodeBasePagePanel.IsOpen = true;
					_nodeFightPagePanel.IsOpen = false;
					break;
				default:
					GD.Print("_nodeFightPagePanel.IsOpen but not mapped");
					break;
			}
		}
		else
		{
			GD.Print("BiggieCombatTextBox.HandleInteraction: uhghh nothing was open ??");
		}
	}
}
