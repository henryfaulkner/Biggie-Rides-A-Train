public partial class Enumerations
{
	public static class Combat
	{
		public static class StateMachine
		{
			public enum States
			{
				BiggieCombatMenu = 0,
				TransitionToBiggieChatAsk = 1,
				TransitionToBiggieChatCharm = 2,
				TransitionToBiggieFightScratch = 3,
				TransitionToBiggieFightBite = 4,
				BiggieChatAsk = 5,
				BiggieChatCharm = 6,
				BiggieFightScratch = 7,
				BiggieFightBite = 8,
				TransitionToEnemyAttackFromBiggieChatAsk = 9,
				TransitionToEnemyAttackFromBiggieChatCharm = 10,
				TransitionToEnemyAttackFromBiggieFightScratch = 11,
				TransitionToEnemyAttackFromBiggieFightBite = 12,
				EnemyAttack = 13,
				TransitionToBiggieCombatMenu = 14,
				BiggieDefeat = 15,
				EnemyDefeatPhysical = 16,
				EnemyDefeatEmotional = 17,
				TransitionToChatterTextBox = 18,
				ChatterTextBox = 19,
			}

			public enum Events
			{
				ShowChatterTextBox = 0,
				SelectChatAsk = 1,
				SelectChatCharm = 2,
				SelectFightScratch = 3,
				SelectFightBite = 4,
				FinishTransition = 5,
				FinishBiggieAttack = 6,
				FinishEnemyAttack = 7,
				BiggieDefeat = 8,
				EnemyDefeatPhysical = 9,
				EnemyDefeatEmotional = 10,
				FinishChatterTextBox = 11,
			}
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

	public static class Movement
	{
		public enum Directions
		{
			Up = 0,
			Right = 1,
			Down = 2,
			Left = 3,
			Idle = 4,
		}
	}
}
