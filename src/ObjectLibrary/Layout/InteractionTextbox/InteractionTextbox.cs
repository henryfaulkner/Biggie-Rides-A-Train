using Godot;
using System;
using System.Collections.Generic;

public partial class InteractionTextBox : CanvasLayer
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _OPTION_CONTAINER_SCENE = new StringName("interact");
	
	private CanvasLayer _nodeSelf = null;
	private VBoxContainer _nodeVBoxContainer = null;
	private Label _nodePromptLabel = null;
	
	private List<OptionContainer> _optionContainerList;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<CanvasLayer>(".");
		_nodeVBoxContainer = GetNode<VBoxContainer>("./TextBoxContainer/Panel/MarginContainer/VBoxContainer");
		_nodePromptLabel = GetNode<Label>("./TextBoxContainer/Panel/MarginContainer/VBoxContainer/PromptContainer/Prompt");
		
		_optionContainerList = new List<OptionContainer>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// call this to instantiate an InteractionTextBox with 
	// an initial prompt and default option
	public void StartInteration(string prompt, string firstOption) 
	{
		_nodePromptLabel.Text = prompt;
		AddOption(firstOption);
	}
	
	// call this to add an additional option to the Option list
	public void AddOption(string option)
	{
		// I could iomprove the performance of this section by preloading the scene
		// but I would need to convert code to GDScript to do so
		var scene = GD.Load<PackedScene>("res://MyScene.tscn");
	}
	
	// call this to display the constructed prompt and option list
	// to the screen
	public void DisplayConstructedTransaction() 
	{
		
	}
	
	// respond to an option, whose selection was submitted
	public void HandleInteraction() 
	{
		
	}
	
	private void ClearInteraction() 
	{
		
	}
}
