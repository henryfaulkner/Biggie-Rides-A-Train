public static partial class Enumerations
{
	public struct DialogueStates 
	{
		public enum Teller
		{
			Introduce = 0,
			AskForTicket = 1,
		}
		
		public enum Chess
		{
			Introduce = 0,
			PlayOpponent = 1,
		}
		
		public enum Therapist
		{
			Introduce = 0,
			OfferTherapy = 1,
		}
		
		public enum DJ
		{
			Introduce = 0,
			Battle = 1,
		}
	}
	
	public static class GameMusic 
	{
		public enum Records
		{
			Empty = 0,
			MainStation = 1,
			SmoothJazz = 2,
			ClubMix = 3,
		}
	} 
}
