using Godot;
using System;

public partial class Scene_ButtonRoom : Node3D
{
	private PressurePlate _nodePressurePlate1 = null;
	private PressurePlate _nodePressurePlate2 = null;
	private PressurePlate _nodePressurePlate3 = null;
	private PressurePlate _nodePressurePlate4 = null;
	private PressurePlate _nodePressurePlate5 = null;
	private StaticBody3D _nodeBarrier = null;

	private SaveStateService _serviceSaveState = null;

	private bool bitPressurePlate1 = false;
	private bool bitPressurePlate2 = false;
	private bool bitPressurePlate3 = false;
	private bool bitPressurePlate4 = false;
	private bool bitPressurePlate5 = false;

	public override void _Ready()
	{
		_nodePressurePlate1 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlate1");
		_nodePressurePlate2 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlate2");
		_nodePressurePlate3 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlate3");
		_nodePressurePlate4 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlate4");
		_nodePressurePlate5 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlate5");
		_nodePressurePlate1.Pressed += () =>
		{
			bitPressurePlate1 = true;
			if (CheckAllPressurePlates()) ProcessAllPressed();
		};
		_nodePressurePlate2.Pressed += () =>
		{
			bitPressurePlate2 = true;
			if (CheckAllPressurePlates()) ProcessAllPressed();
		};
		_nodePressurePlate3.Pressed += () =>
		{
			bitPressurePlate3 = true;
			if (CheckAllPressurePlates()) ProcessAllPressed();
		};
		_nodePressurePlate4.Pressed += () =>
		{
			bitPressurePlate4 = true;
			if (CheckAllPressurePlates()) ProcessAllPressed();
		};
		_nodePressurePlate5.Pressed += () =>
		{
			bitPressurePlate5 = true;
			if (CheckAllPressurePlates()) ProcessAllPressed();
		};

		_nodeBarrier = GetNode<StaticBody3D>("LevelWrapper/TextBoxWrapper/Barrier");

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		var context = _serviceSaveState.Load();
		if (context.IsButtonDoorOpen)
		{
			bitPressurePlate1 = true;
			bitPressurePlate2 = true;
			bitPressurePlate3 = true;
			bitPressurePlate4 = true;
			bitPressurePlate5 = true;
			if (CheckAllPressurePlates()) ProcessAllPressed();
		}
	}

	private void ProcessAllPressed()
	{
		MoveBarrier();
		var context = _serviceSaveState.Load();
		context.IsButtonDoorOpen = true;
		_serviceSaveState.Commit(context);
	}

	private bool CheckAllPressurePlates()
	{
		return bitPressurePlate1
			&& bitPressurePlate2
			&& bitPressurePlate3
			&& bitPressurePlate4
			&& bitPressurePlate5;
	}

	private void MoveBarrier()
	{
		GD.Print("call MoveBarrier");
		_nodeBarrier.Position += new Vector3(50, 0, 0);
	}
}
