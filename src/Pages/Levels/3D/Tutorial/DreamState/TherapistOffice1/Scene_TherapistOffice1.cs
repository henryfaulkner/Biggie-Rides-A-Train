using Godot;
using System;

public partial class Scene_TherapistOffice1 : Node3D
{
	private Therapist3D _nodeTherapist = null;
	private TextBox _nodeTextBox = null;
	private SceneBarrier _nodeSceneBarrier = null;

	private SaveStateService _serviceSaveState = null;

	private TherapistDialogueStates TherapistDialogueState { get; set; }


	public override void _Ready()
	{
		_nodeTherapist = GetNode<Therapist3D>("./LevelWrapper/TextBoxWrapper/Therapist3D");
		_nodeTextBox = GetNode<TextBox>("./LevelWrapper/TextBoxWrapper/TextBox");
		_nodeSceneBarrier = GetNode<SceneBarrier>("./LevelWrapper/TextBoxWrapper/SceneBarrier");

		_nodeTherapist.Interact += ProcessTherapistDialogue;
		TherapistDialogueState = TherapistDialogueStates.First;
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

	private enum TherapistDialogueStates
	{
		First,
		Default,
	}
}
