using System;
using Godot;

public abstract partial class TextBoxProcess : CanvasLayer
{
    [Signal]
    public delegate void CompleteProcessEventHandler();
    public abstract void ExecuteProcess();
}