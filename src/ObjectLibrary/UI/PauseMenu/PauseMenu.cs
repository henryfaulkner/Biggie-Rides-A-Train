using Godot;
using System;

public partial class PauseMenu : Node2D
{
	private static readonly StringName _PAUSE_INPUT = new StringName("escape");

	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.WhenPaused;
	}
}
