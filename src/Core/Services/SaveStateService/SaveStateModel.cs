using System;
using Newtonsoft.Json;

public class SaveStateModel
{
	public SaveStateModel()
	{
		StoredLocation = null;
		AdditionalStoredLocation = null;
		UserSettings = new UserSettingsModel();
	}

	[JsonProperty("StoredLocation")]
	public DoorEntrance StoredLocation { get; set; }

	[JsonProperty("AdditionalStoredLocation")]
	public DoorEntrance AdditionalStoredLocation { get; set; }

	[JsonProperty("HasItemTicketPieceOne")]
	public bool HasItemTicketPieceOne { get; set; }

	[JsonProperty("HasItemTicketPieceTwo")]
	public bool HasItemTicketPieceTwo { get; set; }

	[JsonProperty("HasItemTicketPieceThree")]
	public bool HasItemTicketPieceThree { get; set; }

	[JsonProperty("HasItemTicketPieceFour")]
	public bool HasItemTicketPieceFour { get; set; }

	[JsonProperty("HasItemTape")]
	public bool HasItemTape { get; set; }

	[JsonProperty("HasItemMainStationRecord")]
	public bool HasItemMainStationRecord { get; set; }

	[JsonProperty("HasItemSmoothJazzRecord")]
	public bool HasItemSmoothJazzRecord { get; set; }

	[JsonProperty("HasItemClubMixRecord")]
	public bool HasItemClubMixRecord { get; set; }

	[JsonProperty("ActiveRecord")]
	public Enumerations.GameMusic.Records ActiveRecord { get; set; }

	[JsonProperty("DialogueStateTeller")]
	public Enumerations.DialogueStates.Teller DialogueStateTeller { get; set; }

	[JsonProperty("DialogueStateChess")]
	public Enumerations.DialogueStates.Chess DialogueStateChess { get; set; }

	[JsonProperty("DialogueStateTherapist")]
	public Enumerations.DialogueStates.Therapist DialogueStateTherapist { get; set; }

	[JsonProperty("DialogueStateDJ")]
	public Enumerations.DialogueStates.DJ DialogueStateDJ { get; set; }

	[JsonProperty("DialogueStateSubconscious")]
	public int DialogueStateSubconscious { get; set; }

	// Intro

	// Dream State
	[JsonProperty("IsSwitchDoorOpen")]
	public bool IsSwitchDoorOpen { get; set; }
	[JsonProperty("IsButtonDoorOpen")]
	public bool IsButtonDoorOpen { get; set; }
	[JsonProperty("IsMushroomDead")]
	public bool IsMushroomDead { get; set; }
	[JsonProperty("IsMushroomMoved")]
	public bool IsMushroomMoved { get; set; }

	[JsonProperty("IsDoubleMushroomDefeated")]
	public bool IsDoubleMushroomDefeated { get; set; }

	public UserSettingsModel UserSettings { get; set; }

	public NpcStates NpcStates { get; set; }
}

public class UserSettingsModel
{
	[JsonProperty("FxMuted")]
	public bool FxMuted { get; set; }
}

public class NpcStates
{
	[JsonProperty("LionState")]
	public LionStateMachine.LionStates LionState { get; set; }
}