using Godot;
using System;

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
	private BiggieAttackContainer _nodeBiggieAttackContainer = null;
	private Panel _nodeBiggieAttackPanel = null;

	private PanelAnimationHelper HudAnimationHelper { get; set; }
	private static readonly float _HUD_SPEED_SLOW = 5f;
	private static readonly float _HUD_SPEED_FAST = 8f;
	private PanelAnimationHelper MainAnimationHelper { get; set; }
	private static readonly float _MAIN_SPEED = 20f;

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
		_nodeBiggieAttackContainer = GetNode<BiggieAttackContainer>("./BiggieAttackContainer");
		_nodeBiggieAttackPanel = GetNode<Panel>("./BiggieAttackContainer/BiggieAttackPanel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		SetEnemyAttackContainerService(_nodeEnemyAttackContainer);

		_nodeFightPageBasePanel.SelectFight += HandleFightSelection;
		_nodeChatPageBasePanel.SelectChat += HandleChatSelection;
		_nodeBiggieCombatMenu.EndBiggieCombatMenuTurn += HandleEndBiggieCombatMenuTurn;
		_nodeBiggieCombatMenu.ShowActionInfo += ShowActionInfo;
		_nodeBiggieCombatMenu.HideActionInfo += HideActionInfo;
		_nodeBiggieAttackContainer.EndBiggieAttackTurn += HandleEndBiggieAttackTurn;

		//ApplyCombatStateMachineEvents();

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
		bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
		var currStateId = _globalCombatSingleton.CombatStateMachineService.CurrentCombatState?.Id
			?? Enumerations.Combat.StateMachine.States.BiggieCombatMenu;

		if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatAsk
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatCharm
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightBite
			|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightScratch
		)
		{
			//GD.Print("Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFrom...");
			if (!ProcessFirstPass())
			{
				HideBiggieAttackContainer();
				_nodeBiggieAttackContainer.IsActive = false;
				HideChatterTextBoxTextContainer();
				if (TransitionToEnemyAttack(skipTransition))
				{
					FirstFramePass = true;
					HideSubjectPanel();
					EmitSignal(SignalName.StartEnemyAttackTurn);
					ShowEnemyAttackContainer();
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
					//GD.Print("EmitSignal(SignalName.StartOpponentTurn);");
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieCombatMenu)
		{
			//GD.Print("Enumerations.Combat.StateMachine.States.TransitionToBiggieCombatMenu");
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
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightScratch
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieFightBite)
		{
			//GD.Print("Enumerations.Combat.StateMachine.States.TransitionToBiggieFight...");
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
					_nodeBiggieAttackContainer.IsActive = true;
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatAsk
				|| currStateId == Enumerations.Combat.StateMachine.States.TransitionToBiggieChatCharm)
		{
			//GD.Print("Enumerations.Combat.StateMachine.States.TransitionToBiggieChat...");
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
					_nodeBiggieAttackContainer.IsActive = true;
					_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishTransition);
				}
			}
		}
		else if (_globalCombatSingleton.CombatStateMachineService.IsATransitionToChatterBox(currStateId))
		{
			//GD.Print("Enumerations.Combat.StateMachine.States.TransitionToChatterTextBox...");
			if (!ProcessFirstPass())
			{
				HideBiggieAttackContainer();
				_nodeBiggieAttackContainer.IsActive = false;
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
			////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeEnemyAttackPanel)}");
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
			////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBiggieAttackPanel)}");
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
			////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeBasePagePanel)}");
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
			////GD.Print($"SizeY {MainAnimationHelper.CheckSizeY(_nodeSubjectPanel, _nodeEnemyAttackPanel)}");
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
		//GD.Print("CombatWrapper.HideActionInfo");
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
		finished = TransformToChatterTextBox(skip) && finished;
		return finished;
	}

	private void HandleEndBiggieAttackTurn(double damagePercentage)
	{
		switch (LastCombatOptionUsed)
		{
			case Enumerations.Combat.CombatOptions.Scratch:
				DealPhysicalDamage(2 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.Bite:
				DealPhysicalDamage(3 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.Ask:
				DealEmotionalDamage(2 * damagePercentage);
				break;
			case Enumerations.Combat.CombatOptions.Charm:
				DealEmotionalDamage(3 * damagePercentage);
				break;
			default:
				GD.Print("CombatWrapper HandleEndBiggieAttackTurn LastCombatOptionUsed did not map.");
				break;
		}

		Action action = () =>
		{
			//_globalCombatSingleton.CombatStateMachineService.EmitCombatEvent(Enumerations.Combat.StateMachine.Events.FinishBiggieAttack);
		};
		HelperFunctions.SetTimeout(action);

	}

	private void HandleEndBiggieCombatMenuTurn(int combatOption)
	{
		LastCombatOptionUsed = (Enumerations.Combat.CombatOptions)combatOption;
	}

	[Signal]
	public delegate void ProjectPhysicalDamageEventHandler();
	public void DealPhysicalDamage(double damage)
	{
		_globalCombatSingleton.BiggiePhysicalAttackProxy.DealDamage(damage);
		GD.Print($"enemy health: {_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetHealthPercentage()}");
		EmitSignal(SignalName.ProjectPhysicalDamage);
	}

	public void DealEmotionalDamage(double damage)
	{
		_globalCombatSingleton.BiggieEmotionalAttackProxy.DealDamage(damage);
	}

	public void SetEnemyAttackContainerService(MarginContainer enemyAttackContainer)
	{
		var enemyAttackPanelSize = new Vector2(enemyAttackContainer.GetRect().Size.X - enemyAttackContainer.GetThemeConstant("margin_left") - enemyAttackContainer.GetThemeConstant("margin_right"),
			enemyAttackContainer.GetRect().Size.Y - enemyAttackContainer.GetThemeConstant("margin_top") - enemyAttackContainer.GetThemeConstant("margin_bottom"));
		var enemyAttackPanelPosition = new Vector2(enemyAttackContainer.GetRect().Position.X + enemyAttackContainer.GetThemeConstant("margin_left"),
			enemyAttackContainer.GetRect().Position.Y + enemyAttackContainer.GetThemeConstant("margin_top"));

		GD.Print("CombatWrapper SetEnemyAttackContainerService");
		_globalCombatSingleton.EnemyAttackPanelService.Size = enemyAttackPanelSize;
		GD.Print($"CombatWrapper _globalCombatSingleton.EnemyAttackPanelService.Size {_globalCombatSingleton.EnemyAttackPanelService.Size.X} {_globalCombatSingleton.EnemyAttackPanelService.Size.Y}");
		_globalCombatSingleton.EnemyAttackPanelService.Position = enemyAttackPanelPosition;
		GD.Print($"CombatWrapper _globalCombatSingleton.EnemyAttackPanelService.Position {_globalCombatSingleton.EnemyAttackPanelService.Position.X} {_globalCombatSingleton.EnemyAttackPanelService.Position.Y}");
	}

	// private void ApplyCombatStateMachineEvents()
	// {
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieChatAsk)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToBiggieChatCharm)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToEnemyAttackFromBiggieFightBite)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToBiggieFightScratch)
	// 		.AddEventHandler(TransitionToEnemyAttackEvent);
	// 	_globalCombatSingleton.CombatStateMachineService.GetStateById(Enumerations.Combat.StateMachine.States.TransitionToCombatMenu)
	// 		.AddEventHandler(TransitionToBiggieCombatMenuEvent);
	// }

	// private bool TransitionToEnemyAttackEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideBiggieAttackContainer();
	// 		_nodeBiggieAttackContainer.IsActive = false;
	// 		if (TransitionToEnemyAttack(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartEnemyAttackTurn);
	// 			ShowEnemyAttackContainer();
	// 			return true;
	// 		}
	// 	}
	// 	return false;
	// }

	// private bool TransitionToBiggieCombatMenuEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideEnemyAttackContainer();
	// 		if (TransitionToBiggieCombatMenu(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartBiggieTextTurn);
	// 			ShowBiggieCombatMenuTextContainer();
	// 			return true;
	// 		}
	// 	}
	// 	return false;
	// }

	// private void TransitionToBiggieFightEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideBiggieCombatMenuTextContainer();
	// 		HideActionInfo();
	// 		if (TransitionToBiggieAttack(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartBiggieAttackTurn);
	// 			ShowBiggieAttackContainer();
	// 			_nodeBiggieAttackContainer.IsActive = true;
	// 		}
	// 	}
	// }

	// private void TransitionToBiggieChatEvent()
	// {
	// 	bool skipTransition = Input.IsActionJustPressed(_INTERACT_INPUT);
	// 	if (!ProcessFirstPass())
	// 	{
	// 		HideBiggieCombatMenuTextContainer();
	// 		HideActionInfo();
	// 		if (TransitionToBiggieAttack(skipTransition))
	// 		{
	// 			FirstFramePass = true;
	// 			HideSubjectPanel();
	// 			EmitSignal(SignalName.StartBiggieAttackTurn);
	// 			ShowBiggieAttackContainer();
	// 			_nodeBiggieAttackContainer.IsActive = true;
	// 		}
	// 	}
	// }
}
