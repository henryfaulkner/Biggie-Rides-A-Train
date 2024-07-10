using Godot;
using System;
using System.Reflection.Metadata;

public partial class Scene_TherapistOffice1 : Node3D
{
	private static readonly StringName _COMBAT_SCENE_MUSHROOM = new StringName("res://Pages/CombatScenes/MushroomBattle_2/CombatSceneMushroomBattle_2.tscn");
	private static readonly StringName _COMBAT_SCENE_SUBCONSCIOUS_1 = new StringName("");

	private Therapist3D _nodeTherapist = null;
	private SceneBarrier _nodeSceneBarrier = null;
	private AutoWalk_InteractableArea3D_1 _nodeAutoWalkCollision = null;
	private Subconscious _nodeSubconscious = null;
	private Biggie3D _nodeBiggie = null;
	private Node3D _nodeMushroomFight_2 = null;

	private FramedLevelCamera _nodeMainCamera = null;
	private Camera3D _nodeCameraTalk_1 = null;
	private Camera3D _nodeCameraTalk_2 = null;

	private SaveStateService _serviceSaveState = null;
	private TextBoxService _serviceTextBox = null;

	private bool ProcessingAutoWalk_1 { get; set; }
	private bool ProcessingAutoWalk_2 { get; set; }
	private TherapistDialogueStates TherapistDialogueState { get; set; }
	private SubconsciousDialogueStates SubconsciousDialogueState { get; set; }

	public override void _Ready()
	{
		_nodeTherapist = GetNode<Therapist3D>("./LevelWrapper/TextBoxWrapper/Therapist3D");
		_nodeSceneBarrier = GetNode<SceneBarrier>("./LevelWrapper/TextBoxWrapper/SceneBarrier");
		_nodeAutoWalkCollision = GetNode<AutoWalk_InteractableArea3D_1>("./LevelWrapper/TextBoxWrapper/AutoWalk_InteractableArea3D_1");
		_nodeSubconscious = GetNode<Subconscious>("./LevelWrapper/TextBoxWrapper/Subconscious");
		_nodeBiggie = GetNode<Biggie3D>("./LevelWrapper/TextBoxWrapper/Biggie3D");
		_nodeMushroomFight_2 = GetNode<Node3D>("./LevelWrapper/TextBoxWrapper/MushroomFight_2");

		_nodeMainCamera = GetNode<FramedLevelCamera>("./LevelWrapper/Camera3D");
		_nodeCameraTalk_1 = GetNode<Camera3D>("./LevelWrapper/Camera3D2");
		_nodeCameraTalk_2 = GetNode<Camera3D>("./LevelWrapper/Camera3D4");

		_serviceTextBox = GetNode<TextBoxService>("/root/TextBoxService");

		_nodeTherapist.Interact += ProcessTherapistDialogue;
		TherapistDialogueState = TherapistDialogueStates.First;
		SubconsciousDialogueState = SubconsciousDialogueStates.First;
		_serviceTextBox.InteractionTextBox.SelectedOptionId += ReactToInteractSelection_1;
		_nodeAutoWalkCollision.Collision += () => ProcessingAutoWalk_1 = true;
		ProcessingAutoWalk_1 = false;
		ProcessingAutoWalk_2 = false;

		_serviceSaveState = GetNode<SaveStateService>("/root/SaveStateService");
		var context = _serviceSaveState.Load();
		if (context.DialogueStateSubconscious == (int)SubconsciousDialogueStates.PostMushroomCombat)
		{
			HandlePostMushroomCombat(context);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (ProcessingAutoWalk_1)
		{
			ProcessingAutoWalk_1 = !ProcessAutoWalk_1(delta);
			if (!ProcessingAutoWalk_1)
			{
				//GD.Print("Stop AutoWalk_1");
				_nodeCameraTalk_1.MakeCurrent();
				ProcessSubconsciousDialogue();
			}
		}

		if (ProcessingAutoWalk_2)
		{
			ProcessingAutoWalk_2 = !ProcessAutoWalk_2(delta);
			if (!ProcessingAutoWalk_2)
			{
				//GD.Print("Stop AutoWalk_2");
				SubconsciousDialogueState = SubconsciousDialogueStates.MushroomCombat_Talk;
				_nodeCameraTalk_2.MakeCurrent();
				ProcessSubconsciousDialogue();
			}
		}

		if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
		if (!_serviceTextBox.InteractionTextBox.CanCreateDialogue()) return;
		if (SubconsciousDialogueState == SubconsciousDialogueStates.Second)
		{
			ProcessSubconsciousDialogue();
		}
		else if (SubconsciousDialogueState == SubconsciousDialogueStates.Interaction_1)
		{
			ProcessSubconsciousDialogue();
		}
		else if (SubconsciousDialogueState == SubconsciousDialogueStates.AutoWalk_2)
		{
			ProcessAutoWalk_2(delta);
		}
		else if (SubconsciousDialogueState == SubconsciousDialogueStates.MushroomCombat)
		{
			var context = _serviceSaveState.Load();
			context.DialogueStateSubconscious = (int)SubconsciousDialogueStates.PostMushroomCombat;
			context.StoredLocation = new DoorEntrance(Enumerations.Scenes.TherapistOffice_1, _nodeBiggie.Position.X, _nodeBiggie.Position.Y, _nodeBiggie.Position.Z);
			context.AdditionalStoredLocation = new DoorEntrance(Enumerations.Scenes.TherapistOffice_1, _nodeSubconscious.Position.X, _nodeSubconscious.Position.Y, _nodeSubconscious.Position.Z);
			//GD.Print($"Commit {context.StoredLocation}");
			//GD.Print($"Commit {context.AdditionalStoredLocation}");
			_serviceSaveState.Commit(context);

			var nextScene = (PackedScene)ResourceLoader.Load(_COMBAT_SCENE_MUSHROOM);
			GetTree().ChangeSceneToPacked(nextScene);
		}
		else if (SubconsciousDialogueState == SubconsciousDialogueStates.PostMushroomCombat)
		{
			ProcessSubconsciousDialogue();
		}
		else if (SubconsciousDialogueState == SubconsciousDialogueStates.SubconsciousCombat)
		{
			var nextScene = (PackedScene)ResourceLoader.Load(_COMBAT_SCENE_SUBCONSCIOUS_1);
			GetTree().ChangeSceneToPacked(nextScene);
		}
		else if (SubconsciousDialogueState == SubconsciousDialogueStates.PostSubconsiousCombat)
		{
			//WAKE UP
			var nextScene = (PackedScene)ResourceLoader.Load("PLACEHOLDER");
			GetTree().ChangeSceneToPacked(nextScene);
		}
	}

	public void ProcessTherapistDialogue()
	{
		//GD.Print("Scene_Dream_Room1 ProcessTherapistDialogue");
		switch (TherapistDialogueState)
		{
			case TherapistDialogueStates.First:
				//GD.Print("case TherapistDialogueStates.First");
				if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
				_serviceTextBox.TextBox.AddDialogue("Welcome. I am glad to see you’re here.");
				_serviceTextBox.TextBox.AddDialogue("Your attention is needed here and outside, and neither can be ignored.");
				_serviceTextBox.TextBox.AddDialogue("Biggie, beyond this barrier here, your opponent awaits your arrival.");
				_serviceTextBox.TextBox.AddDialogue("Do not doubt yourself. You will visit me again soon.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				TherapistDialogueState = TherapistDialogueStates.Default;
				_nodeSceneBarrier.CanOpen = true;
				break;
			default:
				//GD.Print("case TherapistDialogueStates.Default or default");
				if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
				_serviceTextBox.TextBox.AddDialogue("Good luck.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				break;
		}
	}

	public bool ProcessAutoWalk_1(double delta)
	{
		//GD.Print("ProcessAutoWalk_1");
		return _nodeBiggie.ForceWalk(_nodeSubconscious.Position + new Vector3(-3.0f, 0.0f, 0.3f), delta);
	}

	public bool ProcessAutoWalk_2(double delta)
	{
		//GD.Print("ProcessAutoWalk_2");
		bool result = _nodeBiggie.ForceWalk(_nodeMushroomFight_2.Position + new Vector3(-2.0f, 0.0f, 1.3f), delta);
		result = _nodeSubconscious.ForceWalk(_nodeMushroomFight_2.Position + new Vector3(2.0f, 0.0f, 0.8f), delta)
			&& result;
		return result;
	}

	private void ProcessSubconsciousDialogue()
	{
		if (!_serviceTextBox.TextBox.CanCreateDialogue()) return;
		if (!_serviceTextBox.InteractionTextBox.CanCreateDialogue()) return;

		switch (SubconsciousDialogueState)
		{
			case SubconsciousDialogueStates.First:
				//GD.Print("SubconsciousDialogueStates.First");
				_serviceTextBox.TextBox.AddDialogue("Hi Biggie. It’s good to see you. I’ve been meaning to talk to you.");
				_serviceTextBox.TextBox.AddDialogue("I think you are making a mistake, going to the train station like you are.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				SubconsciousDialogueState = SubconsciousDialogueStates.Second;
				break;
			case SubconsciousDialogueStates.Second:
				//GD.Print("SubconsciousDialogueStates.Second");
				_serviceTextBox.TextBox.AddDialogue("That train is bad news. You’ll probably get hurt getting on the train.");
				_serviceTextBox.TextBox.AddDialogue("You’re not strong enough to follow in the [color=red]conductor’s[/color] footsteps. Don’t find him.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				SubconsciousDialogueState = SubconsciousDialogueStates.Interaction_1;
				break;
			case SubconsciousDialogueStates.Interaction_1:
				//GD.Print("SubconsciousDialogueStates.Interaction_1");
				_serviceTextBox.InteractionTextBox.StartInteraction("Will you promise me that you will come back home and forget about the train station?", "No", (int)CombatSelectionOptions.Option_1);
				_serviceTextBox.InteractionTextBox.AddOption("No", (int)CombatSelectionOptions.Option_2);
				_serviceTextBox.InteractionTextBox.Execute();
				break;
			case SubconsciousDialogueStates.MushroomCombat_Talk:
				_serviceTextBox.InteractionTextBox.SelectedOptionId += ReactToInteractSelection_2;
				_serviceTextBox.TextBox.AddDialogue("That’s unfortunate. I can see you are not understanding my concern.");
				_serviceTextBox.TextBox.AddDialogue("Please go ahead and fight these two mushrooms. [wave amp=50 freq=6]This should show him he isn’t strong enough[/wave]");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				SubconsciousDialogueState = SubconsciousDialogueStates.MushroomCombat;
				break;
			case SubconsciousDialogueStates.PostMushroomCombat:
				//  todo: check context for these values
				bool mushroomDead = true;
				bool mushroomChat = true;

				if (mushroomDead) _serviceTextBox.TextBox.AddDialogue("Hmmm… those must have been the weakest fungi of all time.");
				else if (mushroomChat) _serviceTextBox.TextBox.AddDialogue("Why have they stopped expelling spores? The mind mushroom really is a pathetic species.");
				_serviceTextBox.TextBox.AddDialogue("The point is… You can’t go.");
				_serviceTextBox.TextBox.ExecuteDialogueQueue();
				SubconsciousDialogueState = SubconsciousDialogueStates.SubconsciousCombat;
				break;
			default:
				//GD.Print("holyyyyyy");
				break;
		}
	}

	private void ReactToInteractSelection_1(int selectOption)
	{
		switch (selectOption)
		{
			case (int)CombatSelectionOptions.Option_1:
			case (int)CombatSelectionOptions.Option_2:
				SubconsciousDialogueState = SubconsciousDialogueStates.AutoWalk_2;
				ProcessingAutoWalk_2 = true;
				return;
			default:
				break;
		}
	}

	private void ReactToInteractSelection_2(int selectOption)
	{
		switch (selectOption)
		{
			case (int)CombatSelectionOptions.Option_1:
			case (int)CombatSelectionOptions.Option_2:
				//
				return;
			default:
				break;
		}
	}

	private void HandlePostMushroomCombat(SaveStateModel context)
	{
		//GD.Print("HandlePostMushroomCombat");
		_nodeCameraTalk_2.MakeCurrent();
		_nodeSubconscious.Position = new Vector3(
			context.AdditionalStoredLocation.X,
			context.AdditionalStoredLocation.Y,
			context.AdditionalStoredLocation.Z
		);
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
		Interaction_1,
		AutoWalk_2,
		MushroomCombat_Talk,
		MushroomCombat,
		PostMushroomCombat,
		SubconsciousCombat,
		PostSubconsiousCombat,
	}

	private enum CombatSelectionOptions
	{
		Option_1,
		Option_2,
	}
}
