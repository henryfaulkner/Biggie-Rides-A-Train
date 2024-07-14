using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class TextBoxService : Node
{
    public InteractionTextBox InteractionTextBox { get; set; }
    public TextBox TextBox { get; set; }

    // HoverTextBox is not participating in the ProcessQueue
    public HoverTextBox HoverTextBox { get; set; }

    private Queue<TextBoxProcess> ProcessQueue { get; set; }
    private TextBoxProcess? CurrentProcess { get; set; }
    private bool IsInboundQueuing { get; set; }

    public override void _Ready()
    {
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

    public void CompleteProcess(TextBoxProcess process)
    {
        ProcessQueue.Dequeue();
        CurrentProcess = null;
        IsInboundQueuing = true;
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
}