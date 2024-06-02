using Godot;
using System;

public partial class Scene_MushroomFight : Node3D
{
	private static readonly StringName _COMBAT_SCENE_MUSHROOM = new StringName("res://Pages/CombatScenes/MushroomBattle_1/CombatSceneMushroomBattle_1.tscn");

	private Node3D _nodeSelf = null;
	private Mushroom3D _nodeMushroom = null;
	private TextBox _nodeTextBox = null;
	private InteractionTextBox _nodeInteractionTextBox = null;

	private SaveStateService _serviceSaveState = null;

	private MushroomDialogueStates MushroomDialogueState { get; set; }

	public override void _Ready()
	{
		_nodeSelf = GetNode<Node3D>(".");
		_nodeMushroom = GetNode<Mushroom3D>("./LevelWrapper/TextBoxWrapper/Mushroom3D");
		_nodeTextBox = GetNode<TextBox>("./LevelWrapper/TextBoxWrapper/TextBox");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("./LevelWrapper/TextBoxWrapper/InteractionTextBox");

		_nodeMushroom.Interact += ProcessMushroomDialogue;
		MushroomDialogueState = MushroomDialogueStates.Discovery;
		_nodeInteractionTextBox.SelectedOptionId += ReactToMushroomSelection;

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		var context = _serviceSaveState.Load();
		if (context.IsMushroomDead)
		{
			_nodeMushroom.QueueFree();
		}
		else if (context.IsMushroomMoved)
		{
			_nodeMushroom.Position += new Vector3(3, 0, 0);
		}
	}

	public override void _PhysicsProcess(double _delta)
	{
	}

	public void ProcessMushroomDialogue()
	{
		//GD.Print("Scene_Dream_Room1 ProcessMushroomDialogue");
		switch (MushroomDialogueState)
		{
			case MushroomDialogueStates.Discovery:
				//GD.Print("case MushroomDialogueStates.Discovery");
				if (!_nodeTextBox.CanCreateDialogue()) return;
				_nodeTextBox.AddDialogue("This mushroom seems content with resting in front of the door.");
				_nodeTextBox.ExecuteDialogueQueue();
				MushroomDialogueState = MushroomDialogueStates.Combat;
				break;
			case MushroomDialogueStates.Combat:
				//GD.Print("case MushroomDialogueStates.Combat");
				if (!_nodeInteractionTextBox.CanCreateDialogue()) return;
				_nodeInteractionTextBox.StartInteraction("Would you like to remove the mushroom in front of the door?", "Yes", (int)MushroomSelectionOptions.CombatYes);
				_nodeInteractionTextBox.AddOption("No", (int)MushroomSelectionOptions.CombatNo);
				_nodeInteractionTextBox.Execute();
				break;
			default:
				break;
		}
	}

	public void ReactToMushroomSelection(int selectOption)
	{
		switch (selectOption)
		{
			case (int)MushroomSelectionOptions.CombatYes:
				var nextScene = (PackedScene)ResourceLoader.Load(_COMBAT_SCENE_MUSHROOM);
				GetTree().ChangeSceneToPacked(nextScene);
				return;
			case (int)MushroomSelectionOptions.CombatNo:
				_nodeTextBox.AddDialogue("The mushroom remains in front of the door.");
				_nodeTextBox.ExecuteDialogueQueue();
				MushroomDialogueState = MushroomDialogueStates.Discovery;
				break;
			default:
				break;
		}
	}

	private void RedirectToMushroomCombat()
	{

	}

	private enum MushroomDialogueStates
	{
		Discovery = 0,
		Combat = 1,
	}

	private enum MushroomSelectionOptions
	{
		CombatYes = 0,
		CombatNo = 1,
	}
}
