using Godot;
using System;
using System.Collections.Generic;

public partial class DjAttackContainer : MarginContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// String.Concat(charList);  
		var cps = new CompositionParsingService("composition-1.txt");
		var c = new List<char>();
		do {
			c = cps.GetNextToken();
			GD.Print(String.Concat(c));
		} while (c[0] != CompositionTokens.EOC);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
