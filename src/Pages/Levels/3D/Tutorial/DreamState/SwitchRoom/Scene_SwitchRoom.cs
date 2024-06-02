using Godot;
using System;

public partial class Scene_SwitchRoom : Node3D
{
	private Switch3D _nodeSwitch = null;
	private StaticBody3D _nodeBarrier = null;

	private SaveStateService _serviceSaveState = null;

	public override void _Ready()
	{
		_nodeSwitch = GetNode<Switch3D>("LevelWrapper/TextBoxWrapper/Switch3D");
		_nodeBarrier = GetNode<StaticBody3D>("LevelWrapper/TextBoxWrapper/Barrier");
		_nodeSwitch.SwitchFlip += HandleSwitchFlip;

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		var context = _serviceSaveState.Load();
		if (context.IsSwitchDoorOpen)
		{
			HandleSwitchFlip();
		}
	}

	private void HandleSwitchFlip()
	{
		//GD.Print("call HandleSwitchFlip");
		_nodeBarrier.Position += new Vector3(50, 0, 0);
		var context = _serviceSaveState.Load();
		context.IsSwitchDoorOpen = true;
		_serviceSaveState.Commit(context);
	}
}
