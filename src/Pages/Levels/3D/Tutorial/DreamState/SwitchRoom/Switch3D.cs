using Godot;
using System;

public partial class Switch3D : Node3D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly int _OFF_FRAME = 0;
	private static readonly int _ON_ANIMATE_FRAME = 1;
	private static readonly int _ON_FRAME = 2;
	private static readonly int _OFF_ANIMATE_FRAME = 3;
	private static readonly int _FRAMES_OF_ANIMATION = 10;
	
	private Sprite3D _nodeSprite = null;
	private Area3D _nodeInteractableArea = null;
	
	public bool SwitchState {get;set;}
	public bool IsAnimating {get;set;}
	public int FrameIndex {get;set;}
	
	public override void _Ready()
	{
		_nodeSprite = GetNode<Sprite3D>("./InteractableArea3D/Sprite3D");
		_nodeInteractableArea = GetNode<Area3D>("./InteractableArea3D");
		_nodeSprite.Frame = _OFF_FRAME;
		SwitchState = false;
		IsAnimating = false;
		FrameIndex = 0;
	}
	
	public override void _Process(double _delta)
	{
		if (HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies())
			&& Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			EmitSignal(SignalName.SwitchFlip);
			SwitchState = !SwitchState;
			IsAnimating = true;
			FrameIndex = 0;
			
			if (_nodeSprite.Frame == _OFF_FRAME || _nodeSprite.Frame == _OFF_ANIMATE_FRAME)
			{
				_nodeSprite.Frame = _ON_ANIMATE_FRAME;
			}
			else if (_nodeSprite.Frame == _ON_FRAME || _nodeSprite.Frame == _ON_ANIMATE_FRAME)
			{
				_nodeSprite.Frame = _OFF_ANIMATE_FRAME;
			}
		}
		
		if (IsAnimating)
		{
			if (FrameIndex > _FRAMES_OF_ANIMATION) {
				if (_nodeSprite.Frame == _OFF_ANIMATE_FRAME)
			{
				_nodeSprite.Frame = _ON_FRAME;
			}
			else if (_nodeSprite.Frame == _ON_ANIMATE_FRAME)
			{
				_nodeSprite.Frame = _ON_FRAME;
			}
				IsAnimating = false;
			}
			FrameIndex += 1;
		}
	}
	
	[Signal]
	public delegate void SwitchFlipEventHandler();
}
