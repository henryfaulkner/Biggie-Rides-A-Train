using Godot;
using System;

public partial class CombatWrapper : Node2D
{
	private Panel _nodeSubjectPanel = null;
	private Panel _nodeBaseTextPanel = null;
	private Panel _nodeAttackPanel = null;
	private HBoxContainer _nodeHudContainerSubject = null;
	private HBoxContainer _nodeHudContainerTargetDown = null;
	private HBoxContainer _nodeHudContainerTargetUp = null;
	private MarginContainer _nodeActionInfo = null;

	private MarginContainer _nodeTextContainer = null;
	private MarginContainer _nodeAttackContainer = null;

	private PanelAnimationHelper HudAnimationHelper { get; set; }
	private static readonly float _HUD_SPEED_SLOW = 3f;
	private static readonly float _HUD_SPEED_FAST = 8f;
	private PanelAnimationHelper MainAnimationHelper { get; set; }
	private static readonly float _MAIN_SPEED = 20f;

	public bool Attacking { get; set; }

	public override void _Ready()
	{
		// note the containers wrapping the panels must have the same position and size
		// the containers can use margin to change panel size for animation
		_nodeSubjectPanel = GetNode<Panel>("./TransformContainer/TransformPanel");
		_nodeBaseTextPanel = GetNode<Panel>("./BiggieCombatTextBox/TextBoxContainer/BasePagePanel");
		_nodeAttackPanel = GetNode<Panel>("./SquareAttackContainer/SquareAttackPanel");
		_nodeHudContainerSubject = GetNode<HBoxContainer>("./HudContainer");
		// Need to Queue Free this at some point
		_nodeHudContainerTargetDown = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 250);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetUp = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 0);
		HudAnimationHelper = new PanelAnimationHelper(_HUD_SPEED_SLOW);
		MainAnimationHelper = new PanelAnimationHelper(_MAIN_SPEED);

		_nodeActionInfo = GetNode<MarginContainer>("HudContainer/ActionInfo");
		_nodeTextContainer = GetNode<MarginContainer>("./BiggieCombatTextBox/TextBoxContainer");
		_nodeAttackContainer = GetNode<MarginContainer>("./SquareAttackContainer");
	}

	public override void _Process(double delta)
	{
	}

	public bool TranslateHudDown()
	{
		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetDown))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_SLOW;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetDown);
		return false;
	}

	public bool TranslateHudUp()
	{
		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetUp))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetUp);
		return false;
	}

	public bool TransformToAttack()
	{
		GD.Print("TransformToAttack");
		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeAttackPanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeAttackPanel))
		{
			_nodeAttackContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeAttackPanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeAttackPanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeAttackPanel);
		}
		else if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeAttackPanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeAttackPanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeAttackPanel);
		}
		else
		{
			//GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeAttackPanel)}");
		}
		return false;
	}

	public bool TransformToText()
	{
		GD.Print("TransformToText");
		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeBaseTextPanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeBaseTextPanel))
		{
			_nodeTextContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeBaseTextPanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBaseTextPanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeBaseTextPanel);
		}
		else if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeBaseTextPanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeBaseTextPanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeBaseTextPanel);
		}
		else
		{
			//GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBaseTextPanel)}");
		}
		return false;
	}

	private HBoxContainer CreateHudContainerTarget(HBoxContainer subject, float diffX, float diffY)
	{
		var result = _nodeHudContainerSubject.Duplicate() as HBoxContainer;
		result.Position = new Vector2(
			result.Position.X + diffX, result.Position.Y + diffY
		);
		return result;
	}

	public void ShowAttackContainer()
	{
		_nodeAttackContainer.Show();
	}

	public void HideAttackContainer()
	{
		_nodeAttackContainer.Hide();
	}

	public void ShowTextContainer()
	{
		_nodeTextContainer.Show();
	}

	public void HideTextContainer()
	{
		_nodeTextContainer.Hide();
	}

	public void ShowActionInfo()
	{
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideActionInfo()
	{
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 0);
	}
}
