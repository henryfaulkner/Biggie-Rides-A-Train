using Godot;
using System;

public partial class DJ : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");
	private static readonly StringName _COMBAT_SCENE_DJ_BATTLE = new StringName("res://CombatScenes/DjBattle/CombatSceneDjBattle.tscn");
	
	private static readonly int _SPRITE_FRAME_MUSHROOM = 0;
	private static readonly int _SPRITE_FRAME_GOAT_IDLE = 1;
	private static readonly int _SPRITE_FRAME_GOAT_SPEAKING = 2;
	private static readonly int _SPRITE_FRAME_CHAIR = 3;
	
	private Node2D _nodeSelf = null;
	private Area2D _nodeInteractableArea = null;
	private Sprite2D _nodeGoatSprites = null;
	private TextBox _nodeTextBox = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSelf = GetNode<Node2D>(".");
		_nodeInteractableArea = GetNode<Area2D>("./InteractableArea");
		_nodeGoatSprites = GetNode<Sprite2D>("./VBoxContainer/Goat/Sprite2D");
		_nodeTextBox = GetNode<TextBox>("../TextBox");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_nodeInteractableArea.GetOverlappingBodies().Count > 2
			&& Input.IsActionJustPressed(_INTERACT_INPUT)) 
		{
			DisplayDialogue();
		}
		
		if (_nodeTextBox.IsOpen()) 
		{
			_nodeGoatSprites.Frame = _SPRITE_FRAME_GOAT_SPEAKING;
		}
		else 
		{
			_nodeGoatSprites.Frame = _SPRITE_FRAME_GOAT_IDLE;
		}
	}
	
	private void DisplayDialogue() 
	{
		if (!_nodeTextBox.CanCreateDialogue()) return;
		using (var context = new SaveStateContext())
		{
			var contextState = context.Load();
			switch(contextState.DialogueStateDJ)
			{
				case Enumerations.DialogueStates.DJ.Introduce:
					_nodeTextBox.AddDialogue("Hi. Welcome to The Club. Please stand up.");
					_nodeTextBox.ExecuteDialogueQueue();
					contextState.DialogueStateDJ = Enumerations.DialogueStates.DJ.Battle;
					context.Commit(contextState);
					break;
				case Enumerations.DialogueStates.DJ.Battle:
					_nodeTextBox.AddDialogue("Alright. I've had enoungh...");
					_nodeTextBox.ExecuteDialogueQueue();
					var nextScene = (PackedScene)ResourceLoader.Load(_COMBAT_SCENE_DJ_BATTLE);
					GetTree().ChangeSceneToPacked(nextScene);
					break;
				default:
					break;
			}
		}
	}
}
