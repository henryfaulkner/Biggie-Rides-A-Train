using Godot;
using System;

public partial class CombatSceneTestBattle : Node2D
{
	private Panel _nodeSubjectPanel = null;
	private Panel _nodeTargetPanel = null;
	private HBoxContainer _nodeOptionContainer = null;
	private HBoxContainer _nodeHudContainerSubject = null;
	private HBoxContainer _nodeHudContainerTarget = null;

	private PanelAnimationHelper HudAnimationHelper { get; set; }
	private PanelAnimationHelper MainAnimationHelper { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// note the containers wrapping the panels must have the same position and size
		// the containers can use margin to change panel size for animation
		_nodeSubjectPanel = GetNode<Panel>("./CombatWrapper/BiggieCombatTextBox/TextBoxContainer/BasePagePanel");
		_nodeTargetPanel = GetNode<Panel>("./CombatWrapper/EnemyAttackContainer/EnemyAttackPanel");
		_nodeOptionContainer = GetNode<HBoxContainer>("./CombatWrapper/BiggieCombatTextBox/TextBoxContainer/BasePagePanel/MarginContainer/OptionContainer");
		_nodeHudContainerSubject = GetNode<HBoxContainer>("./CombatWrapper/HudContainer");
		_nodeHudContainerTarget = _nodeHudContainerSubject.Duplicate() as HBoxContainer;
		_nodeHudContainerTarget.Position = new Vector2(
			_nodeHudContainerTarget.Position.X, _nodeHudContainerTarget.Position.Y + 250
		);
		HudAnimationHelper = new PanelAnimationHelper(3f);
		MainAnimationHelper = new PanelAnimationHelper(20f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_nodeOptionContainer.Visible = false;

		if (!HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTarget))
		{
			HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTarget);
		}

		if (!MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeTargetPanel)
			|| !MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeTargetPanel))
		{
			if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeTargetPanel)
				|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeTargetPanel))
			{
				GD.Print("CenterScaleX");
				GD.Print($"Position {MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeTargetPanel)}");
				GD.Print($"Size {MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeTargetPanel)}");
				MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeTargetPanel);
			}
			else if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeTargetPanel)
				|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeTargetPanel))
			{
				GD.Print("MonoScaleY");
				MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeTargetPanel);
			}
			else
			{
				GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeTargetPanel)}");
			}

		}
	}
}
