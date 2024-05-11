using Godot;
using System;

public partial class Scene_TherapistOffice1 : Node3D
{
	private static readonly StringName _COMBAT_SCENE_MUSHROOM = new StringName("res://Pages/CombatScenes/MushroomBattle_2/CombatSceneMushroomBattle_2.tscn");

	private Therapist3D _nodeTherapist = null;
	private TextBox _nodeTextBox = null;
	private InteractionTextBox _nodeInteractionTextBox = null;
	private SceneBarrier _nodeSceneBarrier = null;
	private AutoWalk_InteractableArea3D_1 _nodeAutoWalkCollision = null;
	private Subconscious _nodeSubconscious = null;
	private Biggie3D _nodeBiggie = null;

	private FramedLevelCamera _nodeMainCamera = null;
	private Camera3D _nodeCameraTalk_1 = null;

	private SaveStateService _serviceSaveState = null;

	private bool ProcessingAutoWalk { get; set; }
	private TherapistDialogueStates TherapistDialogueState { get; set; }
	private SubconsciousDialogueStates SubconsciousDialogueState { get; set; }

	public override void _Ready()
	{
		_nodeTherapist = GetNode<Therapist3D>("./LevelWrapper/TextBoxWrapper/Therapist3D");
		_nodeTextBox = GetNode<TextBox>("./LevelWrapper/TextBoxWrapper/TextBox");
		_nodeInteractionTextBox = GetNode<InteractionTextBox>("./LevelWrapper/TextBoxWrapper/InteractionTextBox");
		_nodeSceneBarrier = GetNode<SceneBarrier>("./LevelWrapper/TextBoxWrapper/SceneBarrier");
		_nodeAutoWalkCollision = GetNode<AutoWalk_InteractableArea3D_1>("./LevelWrapper/TextBoxWrapper/AutoWalk_InteractableArea3D_1");
		_nodeSubconscious = GetNode<Subconscious>("./LevelWrapper/TextBoxWrapper/Subconscious");
		_nodeBiggie = GetNode<Biggie3D>("./LevelWrapper/TextBoxWrapper/Biggie3D");

		_nodeMainCamera = GetNode<FramedLevelCamera>("./LevelWrapper/Camera3D");
		_nodeCameraTalk_1 = GetNode<Camera3D>("./LevelWrapper/Camera3D2");

		_nodeTherapist.Interact += ProcessTherapistDialogue;
		TherapistDialogueState = TherapistDialogueStates.First;
		SubconsciousDialogueState = SubconsciousDialogueStates.First;
		_nodeInteractionTextBox.SelectedOptionId += ReactToCombatSelection;
		_nodeAutoWalkCollision.Collision += () => ProcessingAutoWalk = true;
		ProcessingAutoWalk = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (ProcessingAutoWalk)
		{
			ProcessingAutoWalk = !ProcessAutoWalk(delta);
			if (!ProcessingAutoWalk)
			{
				GD.Print("Stop AutoWalk");
				_nodeCameraTalk_1.MakeCurrent();
				ProcessSubconsciousDialogue();
			}
		}

		if (!_nodeTextBox.CanCreateDialogue()) return;
		if (!_nodeInteractionTextBox.CanCreateDialogue()) return;
		if (SubconsciousDialogueState == SubconsciousDialogueStates.Second)
		{
			ProcessSubconsciousDialogue();
		}
		else if (SubconsciousDialogueState == SubconsciousDialogueStates.MushroomCombat)
		{
			ProcessSubconsciousDialogue();
		}
	}

	public void ProcessTherapistDialogue()
	{
		GD.Print("Scene_Dream_Room1 ProcessTherapistDialogue");
		switch (TherapistDialogueState)
		{
			case TherapistDialogueStates.First:
				GD.Print("case TherapistDialogueStates.First");
				if (!_nodeTextBox.CanCreateDialogue()) return;
				_nodeTextBox.AddDialogue("Welcome. I am glad to see you’re here.");
				_nodeTextBox.AddDialogue("Your attention is needed here and outside, and neither can be ignored.");
				_nodeTextBox.AddDialogue("Biggie, beyond this barrier here, your opponent awaits your arrival.");
				_nodeTextBox.AddDialogue("Do not doubt yourself. You will visit me again soon.");
				_nodeTextBox.ExecuteDialogueQueue();
				TherapistDialogueState = TherapistDialogueStates.Default;
				_nodeSceneBarrier.CanOpen = true;
				break;
			default:
				GD.Print("case TherapistDialogueStates.Default or default");
				if (!_nodeTextBox.CanCreateDialogue()) return;
				_nodeTextBox.AddDialogue("Good luck.");
				_nodeTextBox.ExecuteDialogueQueue();
				break;
		}
	}

	public bool ProcessAutoWalk(double delta)
	{
		return _nodeBiggie.ForceWalk(_nodeSubconscious.Position + new Vector3(-3.0f, 0.0f, 0.3f), delta);
	}

	private void ProcessSubconsciousDialogue()
	{
		if (!_nodeTextBox.CanCreateDialogue()) return;
		if (!_nodeInteractionTextBox.CanCreateDialogue()) return;
		
		switch (SubconsciousDialogueState)
		{
			case SubconsciousDialogueStates.First:
				GD.Print("SubconsciousDialogueStates.First");
				_nodeTextBox.AddDialogue("Hi Biggie. It’s good to see you. I’ve been meaning to talk to you.");
				_nodeTextBox.AddDialogue("I think you are making a mistake, going to the train station like you are.");
				_nodeTextBox.ExecuteDialogueQueue();
				SubconsciousDialogueState = SubconsciousDialogueStates.Second;
				break;
			case SubconsciousDialogueStates.Second:
				GD.Print("SubconsciousDialogueStates.Second");
				_nodeTextBox.AddDialogue("That train is bad news. You’ll probably get hurt even getting on the train.");
				_nodeTextBox.AddDialogue("You’re not strong enough to follow in the conductor’s footsteps. Don’t find him.");
				_nodeTextBox.ExecuteDialogueQueue();
				SubconsciousDialogueState = SubconsciousDialogueStates.MushroomCombat;
				break;
			case SubconsciousDialogueStates.MushroomCombat:
				GD.Print("SubconsciousDialogueStates.MushroomCombat");
				_nodeInteractionTextBox.StartInteraction("Will you promise me that you will come back home and forget about the train station?", "No", (int)CombatSelectionOptions.CombatNo_1);
				_nodeInteractionTextBox.AddOption("No", (int)CombatSelectionOptions.CombatNo_2);
				_nodeInteractionTextBox.Execute();
				break;
			default:
				GD.Print("holyyyyyy");
				break;
		}
	}

	private void ReactToCombatSelection(int selectOption)
	{
		switch (selectOption)
		{
			case (int)CombatSelectionOptions.CombatNo_1:
			case (int)CombatSelectionOptions.CombatNo_2:
				var nextScene = (PackedScene)ResourceLoader.Load(_COMBAT_SCENE_MUSHROOM);
				GetTree().ChangeSceneToPacked(nextScene);
				return;
			default:
				break;
		}
	}

	private enum TherapistDialogueStates
	{
		First,
		Default,
	}

	private enum SubconsciousDialogueStates
	{
		First,
		Second,
		MushroomCombat,
	}

	private enum CombatSelectionOptions
	{
		CombatNo_1,
		CombatNo_2,
	}
}
