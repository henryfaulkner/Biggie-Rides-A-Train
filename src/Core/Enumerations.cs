using Godot;

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
				TransitionToChatterTextBox_FromCombatMenu_SelectChatAsk = 18,
				ChatterTextBox_FromCombatMenu_SelectChatAsk = 19,
				TransitionToChatterTextBox_FromCombatMenu_SelectChatCharm = 20,
				ChatterTextBox_FromCombatMenu_SelectChatCharm = 21,
				TransitionToChatterTextBox_FromCombatMenu_SelectFightScratch = 22,
				ChatterTextBox_FromCombatMenu_SelectFightScratch = 23,
				TransitionToChatterTextBox_FromCombatMenu_SelectFightBite = 24,
				ChatterTextBox_FromCombatMenu_SelectFightBite = 25,
				TransitionToChatterTextBox_FromBiggieChatAsk = 26,
				ChatterTextBox_FromBiggieChatAsk = 27,
				TransitionToChatterTextBox_FromBiggieChatCharm = 28,
				ChatterTextBox_FromBiggieChatCharm = 29,
				TransitionToChatterTextBox_FromBiggieFightScratch = 30,
				ChatterTextBox_FromBiggieFightScratch = 31,
				TransitionToChatterTextBox_FromBiggieFightBite = 32,
				ChatterTextBox_FromBiggieFightBite = 33,
				TransitionToChatterTextBox_FromEnemyAttack = 34,
				ChatterTextBox_FromEnemyAttack = 35,
				TransitionToChatterTextBox_FromDefeatChatter_Ask = 36,
				ChatterTextBox_FromDefeatChatter_Ask = 37,

				TransitionToChatterTextBox_FromDefeatChatter_Charm = 38,
				ChatterTextBox_FromDefeatChatter_Charm = 39,

				TransitionToChatterTextBox_FromDefeatChatter_Scratch = 40,
				ChatterTextBox_FromDefeatChatter_Scratch = 41,

				TransitionToChatterTextBox_FromDefeatChatter_Bite = 42,
				ChatterTextBox_FromDefeatChatter_Bite = 43,
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
				ShowChatterTextBox_SelectChatAsk = 12,
				ShowChatterTextBox_SelectChatCharm = 13,
				ShowChatterTextBox_SelectFightScratch = 14,
				ShowChatterTextBox_SelectFightBite = 15,
				DefeatChatter_Ask = 16,
				DefeatChatter_Charm = 17,
				DefeatChatter_Scratch = 18,
				DefeatChatter_Bite = 19,
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
		Club = 4,
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
