using System;

public class SaveStateModel
{
	public DoorEntrance StoredLocation { get; set; }
	
	public bool HasItemTicketPieceOne { get; set; }
	public bool HasItemTicketPieceTwo { get; set; }
	public bool HasItemTicketPieceThree { get; set; }
	public bool HasItemTicketPieceFour { get; set; }
	public bool HasItemTape { get; set; }
	public bool HasItemMainStationRecord { get; set; }
	public bool HasItemSmoothJazzRecord { get; set; }
	public bool HasItemClubMixRecord { get; set; }
	
	public Enumerations.GameMusic.Records ActiveRecord { get; set; }
	
	public Enumerations.DialogueStates.Teller DialogueStateTeller { get; set; }
	public Enumerations.DialogueStates.Chess DialogueStateChess { get; set; }
	public Enumerations.DialogueStates.Therapist DialogueStateTherapist { get; set; }
	public Enumerations.DialogueStates.DJ DialogueStateDJ { get; set; }
}
