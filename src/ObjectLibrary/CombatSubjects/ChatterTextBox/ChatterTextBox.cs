using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class ChatterTextBox : CanvasLayer
{
	private static readonly int _DEFAULT_PAGE_LENGTH = 225;
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");

	private CanvasLayer _nodeSelf = null;
	private MarginContainer _nodeTextBoxContainer = null;
	private Label _nodeStart = null;
	private RichTextLabel _nodeDialogue = null;
	private Label _nodeEnd = null;

	private Queue<List<string>> _dialogueListQueue = null;
	private List<string> _dialogueList = null; // splice of single dialogue instance
	private int _dialogueListPointer = 0;

	private bool _isOpen = false;
	private bool _isTextMoving = false;
	private bool _isReading = false;

	private float CharReadRate { get; set; }

	private LoggingService _globalLoggerSingleton = null;
	private CombatSingleton _globalCombatSingleton = null;

	public override void _Ready()
	{
		CharReadRate = 100f;
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeTextBoxContainer = GetNode<MarginContainer>("./TextBoxContainer");
		_nodeStart = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Start");
		_nodeDialogue = GetNode<RichTextLabel>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/Dialogue");
		_nodeEnd = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/HBoxContainer/End");
		_dialogueListQueue = new Queue<List<string>>();
		_dialogueList = new List<string>();
		HideTextBox();

		//_globalLogger = GetNode<LoggingService>("/root/LoggingService");
		//_globalLogger.LogError("I'm gonna lose it");
		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
	}

	public override void _Process(double delta)
	{
		if (!IsOpen()) return;
		// ASSUMING INPUTMAP HAS A MAPPING FOR interact
		if (Input.IsActionJustPressed(_INTERACT_INPUT))// && !_isTextMoving) 
		{
			if (_isTextMoving)
			{
				CharReadRate = 5000f;
			}
			else
			{
				_nodeDialogue.Clear();
				AdvanceTextBox();
			}
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
			////GD.Print("Do NOT add dialogue while the TextBox is open. Check CanCreateDialogue method before AddDialogue is called");
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
		// BBCode
		//https://www.youtube.com/watch?v=2Womvf8Uemk&t=392s
		_nodeDialogue.AppendText(dialogue); ;
		_nodeDialogue.VisibleCharacters = 0;
		_isTextMoving = true;
		int len = dialogue.Length;
		for (int i = 0; i < len; i++)
		{
			TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(1 / CharReadRate)));
			_nodeDialogue.VisibleCharacters += 1;
			await Task.Delay(span);
		}
		_isTextMoving = false;
	}

	public void AdvanceTextBox()
	{
		CharReadRate = 100f;
		_dialogueListPointer += 1;
		if (_dialogueListPointer >= _dialogueList.Count)
		{
			if (GetDialogueListQueueCount() > 0)
			{
				_dialogueList = _dialogueListQueue.Dequeue();
				ReadDialogue(_dialogueList[0]);
				_dialogueListPointer = 0;
			}
			else
			{
				HideTextBox();
				EmitSignal(SignalName.HidingTextBox);
				EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishChatterTextBox);
			}
		}
		else
		{
			ReadDialogue(_dialogueList[_dialogueListPointer]);
		}
	}

	[Signal]
	public delegate void HidingTextBoxEventHandler();

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

	public bool IsOpen()
	{
		return _isOpen;
	}

	public int GetDialogueListQueueCount()
	{
		return _dialogueListQueue.Count;
	}

	public bool CanCreateDialogue()
	{
		if (!_isOpen && IsReading())
		{
			IsReading(false);
			return false;
		}
		return !_isOpen && !IsReading();
	}

	private void DebugDialogueQueue()
	{
		////GD.Print("**** START DebugDialogueQueue ****");
		foreach (List<string> queueEntry in _dialogueListQueue)
		{
			////GD.Print("**** START QUEUE ENTRY ****");
			foreach (string dialogue in queueEntry)
			{
				////GD.Print("**** START PAGE ENTRY ****");
				////GD.Print(dialogue);
				////GD.Print("**** END PAGE ENTRY ****");
			}
			////GD.Print("**** END QUEUE ENTRY ****");
		}
		////GD.Print("**** END DebugDialogueQueue ****");
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

	private static readonly StringName _COMBAT_EVENT = new StringName("CombatEvent");
	private void EmitCombatEvent(Enumerations.Combat.StateMachine.Events eventId)
	{
		GD.Print("CombatWrapper kill me");
		if (CheckChatterConditions()) return;
		_globalCombatSingleton.CombatStateMachineService.EmitSignal(_COMBAT_EVENT, (int)eventId);
	}

	private bool firstDialogueDone = false;
	private bool CheckChatterConditions()
	{
		GD.Print("ChatterTextBox CheckChatterConditions");
		if (_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetHealthPercentage() < 100 && !firstDialogueDone)
		{
			AddDialogue("Pizza Pizza.");
			AddDialogue("Please.");
			ExecuteDialogueQueue();
			_globalCombatSingleton.CombatStateMachineService.EmitSignal(_COMBAT_EVENT, (int)Enumerations.Combat.StateMachine.Events.ShowChatterTextBox);
			firstDialogueDone = true;
			return true;
		}
		return false;
	}
}
