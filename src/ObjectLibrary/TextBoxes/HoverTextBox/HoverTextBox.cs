using Godot;
using System;

public partial class HoverTextBox : CanvasLayer
{


	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private RichTextLabel _nodeText = null;

	private bool IsOpen { get; set; }

	private LoggingService _globalLoggerSingleton = null;

	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeText = GetNode<RichTextLabel>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Dialogue");

		HideTextBox();
		ClearText();
	}

	public void ShowTextBox()
	{
		IsOpen = true;
		_nodeTextBoxContainer.Show();
	}

	public void HideTextBox()
	{
		IsOpen = false;
		_nodeTextBoxContainer.Hide();
	}

	public void SetText(string text)
	{
		_nodeText.Text = text;
	}

	public void ClearText()
	{
		_nodeText.Text = string.Empty;
	}
}
