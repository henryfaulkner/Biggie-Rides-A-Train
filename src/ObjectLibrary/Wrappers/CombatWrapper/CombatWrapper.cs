using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CombatWrapper : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Panel _nodeSubjectPanel = null;
	private Panel _nodeBasePagePanel = null;
	private FightPageBasePanel _nodeFightPageBasePanel = null;
	private ChatPageBasePanel _nodeChatPageBasePanel = null;
	private Panel _nodeChatterTextBoxPanel = null;
	private HBoxContainer _nodeHudContainerSubject = null;
	private HBoxContainer _nodeHudContainerTargetEnemyAttack = null;
	private HBoxContainer _nodeHudContainerTargetBiggieAttack = null;
	private HBoxContainer _nodeHudContainerTargetBiggieCombatMenu = null;
	private HBoxContainer _nodeHudContainerTargetChatterTextBox = null;

	private MarginContainer _nodeActionInfo = null;
	private BiggieCombatMenu _nodeBiggieCombatMenu = null;
	private MarginContainer _nodeBiggieCombatMenuTextContainer = null;
	private ChatterTextBox _nodeChatterTextBox = null;
	private MarginContainer _nodeChatterTextBoxTextContainer = null;
	private MarginContainer _nodeEnemyAttackContainer = null;
	private Panel _nodeEnemyAttackPanel = null;
	private MarginContainer _nodeBiggieAttackContainer = null;
	private Panel _nodeBiggieAttackPanel = null;
	private FightMove _nodeBiggieFightMove = null;
	private ChatMove _nodeBiggieChatMove = null;

	private PanelAnimationHelper HudAnimationHelper { get; set; }
	private static readonly float _HUD_SPEED_SLOW = 5f;
	private static readonly float _HUD_SPEED_FAST = 8f;
	private PanelAnimationHelper MainAnimationHelper { get; set; }
	private static readonly float _MAIN_SPEED = 20f;


	private ProgressBar _nodeBiggieHpProgressBar = null;
	private Label _nodeBiggieHpValueLabel = null;

	private ProgressBar _nodeBiggieSpProgressBar = null;
	private Label _nodeBiggieSpValueLabel = null;

	private CombatSingleton _globalCombatSingleton = null;
	private Enumerations.Combat.CombatOptions LastCombatOptionUsed { get; set; }

	public override void _Ready()
	{
		// note the containers wrapping the panels must have the same position and size
		// the containers can use margin to change panel size for animation
		_nodeSubjectPanel = GetNode<Panel>("./TransformContainer/TransformPanel");
		_nodeBasePagePanel = GetNode<Panel>("./BiggieCombatMenu/TextBoxContainer/BasePagePanel");
		_nodeFightPageBasePanel = GetNode<FightPageBasePanel>("./BiggieCombatMenu/TextBoxContainer/FightPagePanel");
		_nodeChatPageBasePanel = GetNode<ChatPageBasePanel>("./BiggieCombatMenu/TextBoxContainer/ChatPagePanel");
		_nodeChatterTextBoxPanel = GetNode<Panel>("./ChatterTextBox/TextBoxContainer/Panel");
		_nodeHudContainerSubject = GetNode<HBoxContainer>("./HudContainer");
		// Need to Queue Free this at some point
		_nodeHudContainerTargetEnemyAttack = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 250);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetBiggieAttack = CreateHudContainerTarget(_nodeHudContainerSubject, 0, -160);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetBiggieCombatMenu = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 0);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetChatterTextBox = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 0);
		HudAnimationHelper = new PanelAnimationHelper(_HUD_SPEED_SLOW);
		MainAnimationHelper = new PanelAnimationHelper(_MAIN_SPEED);

		_nodeActionInfo = GetNode<MarginContainer>("HudContainer/ActionInfo");
		_nodeBiggieCombatMenu = GetNode<BiggieCombatMenu>("./BiggieCombatMenu");
		_nodeBiggieCombatMenuTextContainer = GetNode<MarginContainer>("./BiggieCombatMenu/TextBoxContainer");
		_nodeChatterTextBox = GetNode<ChatterTextBox>("./ChatterTextBox");
		_nodeChatterTextBoxTextContainer = GetNode<MarginContainer>("./ChatterTextBox/TextBoxContainer");
		_nodeEnemyAttackContainer = GetNode<MarginContainer>("./EnemyAttackContainer");
		_nodeEnemyAttackPanel = GetNode<Panel>("./EnemyAttackContainer/EnemyAttackPanel");
		_nodeBiggieAttackContainer = GetNode<MarginContainer>("./BiggieAttackContainer");
		_nodeBiggieAttackPanel = GetNode<Panel>("./BiggieAttackContainer/BiggieAttackPanel");
		_nodeBiggieFightMove = GetNode<FightMove>("./BiggieAttackContainer/FightMove");
		_nodeBiggieChatMove = GetNode<ChatMove>("./BiggieAttackContainer/ChatMove");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		SetEnemyAttackContainerService(_nodeEnemyAttackContainer);

		_nodeFightPageBasePanel.SelectFight += HandleFightSelection;
		_nodeChatPageBasePanel.SelectChat += HandleChatSelection;
		_nodeBiggieCombatMenu.EndBiggieCombatMenuTurn += HandleEndBiggieCombatMenuTurn;
		_nodeBiggieCombatMenu.ShowActionInfo += ShowActionInfo;
		_nodeBiggieCombatMenu.HideActionInfo += HideActionInfo;
		_nodeBiggieFightMove.EndBiggieAttackTurn += HandleEndBiggieAttackTurn;
		_nodeBiggieChatMove.EndBiggieAttackTurn += HandleEndBiggieAttackTurn;

		_nodeBiggieHpProgressBar = GetNode<ProgressBar>("./HudContainer/BarContainer/MarginContainer/HealthContainer/Health/MarginContainer/ProgressBar");
		_nodeBiggieHpValueLabel = GetNode<Label>("./HudContainer/BarContainer/MarginContainer/HealthContainer/Health/HpValueLabel");

		_nodeBiggieSpProgressBar = GetNode<ProgressBar>("./HudContainer/BarContainer/MarginContainer/SpecialContainer/Special/MarginContainer/ProgressBar");
		_nodeBiggieSpValueLabel = GetNode<Label>("./HudContainer/BarContainer/MarginContainer/SpecialContainer/Special/SpValueLabel");
		_globalCombatSingleton.SpecialMeter = new SpecialMeter()
		{
			MaxLevel = 9,
			CurrentLevel = 0,
			VisualLevel = 0,
			ProgressBar = _nodeBiggieSpProgressBar,
			ValueLabel = _nodeBiggieSpValueLabel
		};
		_globalCombatSingleton.SpecialMeter.ValueLabel.Text = $"{_globalCombatSingleton.SpecialMeter.CurrentLevel}/{_globalCombatSingleton.SpecialMeter.MaxLevel}";
		_globalCombatSingleton.SpecialMeter.TweenVisualLevelTowardCurrentLevel();

		HideEnemyAttackContainer();
		HideBiggieAttackContainer();
		HideActionInfo();

		FirstFramePass = true;
	}

	private bool FirstFramePass { get; set; }
	private bool ProcessFirstPass()
	{
		if (FirstFramePass)
		{
			ShowSubjectPanel();
			FirstFramePass = false;
			return true;
		}
		return false;
	}

	public override void _Process(double delta)
	{
		if (CheckForBiggeDefeat()) EmitSignal(SignalName.BiggieDefeat);
		if (CheckForEnemyListPhysicalDefeat()) EmitSignal(SignalName.EnemyListPhysicalDefeat);
		if (CheckForEnemyListEmotionalDefeat()) EmitSignal(SignalName.EnemyListEmotionalDefeat);

		bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
		var currStateId = _globalCombatSingleton.CombatStateMachineService.CurrentCombatState?.Id
			?? Enumerations.Combat.StateMachine.States.BiggieCombatMenu;

		if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatSpecialAttack
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatSpecialChat
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightChat
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightAttack
		)
		{
			////GD.Print("Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFrom...");
			if (!ProcessFirstPass())
			{
				HideBiggieAttackContainer();
				_nodeBiggieFightMove.IsActive = false;
				_nodeBiggieChatMove.IsActive = false;
				HideChatterTextBoxTextContainer();
				if (TransitionToEnemyAttack(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartEnemyAttackTurn);
					ShowEnemyAttackContainer();
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
					////GD.Print("EmitSignal(SignalName.StartOpponentTurn);");
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieCombatMenu)
		{
			////GD.Print("Enumerations.Combat.StateMachine.States.TransitionToBiggieCombatMenu");
			if (!ProcessFirstPass())
			{
				HideEnemyAttackContainer();
				HideChatterTextBoxTextContainer();
				if (TransitionToBiggieCombatMenu(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartBiggieTextTurn);
					ShowBiggieCombatMenuTextContainer();
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightAttack
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatSpecialAttack)
		{
			////GD.Print("Enumerations.Combat.StateMachine.States.TransitionToBiggieFight...");
			if (!ProcessFirstPass())
			{
				HideBiggieCombatMenuTextContainer();
				HideActionInfo();
				HideChatterTextBoxTextContainer();
				if (TransitionToBiggieAttack(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartBiggieAttackTurn);
					ShowBiggieAttackContainer();
					_nodeBiggieFightMove.IsActive = true;
					_nodeBiggieFightMove.Show();
					_nodeBiggieChatMove.IsActive = false;
					_nodeBiggieChatMove.Hide();
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightChat
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatSpecialChat)
		{
			////GD.Print("Enumerations.Combat.StateMachine.States.TransitionToBiggieChat...");
			if (!ProcessFirstPass())
			{
				HideBiggieCombatMenuTextContainer();
				HideActionInfo();
				HideChatterTextBoxTextContainer();
				if (TransitionToBiggieAttack(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartBiggieAttackTurn);
					ShowBiggieAttackContainer();
					_nodeBiggieFightMove.IsActive = false;
					_nodeBiggieFightMove.Hide();
					_nodeBiggieChatMove.IsActive = true;
					_nodeBiggieChatMove.Show();
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (_globalCombatSingleton.CombatStateMachineService.IsATransitionToChatterBox(currStateId))
		{
			////GD.Print("Enumerations.Combat.StateMachine.States.TransitionToChatterTextBox...");
			if (!ProcessFirstPass())
			{
				HideBiggieAttackContainer();
				_nodeBiggieFightMove.IsActive = false;
				_nodeBiggieChatMove.IsActive = false;
				HideEnemyAttackContainer();
				HideBiggieCombatMenuTextContainer();
				HideActionInfo();
				if (TransitionToChatterTextBox(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartChatterTextBoxTurn);
					ShowChatterTextBoxTextContainer();
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (_globalCombatSingleton.CombatStateMachineService.IsATargetEnemyTransition(currStateId))
		{
			//GD.Print("Hello World");
		}
	}

	[Signal]
	public delegate void StartEnemyAttackTurnEventHandler();
	[Signal]
	public delegate void StartBiggieTextTurnEventHandler();
	[Signal]
	public delegate void StartBiggieAttackTurnEventHandler();
	[Signal]
	public delegate void StartChatterTextBoxTurnEventHandler();

	public bool TranslateHudToEnemyAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack);
		}

		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_SLOW;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack);
		return false;
	}

	public bool TranslateHudToBiggieAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack);
		}

		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack);
		return false;
	}

	public bool TranslateHudToBiggieCombatMenu(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieCombatMenu);
		}

		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieCombatMenu))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieCombatMenu);
		return false;
	}

	public bool TranslateHudToChatterTextBox(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeHudContainerSubject, _nodeHudContainerTargetChatterTextBox);
		}

		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetChatterTextBox))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetChatterTextBox);
		return false;
	}

	public bool TransformToEnemyAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeSubjectPanel, _nodeEnemyAttackPanel);
		}

		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeEnemyAttackPanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeEnemyAttackPanel))
		{
			_nodeEnemyAttackContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeEnemyAttackPanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeEnemyAttackPanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeEnemyAttackPanel);
		}
		else if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeEnemyAttackPanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeEnemyAttackPanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeEnemyAttackPanel);
		}
		else
		{
			//////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeEnemyAttackPanel)}");
		}
		return false;
	}

	public bool TransformToBiggieAttack(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeSubjectPanel, _nodeBiggieAttackPanel);
		}

		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeBiggieAttackPanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeBiggieAttackPanel))
		{
			_nodeBiggieAttackContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeBiggieAttackPanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBiggieAttackPanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeBiggieAttackPanel);
		}
		else if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeBiggieAttackPanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeBiggieAttackPanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeBiggieAttackPanel);
		}
		else
		{
			//////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBiggieAttackPanel)}");
		}
		return false;
	}

	public bool TransformToBiggieCombatMenu(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeSubjectPanel, _nodeBasePagePanel);
		}

		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeBasePagePanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeBasePagePanel))
		{
			_nodeBiggieCombatMenuTextContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeBasePagePanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBasePagePanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeBasePagePanel);
		}
		else if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeBasePagePanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeBasePagePanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeBasePagePanel);
		}
		else
		{
			//////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBasePagePanel)}");
		}
		return false;
	}

	public bool TransformToChatterTextBox(bool skip)
	{
		if (skip)
		{
			HudAnimationHelper.SkipAnimation(_nodeSubjectPanel, _nodeChatterTextBoxPanel);
		}

		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeChatterTextBoxPanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeChatterTextBoxPanel))
		{
			_nodeEnemyAttackContainer.Show();
			return true;
		}

		if (!MainAnimationHelper.CheckPositionX(_nodeSubjectPanel, _nodeChatterTextBoxPanel)
			|| !MainAnimationHelper.CheckSizeX(_nodeSubjectPanel, _nodeChatterTextBoxPanel))
		{
			MainAnimationHelper.CenterScaleX(_nodeSubjectPanel, _nodeChatterTextBoxPanel);
		}
		else if (!MainAnimationHelper.CheckPositionY(_nodeSubjectPanel, _nodeChatterTextBoxPanel)
			|| !MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeChatterTextBoxPanel))
		{
			MainAnimationHelper.MonoScaleY(_nodeSubjectPanel, _nodeChatterTextBoxPanel);
		}
		else
		{
			//////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeEnemyAttackPanel)}");
		}
		return false;
	}

	private HBoxContainer CreateHudContainerTarget(HBoxContainer subject, float diffX, float diffY)
	{
		var result = _nodeHudContainerSubject.Duplicate() as HBoxContainer;
		result.Position = new Vector2(
			result.Position.X + diffX, result.Position.Y + diffY
		);
		return result;
	}

	public void ShowEnemyAttackContainer()
	{
		_nodeEnemyAttackContainer.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideEnemyAttackContainer()
	{
		_nodeEnemyAttackContainer.Modulate = new Color(1, 1, 1, 0);
	}

	public void ShowBiggieAttackContainer()
	{
		_nodeBiggieAttackContainer.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideBiggieAttackContainer()
	{
		_nodeBiggieAttackContainer.Modulate = new Color(1, 1, 1, 0);
	}

	public void ShowBiggieCombatMenuTextContainer()
	{
		_nodeBiggieCombatMenuTextContainer.Modulate = new Color(1, 1, 1, 1);
		_nodeBiggieCombatMenu.StartTurn();
	}

	public void HideBiggieCombatMenuTextContainer()
	{
		_nodeBiggieCombatMenuTextContainer.Modulate = new Color(1, 1, 1, 0);
	}

	public void ShowChatterTextBoxTextContainer()
	{
		_nodeChatterTextBoxTextContainer.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideChatterTextBoxTextContainer()
	{
		_nodeChatterTextBoxTextContainer.Modulate = new Color(1, 1, 1, 0);
	}

	public void ShowActionInfo()
	{
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideActionInfo()
	{
		////GD.Print("CombatWrapper.HideActionInfo");
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 0);
	}

	public void ShowSubjectPanel()
	{
		_nodeSubjectPanel.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideSubjectPanel()
	{
		_nodeSubjectPanel.Modulate = new Color(1, 1, 1, 0);
	}

	public void HandleFightSelection(int selectedIndex)
	{
		if (selectedIndex != (int)Enumerations.Combat.FightPagePanelOptions.Back)
		{
			//_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		}
	}

	public void HandleChatSelection(int selectedIndex)
	{
		if (selectedIndex != (int)Enumerations.Combat.ChatPagePanelOptions.Back)
		{
			//_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		}
	}

	private bool TransitionToBiggieAttack(bool skip)
	{
		bool finished = false;
		finished = TranslateHudToBiggieAttack(skip);
		finished = TransformToBiggieAttack(skip) && finished;
		return finished;
	}

	private bool TransitionToEnemyAttack(bool skip)
	{
		bool finished = false;
		finished = TranslateHudToEnemyAttack(skip);
		finished = TransformToEnemyAttack(skip) && finished;
		return finished;
	}

	private bool TransitionToBiggieCombatMenu(bool skip)
	{
		bool finished = false;
		finished = TranslateHudToBiggieCombatMenu(skip);
		finished = TransformToBiggieCombatMenu(skip) && finished;
		return finished;
	}

	private bool TransitionToChatterTextBox(bool skip)
	{
		bool finished = false;
		finished = TranslateHudToChatterTextBox(skip);
		//finished = TransformToChatterTextBox(skip) && finished;
		return finished;
	}

	private void HandleEndBiggieAttackTurn(float damagePercentage, bool isPerfect, bool isTrash)
	{
		switch (LastCombatOptionUsed)
		{
			case Enumerations.Combat.CombatOptions.Attack:
				DealPhysicalDamage(3 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.Chat:
				DealEmotionalDamage(3 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.SpecialAttack:
				DealPhysicalDamage(5 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.SpecialChat:
				DealEmotionalDamage(5 * damagePercentage);
				break;
			default:
				GD.Print("CombatWrapper HandleEndBiggieAttackTurn LastCombatOptionUsed did not map.");
				break;
		}

		if (isPerfect)
		{
			_globalCombatSingleton.SpecialMeter.AddToSpecialMeter(1);
		}
		else if (isTrash)
		{
			_globalCombatSingleton.SpecialMeter.AddToSpecialMeter(-1);
		}

		Action action = () =>
		{
			//_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		};
		HelperFunctions.SetTimeout(action);
	}

	[Signal]
	public delegate void BiggieDefeatEventHandler();
	[Signal]
	public delegate void EnemyListPhysicalDefeatEventHandler();
	[Signal]
	public delegate void EnemyListEmotionalDefeatEventHandler();

	private void HandleEndBiggieCombatMenuTurn(int combatOption, int enemyTargetIndex)
	{
		LastCombatOptionUsed = (Enumerations.Combat.CombatOptions)combatOption;
		_nodeBiggieCombatMenu.Visible = false;
		_nodeBiggieCombatMenu.EndTurn();

		switch ((Enumerations.Combat.CombatOptions)combatOption)
		{
			case Enumerations.Combat.CombatOptions.SpecialAttack:
				_globalCombatSingleton.TargetedBiggieEmotionalAttackProxy = _globalCombatSingleton.EnemyTargetList[enemyTargetIndex].BiggieEmotionalAttackProxy;
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectChatSpecialAttack);
				break;
			case Enumerations.Combat.CombatOptions.SpecialChat:
				_globalCombatSingleton.TargetedBiggieEmotionalAttackProxy = _globalCombatSingleton.EnemyTargetList[enemyTargetIndex].BiggieEmotionalAttackProxy;
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectChatSpecialChat);
				break;
			case Enumerations.Combat.CombatOptions.Attack:
				_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy = _globalCombatSingleton.EnemyTargetList[enemyTargetIndex].BiggiePhysicalAttackProxy;
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectFightAttack);
				break;
			case Enumerations.Combat.CombatOptions.Chat:
				_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy = _globalCombatSingleton.EnemyTargetList[enemyTargetIndex].BiggiePhysicalAttackProxy;
				_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.SelectFightChat);
				break;
			default:
				//GD.Print("CombatSceneDjBattle.EndBiggieCombatMenuTurn: Could not map combat options");
				break;
		}

		return;
	}

	[Signal]
	public delegate void ProjectPhysicalDamageEventHandler();
	public void DealPhysicalDamage(double damage)
	{
		_globalCombatSingleton.TargetedBiggiePhysicalAttackProxy.DealDamage(damage);
		int id = CheckForEnemyTargetDefeat();
		if (id > -1) HandleEnemyTargetDefeat(id);
		EmitSignal(SignalName.ProjectPhysicalDamage);
	}

	[Signal]
	public delegate void ProjectEmotionalDamageEventHandler();
	public void DealEmotionalDamage(double damage)
	{
		_globalCombatSingleton.TargetedBiggieEmotionalAttackProxy.DealDamage(damage);
		int id = CheckForEnemyTargetDefeat();
		if (id > -1) HandleEnemyTargetDefeat(id);
		EmitSignal(SignalName.ProjectPhysicalDamage);
	}

	private int CheckForEnemyTargetDefeat()
	{
		foreach (var target in _globalCombatSingleton.EnemyTargetList)
		{
			if (target.BiggiePhysicalAttackProxy.IsTargetDefeated()
				|| target.BiggieEmotionalAttackProxy.IsTargetDefeated())
			{
				return target.Id;
			}
		}
		return -1;
	}

	private void HandleEnemyTargetDefeat(int id)
	{
		GD.Print("HandleTargetDefeat");
		GD.Print($"Before remove EnemyTargetList.Count : {_globalCombatSingleton.EnemyTargetList.Count}");
		var target = _globalCombatSingleton.EnemyTargetList
			.Where(x => x.Id == id)
			.First();
		target.TargetPanel.QueueFree();
		_globalCombatSingleton.EnemyTargetList.Remove(target);
		GD.Print($"After remove EnemyTargetList.Count : {_globalCombatSingleton.EnemyTargetList.Count}");
	}

	public void SetEnemyAttackContainerService(MarginContainer enemyAttackContainer)
	{
		_globalCombatSingleton.EnemyAttackPanelService = new EnemyAttackPanelService();

		var enemyAttackPanelSize = new Vector2(enemyAttackContainer.GetRect().Size.X - enemyAttackContainer.GetThemeConstant("margin_left") - enemyAttackContainer.GetThemeConstant("margin_right"),
			enemyAttackContainer.GetRect().Size.Y - enemyAttackContainer.GetThemeConstant("margin_top") - enemyAttackContainer.GetThemeConstant("margin_bottom"));
		var enemyAttackPanelPosition = new Vector2(enemyAttackContainer.GetRect().Position.X + enemyAttackContainer.GetThemeConstant("margin_left"),
			enemyAttackContainer.GetRect().Position.Y + enemyAttackContainer.GetThemeConstant("margin_top"));

		//GD.Print("CombatWrapper SetEnemyAttackContainerService");
		_globalCombatSingleton.EnemyAttackPanelService.Size = enemyAttackPanelSize;
		//GD.Print($"CombatWrapper _globalCombatSingleton.EnemyAttackPanelService.Size {_globalCombatSingleton.EnemyAttackPanelService.Size.X} {_globalCombatSingleton.EnemyAttackPanelService.Size.Y}");
		_globalCombatSingleton.EnemyAttackPanelService.Position = enemyAttackPanelPosition;
		//GD.Print($"CombatWrapper _globalCombatSingleton.EnemyAttackPanelService.Position {_globalCombatSingleton.EnemyAttackPanelService.Position.X} {_globalCombatSingleton.EnemyAttackPanelService.Position.Y}");
	}

	public bool CheckForBiggeDefeat()
	{
		return _globalCombatSingleton.EnemyPhysicalAttackProxy.IsTargetDefeated() && !_nodeChatterTextBox.IsOpen();
	}

	public bool CheckForEnemyListPhysicalDefeat()
	{
		return !_nodeChatterTextBox.IsOpen()
			&& _globalCombatSingleton.AreAnyEnemiesPhysicallyDefeated()
			&& _globalCombatSingleton.AreAllEnemiesDefeated();
	}

	public bool CheckForEnemyListEmotionalDefeat()
	{
		return !_nodeChatterTextBox.IsOpen()
			&& !_globalCombatSingleton.AreAnyEnemiesPhysicallyDefeated()
			&& _globalCombatSingleton.AreAllEnemiesDefeated();
	}

	public void ChangeBiggieHealthBar()
	{
		var tween = _nodeBiggieHpProgressBar.GetTree().CreateTween();
		tween.TweenProperty(_nodeBiggieHpProgressBar, "value", _globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetHealthPercentage(), 1).SetTrans(Tween.TransitionType.Linear);
		_nodeBiggieHpValueLabel.Text = $"{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetCurrentHealth()}/{_globalCombatSingleton.EnemyPhysicalAttackProxy.GetTargetMaxHealth()}";
	}
}
