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
				TransitionToBiggieChatSpecialAttack = 1,
				TransitionToBiggieChatSpecialChat = 2,
				TransitionToBiggieFightAttack = 3,
				TransitionToBiggieFightChat = 4,
				BiggieChatSpecialAttack = 5,
				BiggieChatSpecialChat = 6,
				BiggieFightAttack = 7,
				BiggieFightChat = 8,
				TransitionToEnemyAttackFromBiggieChatSpecialAttack = 9,
				TransitionToEnemyAttackFromBiggieChatSpecialChat = 10,
				TransitionToEnemyAttackFromBiggieFightAttack = 11,
				TransitionToEnemyAttackFromBiggieFightChat = 12,
				EnemyAttack = 13,
				TransitionToBiggieCombatMenu = 14,
				BiggieDefeat = 15,
				EnemyDefeatPhysical = 16,
				EnemyDefeatEmotional = 17,
				TransitionToChatterTextBox_FromCombatMenu_SelectChatSpecialAttack = 18,
				ChatterTextBox_FromCombatMenu_SelectChatSpecialAttack = 19,
				TransitionToChatterTextBox_FromCombatMenu_SelectChatSpecialChat = 20,
				ChatterTextBox_FromCombatMenu_SelectChatSpecialChat = 21,
				TransitionToChatterTextBox_FromCombatMenu_SelectFightAttack = 22,
				ChatterTextBox_FromCombatMenu_SelectFightAttack = 23,
				TransitionToChatterTextBox_FromCombatMenu_SelectFightChat = 24,
				ChatterTextBox_FromCombatMenu_SelectFightChat = 25,
				TransitionToChatterTextBox_FromBiggieChatSpecialAttack = 26,
				ChatterTextBox_FromBiggieChatSpecialAttack = 27,
				TransitionToChatterTextBox_FromBiggieChatSpecialChat = 28,
				ChatterTextBox_FromBiggieChatSpecialChat = 29,
				TransitionToChatterTextBox_FromBiggieFightAttack = 30,
				ChatterTextBox_FromBiggieFightAttack = 31,
				TransitionToChatterTextBox_FromBiggieFightChat = 32,
				ChatterTextBox_FromBiggieFightChat = 33,
				TransitionToChatterTextBox_FromEnemyAttack = 34,
				ChatterTextBox_FromEnemyAttack = 35,
				TargetEnemy_FromBiggieChatSpecialAttack = 36,
				TargetEnemy_FromBiggieChatSpecialChat = 37,
				TargetEnemy_FromBiggieFightAttack = 38,
				TargetEnemy_FromBiggieFightChat = 39,
			}

			public enum Events
			{
				ShowChatterTextBox = 0,
				SelectChatSpecialAttack = 1,
				SelectChatSpecialChat = 2,
				SelectFightAttack = 3,
				SelectFightChat = 4,
				FinishTransition = 5,
				FinishBiggieAttack = 6,
				FinishEnemyAttack = 7,
				BiggieDefeat = 8,
				EnemyDefeatPhysical = 9,
				EnemyDefeatEmotional = 10,
				FinishChatterTextBox = 11,
				ShowChatterTextBox_SelectChatSpecialAttack = 12,
				ShowChatterTextBox_SelectChatSpecialChat = 13,
				ShowChatterTextBox_SelectFightAttack = 14,
				ShowChatterTextBox_SelectFightChat = 15,

				// DEPRECATE THIS
				TargetEnemy_SpecialAttack = 16,
				TargetEnemy_SpecialChat = 17,
				TargetEnemy_Attack = 18,
				TargetEnemy_Chat = 19,
				FinishTargetEnemy = 20,
			}
		}

		public enum CombatOptions
		{
			SpecialAttack = 0,
			SpecialChat = 1,
			Chat = 2,
			Attack = 3,
			Info = 4,
		}

		public enum BasePagePanelOptions
		{
			Fight = 0,
			Chat = 1,
			Info = 2,
			Exit = 3,
		}

		public enum FightPagePanelOptions
		{
			Attack = 0,
			Chat = 1,
			Back = 2,
		}

		public enum ChatPagePanelOptions
		{
			SpecialAttack = 0,
			SpecialChat = 1,
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
		TherapistOffice_1,
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

	public static class Physics
	{
		public enum Rotations
		{
			Default,
			Forward,
		}
	}

	public static class Cameras
	{
		public enum Direction
		{
			Up = 0,
			Right = 1,
			Down = 2,
			Left = 3,
			UpRight = 4,
			DownRight = 5,
			DownLeft = 6,
			UpLeft = 7,
		}
	}
}
