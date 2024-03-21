public partial class Enumerations
{
	public enum CombatStates
	{
		Text = 0,
		TransitionToEnemyAttack = 1,
		EnemyAttack = 2,
		TransitionToText = 3,
		BiggieFight = 4,
		TransitionToBiggieFight = 5,
		BiggieChat = 6,
		TransitionToBiggieChat = 7,
	}

	public enum CombatOptions
	{
		Ask = 0,
		Charm = 1,
		Bite = 2,
		Scratch = 3,
	}

	public enum BasePagePanelOptions
	{
		Fight = 0,
		Chat = 1,
		Exit = 2,
	}

	public enum FightPagePanelOptions
	{
		Scratch = 0,
		Bite = 1,
		Back = 2,
	}

	public enum ChatPagePanelOptions
	{
		Ask = 0,
		Charm = 1,
		Back = 2,
	}

	public enum CombatOptionTypes
	{
		Emotional = 0,
		Physical = 1,
	}

	public enum LogLevels
	{
		Debug = 0,
		Info = 1,
		Error = 2,
	}

	public enum Scenes
	{
		Empty = 0,
		OutsideStation = 1,
		MainStation = 2,
		TherapistOffice = 3,
		Club = 4
	}

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
			TestBattle = 2,
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
