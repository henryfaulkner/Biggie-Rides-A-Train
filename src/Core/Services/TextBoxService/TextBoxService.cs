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

    public override void _Ready()
    {
        _nodeTextBoxWrapper = (TextBoxWrapper)GetNode("/root").FindChild("TextBoxWrapper");

        ProcessQueue = new Queue<TextBoxProcess>();
        CurrentProcess = null;
        IsInboundQueuing = true;
    }

    public override void _Process(double delta)
    {
        if (!IsInboundQueuing && CurrentProcess == null)
        {
            if (!ProcessQueue.Any())
            {
                ResumeInboundQueuing();
                return;
            }

            CurrentProcess = ProcessQueue.Dequeue();
        }
    }

    public void ExecuteQueuedProcesses()
    {
        throw new NotImplementedException();
    }

    public void CompleteProcess(TextBoxProcess process)
    {
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
        CurrentProcess = null;
        IsInboundQueuing = true;
    }

    public InteractionTextBox CreateInteractionTextBox()
    {
        var result = TextBoxFactory.SpawnInteractionTextBox();
        return result;
    }

    public TextBox CreateTextBox()
    {
        var result = TextBoxFactory.SpawnTextBox();
        return result;
    }

    public HoverTextBox CreateHoverTextBox()
    {
        var result = TextBoxFactory.SpawnHoverTextBox();
        return result;
    }
}