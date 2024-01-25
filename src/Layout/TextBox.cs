using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class TextBox : CanvasLayer
{
	private static readonly float _CHAR_READ_RATE = .01f;
	private static readonly int _DEFAULT_PAGE_LENGTH = 160;
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	
	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private Label _nodeStart = null;
	private Label _nodeDialogue = null;
	private Label _nodeEnd = null;	
	
	private List<string> _dialogueList = null;
	private int _dialoguePointer = 0;
	
	private bool _isOpen = false;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeStart = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Start");
		_nodeDialogue = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Dialogue");
		_nodeEnd = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/End");
		_dialogueList = new List<string>();
		HideTextBox();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// ASSUMING INPUTMAP HAS A MAPPING FOR interact
		if (Input.IsActionJustPressed(_INTERACT_INPUT)) {
			AdvanceTextBox();
		}
	}
	
	public void ShowTextBox() 
	{	
		_isOpen = true;
		_nodeStart.Text = "* ";
		_nodeTextBoxContainer.Show();
	}
	
	public void AddDialogue(string fullDialogue) 
	{
		_nodeDialogue.Text = fullDialogue;
		_dialogueList = SplitDialogue(fullDialogue, _DEFAULT_PAGE_LENGTH);
		ShowTextBox();	
		ReadDialogue(_dialogueList[0]);
	}
	
	private List<string> SplitDialogue(string fullDialogue, int pageLength) 
	{
		List<string> result = new List<string>();
		int fullDialogueCharCount = fullDialogue.Length;
		int fullPageCount = Convert.ToInt32(Math.Floor((double)(fullDialogueCharCount / pageLength)));
		int excessPageCharCount = fullDialogueCharCount - (fullPageCount * pageLength);
		
		int offset = 0;
		for (int i = 0; i < fullPageCount; i += 1) 
		{
			result.Add(fullDialogue.Substring(offset, pageLength));	
			offset += pageLength;
		}
		// Account for excess
		if (excessPageCharCount > 0) result.Add(fullDialogue.Substring(offset, excessPageCharCount));
		return result;
	}
	
	private async Task ReadDialogue(string dialogue) 
	{
		_nodeDialogue.VisibleCharacters = 0;
		int len = dialogue.Length;
		TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(_CHAR_READ_RATE)));
		for (int i = 0; i < len; i++) 
		{
			_nodeDialogue.VisibleCharacters += 1;
			await Task.Delay(span);
		}
	}
	
	public void AdvanceTextBox() 
	{
		_dialoguePointer += 1;
		if (_dialoguePointer >= _dialogueList.Count) 
		{
			HideTextBox();
		}
		else 
		{
			_nodeDialogue.Text = _dialogueList[_dialoguePointer];
			ReadDialogue(_dialogueList[_dialoguePointer]);
		}
	}
	
	public void HideTextBox() 
	{
		_isOpen = false;
		_nodeStart.Text = string.Empty;
		_nodeDialogue.Text = string.Empty;
		_nodeEnd.Text = string.Empty;
		_dialogueList.Clear();
		_dialoguePointer = 0;
		_nodeTextBoxContainer.Hide();
	}
	
	public bool IsOpen() {
		return _isOpen;
	}
}
