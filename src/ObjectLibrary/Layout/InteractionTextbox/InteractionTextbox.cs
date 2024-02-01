using Godot;
using System;
using System.Collecions.Generic;

public partial class InteractionTextbox : CanvasLayer
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	
	private List<OptionContainer> _optionContainerList;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_optionContainerList = new List<OptionContainer>()
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void StartInteration(string prompt, string firstOption) 
	{
		
	}
	
	public void AddTransactionOption(string option)
	{
		
	}
	
	public void ExecuteTransaction() 
	{
		
	}
	
	public void HandleTransactionResult() 
	{
		
	}
}
