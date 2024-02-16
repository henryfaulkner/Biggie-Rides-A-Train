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
	
	public GameMusic.Records ActiveRecord { get; set; }
	
	public DialogueStates.Teller DialogueStateTeller { get; set; }
	public DialogueStates.Chess DialogueStateChess { get; set; }
	public DialogueStates.Therapist DialogueStateTherapist { get; set; }
}
