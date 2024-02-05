using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class TextBox : CanvasLayer
{
	private static readonly float _CHAR_READ_RATE = .005f;
	private static readonly int _DEFAULT_PAGE_LENGTH = 175;
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	
	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private Label _nodeStart = null;
	private Label _nodeDialogue = null;
	private Label _nodeEnd = null;	
	
	private Queue<List<string>> _dialogueListQueue = null;
	private List<string> _dialogueList = null; // splice of single dialogue instance
	private int _dialogueListPointer = 0;
	
	private bool _isOpen = false;
	private bool _isTextMoving = false;
	private bool _isReading = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeStart = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Start");
		_nodeDialogue = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Dialogue");
		_nodeEnd = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/End");
		_dialogueListQueue = new Queue<List<string>>();
		_dialogueList = new List<string>();
		HideTextBox();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// ASSUMING INPUTMAP HAS A MAPPING FOR interact
		if (Input.IsActionJustPressed(_INTERACT_INPUT) && !_isTextMoving) 
		{
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
		if (IsOpen()) 
		{
			GD.Print("Do NOT add dialogue while the TextBox is open. Check CanCreateDialogue method before AddDialogue is called");
			return;
		}
		
		_dialogueListQueue.Enqueue(SplitDialogue(fullDialogue, _DEFAULT_PAGE_LENGTH));
	}
	
	public void ExecuteDialogueQueue() 
	{
		//DebugDialogueQueue();
		_isReading = true;
		if (GetDialogueListQueueCount() > 0)
		{
			_dialogueList = _dialogueListQueue.Dequeue();
			ShowTextBox();	
			ReadDialogue(_dialogueList[0]);	
		}
	}
	
	private async Task ReadDialogue(string dialogue) 
	{
		_nodeDialogue.Text = dialogue;
		_nodeDialogue.VisibleCharacters = 0;
		_isTextMoving = true;
		int len = dialogue.Length;
		TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(_CHAR_READ_RATE)));
		for (int i = 0; i < len; i++) 
		{
			// ASSUMING INPUTMAP HAS A MAPPING FOR interact
			//if (Input.IsActionJustPressed(_INTERACT_INPUT) && _isTextMoving) 
			//{
				//_nodeDialogue.VisibleCharacters = len;
				//_isTextMoving = false;
				//break;
			//}
			
			_nodeDialogue.VisibleCharacters += 1;
			await Task.Delay(span);
		}
		_isTextMoving = false;
	}
	
	public void AdvanceTextBox() 
	{
		_dialogueListPointer += 1;
		if (_dialogueListPointer >= _dialogueList.Count) 
		{
			if(GetDialogueListQueueCount() > 0)
			{
				_dialogueList = _dialogueListQueue.Dequeue();
				ReadDialogue(_dialogueList[0]);	
				_dialogueListPointer = 0;
			}
			else 
			{
				HideTextBox();
			}
		}
		else 
		{
			ReadDialogue(_dialogueList[_dialogueListPointer]);
		}
	}
	
	public void HideTextBox() 
	{
		_isOpen = false;
		_nodeStart.Text = string.Empty;
		_nodeDialogue.Text = string.Empty;
		_nodeEnd.Text = string.Empty;
		_dialogueList.Clear();
		_dialogueListPointer = 0;
		_nodeDialogue.VisibleCharacters = 0;
		_nodeTextBoxContainer.Hide();
	}
	
	public bool IsOpen() {
		return _isOpen;
	}
	
	public int GetDialogueListQueueCount() 
	{
		GD.Print("_dialogueListQueue.Count, ", _dialogueListQueue.Count);
		return _dialogueListQueue.Count;
	}
	
	public bool CanCreateDialogue() {
		if (!_isOpen && IsReading())
		{
			IsReading(false);
			return false;
		}
		return !_isOpen && !IsReading();
	}
	
	private void DebugDialogueQueue() 
	{
		GD.Print("**** START DebugDialogueQueue ****");
		foreach (List<string> queueEntry in _dialogueListQueue)
		{
			GD.Print("**** START QUEUE ENTRY ****");
			foreach (string dialogue in queueEntry) 
			{
				GD.Print("**** START PAGE ENTRY ****");
				GD.Print(dialogue);
				GD.Print("**** END PAGE ENTRY ****");
			}
			GD.Print("**** END QUEUE ENTRY ****");
		}
		GD.Print("**** END DebugDialogueQueue ****");
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
	
	public bool IsReading() 
	{
		return _isReading;
	}
	
	public void IsReading(bool isReading)
	{
		_isReading = isReading;
	}
}
