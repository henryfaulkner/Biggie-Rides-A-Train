using Godot;
using System;
using System.Threading.Tasks;

public partial class TextBox : CanvasLayer
{
	private static readonly float _CHAR_READ_RATE = .1f;
	
	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private Label _nodeStart = null;
	private Label _nodeDialogue = null;
	private Label _nodeEnd = null;	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeStart = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Start");
		_nodeDialogue = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Dialogue");
		_nodeEnd = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/End");
		HideTextBox();
		AddDialogue("SHEEEEEEESSSSSHHH");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public void ShowTextBox() 
	{	
		_nodeStart.Text = "* ";
		_nodeTextBoxContainer.Show();
		GetTree().Paused = true;
	}
	
	public void HideTextBox() 
	{
		_nodeStart.Text = string.Empty;
		_nodeDialogue.Text = string.Empty;
		_nodeEnd.Text = string.Empty;
		_nodeTextBoxContainer.Hide();
		GetTree().Paused = false;
	}
	
	public async Task AddDialogue(string dialogue) 
	{
		_nodeDialogue.Text = dialogue;
		ShowTextBox();	
		
		_nodeDialogue.VisibleCharacters = 0;
		int len = dialogue.Length;
		TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(_CHAR_READ_RATE)));
		for (int i = 0; i < len; i++) 
		{
			_nodeDialogue.VisibleCharacters += 1;
			await Task.Delay(span);
		}
	}
}
