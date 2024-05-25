using Godot;
using System;

public partial class OpenPauseMenuListener : Node
{
	private static readonly StringName _PAUSE_INPUT = new StringName("escape");
	private Node2D _nodePauseMenu = null;

	public override void _Ready()
	{
		_nodePauseMenu = GetNode<Node2D>("..");
		ProcessMode = Node.ProcessModeEnum.Always;
	}

	public override void _Process(double _delta)
	{
		if (Input.IsActionJustPressed(_PAUSE_INPUT))
		{
			if (GetTree().Paused)
			{
				GD.Print("Unpause");
				CloseMenu();
				GetTree().Paused = false;
			}
			else
			{
				GD.Print("Pause");
				OpenMenu();
				GetTree().Paused = true;
			}
		}
	}

	private void OpenMenu()
	{
		GD.Print("OpenMenu");
		_nodePauseMenu.Show();
	}

	private void CloseMenu()
	{
		GD.Print("CloseMenu");
		_nodePauseMenu.Hide();
	}
}
