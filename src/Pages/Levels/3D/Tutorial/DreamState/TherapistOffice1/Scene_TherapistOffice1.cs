using Godot;
using System;

public partial class Scene_TherapistOffice1 : Node3D
{
	private Therapist3D _nodeTherapist = null;
	private TextBox _nodeTextBox = null;
	private SceneBarrier _nodeSceneBarrier = null;	
	private AutoWalk_InteractableArea3D_1 _nodeAutoWalkCollision = null;
	private Subconscious _nodeSubconscious = null; 
	private Biggie3D _nodeBiggie = null;

	private SaveStateService _serviceSaveState = null;

	private bool ProcessingAutoWalk { get; set; }
	private TherapistDialogueStates TherapistDialogueState { get; set; }


	public override void _Ready()
	{
		_nodeTherapist = GetNode<Therapist3D>("./LevelWrapper/TextBoxWrapper/Therapist3D");
		_nodeTextBox = GetNode<TextBox>("./LevelWrapper/TextBoxWrapper/TextBox");
		_nodeSceneBarrier = GetNode<SceneBarrier>("./LevelWrapper/TextBoxWrapper/SceneBarrier");
		_nodeAutoWalkCollision = GetNode<AutoWalk_InteractableArea3D_1>("./LevelWrapper/TextBoxWrapper/AutoWalk_InteractableArea3D_1");
		_nodeSubconscious = GetNode<Subconscious>("./LevelWrapper/TextBoxWrapper/Subconscious");
		_nodeBiggie = GetNode<Biggie3D>("./LevelWrapper/TextBoxWrapper/Biggie3D");

		_nodeTherapist.Interact += ProcessTherapistDialogue;
		TherapistDialogueState = TherapistDialogueStates.First;
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
			}
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

	private enum TherapistDialogueStates
	{
		First,
		Default,
	}
}
