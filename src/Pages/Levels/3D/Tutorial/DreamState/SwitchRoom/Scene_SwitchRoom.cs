using Godot;
using System;

public partial class Scene_SwitchRoom : Node3D
{
	private Switch3D _nodeSwitch = null;
	private StaticBody3D _nodeBarrier = null;

	public override void _Ready()
	{
		_nodeSwitch = GetNode<Switch3D>("LevelWrapper/TextBoxWrapper/Switch3D");
		_nodeBarrier = GetNode<StaticBody3D>("LevelWrapper/TextBoxWrapper/Barrier");
		_nodeSwitch.SwitchFlip += HandleSwitchFlip;
	}

	bool switchBit = false;
	private void HandleSwitchFlip()
	{
		GD.Print("call HandleSwitchFlip");
		if (!switchBit)
		{
			_nodeBarrier.Position += new Vector3(50, 0, 0);
		}
	}
}
