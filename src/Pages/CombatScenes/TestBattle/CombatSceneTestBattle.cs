using Godot;
using System;

public partial class CombatSceneTestBattle : Node2D
{
	private Panel _nodeSubjectPanel = null;
	private Panel _nodeTargetPanel = null;

	private PanelAnimationHelper PanelAnimationHelper { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSubjectPanel = GetNode<Panel>("./SubjectPanel");
		_nodeTargetPanel = GetNode<Panel>("./TargetPanel");
		PanelAnimationHelper = new PanelAnimationHelper();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PanelAnimationHelper.CenterScaleHAndUniScaleYOverTime(0.0, _nodeTargetPanel, _nodeSubjectPanel);
	}
}
