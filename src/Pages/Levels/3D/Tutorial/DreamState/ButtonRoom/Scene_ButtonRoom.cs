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
	private RotationPaws _nodeRotationPaws = null;
	private RotationPaws _nodeRotationPaws2 = null;

	private SaveStateService _serviceSaveState = null;
	private RotationService _serviceRotation = null;
	private GravityService _serviceGravity = null;

	private bool bitPressurePlate1 = false;
	private bool bitPressurePlate2 = false;
	private bool bitPressurePlate3 = false;
	private bool bitPressurePlate4 = false;
	private bool bitPressurePlate5 = false;

	public override void _Ready()
	{
		_nodePressurePlate1 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlates/PressurePlate1");
		_nodePressurePlate2 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlates/PressurePlate2");
		_nodePressurePlate3 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlates/PressurePlate3");
		_nodePressurePlate4 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlates/PressurePlate4");
		_nodePressurePlate5 = GetNode<PressurePlate>("LevelWrapper/TextBoxWrapper/PressurePlates/PressurePlate5");
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
		_nodeRotationPaws = GetNode<RotationPaws>("./LevelWrapper/TextBoxWrapper/RotationPaws");
		_nodeRotationPaws.Rotate += HandleForwardRotate;
		_nodeRotationPaws2 = GetNode<RotationPaws>("./LevelWrapper/TextBoxWrapper/RotationPaws2");
		_nodeRotationPaws2.Rotate += HandleForwardRotate;

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		_serviceRotation = GetNode<RotationService>("/root/RotationService");
		_serviceGravity = GetNode<GravityService>("/root/GravityService");
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
		//GD.Print("call MoveBarrier");
		_nodeBarrier.Position += new Vector3(50, 0, 0);
	}

	public void HandleForwardRotate()
	{
		//GD.Print("HandleForwardRotate");
		switch (_serviceRotation.CurrentRotation)
		{
			case Enumerations.Physics.Rotations.Default:
				//GD.Print("Enumerations.Physics.Rotations.Default");
				_serviceRotation.RotateToForward();
				_serviceGravity.SetForwardGravity();
				break;
			case Enumerations.Physics.Rotations.Forward:
				//GD.Print("Enumerations.Physics.Rotations.Forward");
				_serviceRotation.RotateToDefault();
				_serviceGravity.SetDefaultGravity();
				break;
			default:
				//GD.Print("RotationService CurrentRotation could not be mapped.");
				break;
		}
	}
}
