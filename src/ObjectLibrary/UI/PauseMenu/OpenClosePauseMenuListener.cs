using Godot;
using System;

public partial class OpenClosePauseMenuListener : Node
{
	private static readonly StringName _PAUSE_INPUT = new StringName("escape");

	private Panel _nodeBasePanel = null;
	private Panel _nodeUserSettingsPanel = null;

	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;

		_nodeBasePanel = GetNode<Panel>("../MarginContainer/BasePanel");
		_nodeUserSettingsPanel = GetNode<Panel>("../MarginContainer/UserSettingsPanel");
	}

	public override void _Process(double _delta)
	{
		if (Input.IsActionJustPressed(_PAUSE_INPUT))
		{
			if (GetTree().Paused)
			{
				GD.Print("Unpause");
				_nodeBasePanel.Hide();
				_nodeUserSettingsPanel.Hide();
				EmitSignal(SignalName.CloseMenu);
				GetTree().Paused = false;
			}
			else
			{
				GD.Print("Pause");
				_nodeBasePanel.Show();
				EmitSignal(SignalName.OpenMenu);
				GetTree().Paused = true;
			}
		}
	}

	[Signal]
	public delegate void OpenMenuEventHandler();

	[Signal]
	public delegate void CloseMenuEventHandler();
}
