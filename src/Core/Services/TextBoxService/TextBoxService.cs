using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class TextBoxService : Node
{
	private TextBoxWrapper _nodeTextBoxWrapper = null;

	private Queue<TextBoxProcess> ProcessQueue { get; set; }
	private TextBoxProcess? CurrentProcess { get; set; }
	private bool IsInboundQueuing { get; set; }

	public TextBoxService()
	{
		ProcessQueue = new Queue<TextBoxProcess>();
		CurrentProcess = null;
		IsInboundQueuing = true;
	}

	public override void _Ready()
	{
		GD.Print("Call TextBoxService _Ready");
		_nodeTextBoxWrapper = GetTextBoxWrapper();
		if (_nodeTextBoxWrapper == null) GD.Print($"_nodeTextBoxWrapper is null");
	}

	public override void _Process(double delta)
	{
		if (!IsInboundQueuing && CurrentProcess == null)
		{
			if (ProcessQueue == null)
			{
				GD.PrintErr("TextBoxService ProcessQueue exit early. ProcessQueue is null.");
				return;
			}
			if (!ProcessQueue.Any())
			{
				GD.PrintErr("ExecuteQueuedProcesses exit early. ProcessQueue is empty.");
				ResumeInboundQueuing();
				return;
			}

			CurrentProcess = ProcessQueue.Dequeue();
			CurrentProcess.ExecuteProcess();
		}
		else
		{
			// GD.Print($"InboundQueuing {IsInboundQueuing}");
			// GD.Print($"CurrentProcess null? {CurrentProcess == null}");
		}
	}

	public void ExecuteQueuedProcesses()
	{
		IsInboundQueuing = false;
	}

	public void CompleteProcess(TextBoxProcess process)
	{
		GD.Print("Call CompleteProcesss");
		process.QueueFree();
		CurrentProcess = null;
	}

	public void EnqueueProcess(TextBoxProcess process)
	{
		process.CompleteProcess += () => CompleteProcess(process);
		ProcessQueue.Enqueue(process);
	}

	public void ResumeInboundQueuing()
	{
		IsInboundQueuing = true;
	}

	public InteractionTextBox CreateInteractionTextBox()
	{
		var result = TextBoxFactory.SpawnInteractionTextBox();

		// check if TextBox is disposed. Will occur on new scene.
		if (_nodeTextBoxWrapper == null || !IsInstanceValid(_nodeTextBoxWrapper)) _nodeTextBoxWrapper = GetTextBoxWrapper();

		_nodeTextBoxWrapper.AddChild(result);
		return result;
	}

	public TextBox CreateTextBox()
	{
		var result = TextBoxFactory.SpawnTextBox();
		if (_nodeTextBoxWrapper == null || !IsInstanceValid(_nodeTextBoxWrapper)) _nodeTextBoxWrapper = GetTextBoxWrapper();
		_nodeTextBoxWrapper.AddChild(result);
		return result;
	}

	public HoverTextBox CreateHoverTextBox()
	{
		var result = TextBoxFactory.SpawnHoverTextBox();
		if (_nodeTextBoxWrapper == null || !IsInstanceValid(_nodeTextBoxWrapper)) _nodeTextBoxWrapper = GetTextBoxWrapper();
		_nodeTextBoxWrapper.AddChild(result);
		return result;
	}

	public bool HasTextBoxOpen()
	{
		if (CurrentProcess != null
			|| ProcessQueue.Any())
		{
			return true;
		}
		return false;
	}

	private TextBoxWrapper GetTextBoxWrapper()
	{
		GD.Print("Call GetTextBoxWrapper");
		var result = (TextBoxWrapper)GetTree().Root.FindChild("TextBoxWrapper", true, false);
		if (result == null)
		{
			GD.PrintErr("Could not find TextBoxWrapper. returning null");
		}
		return result;
	}
}
