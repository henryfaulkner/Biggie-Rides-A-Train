public static partial class Enumerations
{
	public struct DialogueStates 
	{
		public enum Teller
		{
			Introduce,
			AskForTicket,
		}
		
		public enum Chess
		{
			Introduce,
			PlayOpponent,
		}
		
		public enum Therapist
		{
			Introduce,
			OfferTherapy,
		}
		
		public enum DJ
		{
			Introduce,
			Battle,
		}
	}
	
	public static class GameMusic 
	{
		public enum Records
		{
			Empty,
			MainStation,
			SmoothJazz,
			ClubMix,
		}
	} 
}
