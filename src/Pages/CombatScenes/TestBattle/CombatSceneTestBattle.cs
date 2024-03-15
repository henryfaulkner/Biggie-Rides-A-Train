using Godot;
using System;

public partial class CombatSceneTestBattle : Node2D
{
	private SquareAttackContainer _nodeSquareAttackContainer = null;
	private BiggieCombatTextBox _nodeBiggieCombatTextBox = null;
	private Panel _nodeSubjectPanel = null;
	private Panel _nodeBaseTextPanel = null;
	private Panel _nodeAttackPanel = null;
	private HBoxContainer _nodeOptionContainer = null;
	private HBoxContainer _nodeHudContainerSubject = null;
	private HBoxContainer _nodeHudContainerTarget = null;
	
	private MarginContainer _nodeTextContainer = null;
	private MarginContainer _nodeAttackContainer = null;
	private MarginContainer _nodeDjAttackContainer = null;

	private PanelAnimationHelper HudAnimationHelper { get; set; }
	private PanelAnimationHelper MainAnimationHelper { get; set; }
	
	private bool Attacking { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// note the containers wrapping the panels must have the same position and size
		// the containers can use margin to change panel size for animation
		_nodeSubjectPanel = GetNode<Panel>("./CombatWrapper/TransformContainer/TransformPanel");
		_nodeBaseTextPanel = GetNode<Panel>("./CombatWrapper/BiggieCombatTextBox/TextBoxContainer/BasePagePanel");
		_nodeAttackPanel = GetNode<Panel>("./CombatWrapper/SquareAttackContainer/SquareAttackPanel");
		_nodeOptionContainer = GetNode<HBoxContainer>("./CombatWrapper/BiggieCombatTextBox/TextBoxContainer/BasePagePanel/MarginContainer/OptionContainer");
		_nodeHudContainerSubject = GetNode<HBoxContainer>("./CombatWrapper/HudContainer");
		_nodeHudContainerTarget = _nodeHudContainerSubject.Duplicate() as HBoxContainer;
		_nodeHudContainerTarget.Position = new Vector2(
			_nodeHudContainerTarget.Position.X, _nodeHudContainerTarget.Position.Y + 250
		);
		HudAnimationHelper = new PanelAnimationHelper(3f);
		MainAnimationHelper = new PanelAnimationHelper(20f);
		
		_nodeTextContainer = GetNode<MarginContainer>("./CombatWrapper/BiggieCombatTextBox/TextBoxContainer");
		_nodeAttackContainer = GetNode<MarginContainer>("./CombatWrapper/SquareAttackContainer");
		_nodeDjAttackContainer = GetNode<MarginContainer>("./CombatWrapper/DjAttackContainer");
		_nodeTextContainer.Hide();
		_nodeAttackContainer.Hide();
		_nodeDjAttackContainer.Hide();
		
		Attacking = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTarget))
		{
			HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTarget);
		}
		
		if (Attacking)
		{
			TransformToAttack();
		}
		else
		{
			TransformToText();
		}
	}
	
	public void TransformToAttack()
	{
		if (!MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeAttackPanel)
			|| !MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeAttackPanel))
		{
			if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeAttackPanel)
				|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeAttackPanel))
			{
				GD.Print("CenterScaleX");
				GD.Print($"Position {MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeAttackPanel)}");
				GD.Print($"Size {MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeAttackPanel)}");
				MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeAttackPanel);
			}
			else if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeAttackPanel)
				|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeAttackPanel))
			{
				GD.Print("MonoScaleY");
				MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeAttackPanel);
			}
			else
			{
				GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeAttackPanel)}");
			}
		}
		else 
		{
			_nodeAttackContainer.Show();
			_nodeDjAttackContainer.Show();
			Attacking = false;
		}
	}
	
	public void TransformToText()
	{
		if (!MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeBaseTextPanel)
			|| !MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeBaseTextPanel))
		{
			if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeBaseTextPanel)
				|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeBaseTextPanel))
			{
				GD.Print("CenterScaleX");
				GD.Print($"Position {MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeBaseTextPanel)}");
				GD.Print($"Size {MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeBaseTextPanel)}");
				MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeBaseTextPanel);
			}
			else if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeBaseTextPanel)
				|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBaseTextPanel))
			{
				GD.Print("MonoScaleY");
				MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeBaseTextPanel);
			}
			else
			{
				GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBaseTextPanel)}");
			}
		}
		else
		{
			_nodeTextContainer.Show();
		}
	}
}
