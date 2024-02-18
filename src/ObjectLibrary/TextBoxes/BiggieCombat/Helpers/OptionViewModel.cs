using Godot;
using System;

public partial class OptionViewModel : Container
{
	private static readonly StringName _STYLEBOX_NAME = new StringName("SelectionPanelStyleBox");
	
	private Container _nodeSelf = null;
	private Panel _nodeSelectionPanel = null;
	private Label _nodeOptionLabel = null;

	public int Id { get; set; }
	public bool IsSelected { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Container>(".");
		_nodeSelectionPanel = GetNode<Panel>("./Button/Panel");
		_nodeOptionLabel = GetNode<Label>("./HBoxContainer/MarginContainer/Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsSelected)
		{	
			_nodeOptionLabel.LabelSettings.FontColor = new Color(0xffffffff);
			AddSelectBorder();
		}
		else
		{
			_nodeOptionLabel.LabelSettings.FontColor = new Color(0x707070ff);
			RemoveSelectBorder();
		}
	}
	
	private void AddSelectBorder() 
	{
		StyleBoxFlat newStyleboxNormal = _nodeSelectionPanel.GetThemeStylebox(_STYLEBOX_NAME).Duplicate() as StyleBoxFlat;
		newStyleboxNormal.BorderWidthLeft = 2;
		newStyleboxNormal.BorderWidthTop = 2;
		newStyleboxNormal.BorderWidthRight = 2;
		newStyleboxNormal.BorderWidthBottom = 2;
		_nodeSelectionPanel.AddThemeStyleboxOverride(_STYLEBOX_NAME, newStyleboxNormal);
	}
	
	private void RemoveSelectBorder()
	{
		StyleBoxFlat newStyleboxNormal = _nodeSelectionPanel.GetThemeStylebox(_STYLEBOX_NAME).Duplicate() as StyleBoxFlat;
		newStyleboxNormal.BorderWidthLeft = 0;
		newStyleboxNormal.BorderWidthTop = 0;
		newStyleboxNormal.BorderWidthRight = 0;
		newStyleboxNormal.BorderWidthBottom = 0;
		_nodeSelectionPanel.AddThemeStyleboxOverride(_STYLEBOX_NAME, newStyleboxNormal);
	}
}
