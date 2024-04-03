using Godot;
using System;

public partial class CombatWrapper : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	private Panel _nodeSubjectPanel = null;
	private Panel _nodeBasePagePanel = null;
	private FightPageBasePanel _nodeFightPageBasePanel = null;
	private ChatPageBasePanel _nodeChatPageBasePanel = null;
	private HBoxContainer _nodeHudContainerSubject = null;
	private HBoxContainer _nodeHudContainerTargetEnemyAttack = null;
	private HBoxContainer _nodeHudContainerTargetBiggieAttack = null;
	private HBoxContainer _nodeHudContainerTargetText = null;

	private MarginContainer _nodeActionInfo = null;
	private BiggieCombatTextBox _nodeBiggieCombatTextBox = null;
	private MarginContainer _nodeTextContainer = null;
	private MarginContainer _nodeEnemyAttackContainer = null;
	private Panel _nodeEnemyAttackPanel = null;
	private MarginContainer _nodeBiggieAttackContainer = null;
	private Panel _nodeBiggieAttackPanel = null;

	private PanelAnimationHelper HudAnimationHelper { get; set; }
	private static readonly float _HUD_SPEED_SLOW = 5f;
	private static readonly float _HUD_SPEED_FAST = 8f;
	private PanelAnimationHelper MainAnimationHelper { get; set; }
	private static readonly float _MAIN_SPEED = 20f;

	private CombatSingleton _globalCombatSingleton = null;
	private Enumerations.CombatOptions LastCombatOptionUsed { get; set; }

	public override void _Ready()
	{
		// note the containers wrapping the panels must have the same position and size
		// the containers can use margin to change panel size for animation
		_nodeSubjectPanel = GetNode<Panel>("./TransformContainer/TransformPanel");
		_nodeBasePagePanel = GetNode<Panel>("./BiggieCombatTextBox/TextBoxContainer/BasePagePanel");
		_nodeFightPageBasePanel = GetNode<FightPageBasePanel>("./BiggieCombatTextBox/TextBoxContainer/FightPagePanel");
		_nodeChatPageBasePanel = GetNode<ChatPageBasePanel>("./BiggieCombatTextBox/TextBoxContainer/ChatPagePanel");
		_nodeHudContainerSubject = GetNode<HBoxContainer>("./HudContainer");
		// Need to Queue Free this at some point
		_nodeHudContainerTargetEnemyAttack = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 250);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetBiggieAttack = CreateHudContainerTarget(_nodeHudContainerSubject, 0, -160);
		// Need to Queue Free this at some point
		_nodeHudContainerTargetText = CreateHudContainerTarget(_nodeHudContainerSubject, 0, 0);
		HudAnimationHelper = new PanelAnimationHelper(_HUD_SPEED_SLOW);
		MainAnimationHelper = new PanelAnimationHelper(_MAIN_SPEED);

		_nodeActionInfo = GetNode<MarginContainer>("HudContainer/ActionInfo");
		_nodeBiggieCombatTextBox = GetNode<BiggieCombatTextBox>("./BiggieCombatTextBox");
		_nodeTextContainer = GetNode<MarginContainer>("./BiggieCombatTextBox/TextBoxContainer");
		_nodeEnemyAttackContainer = GetNode<MarginContainer>("./EnemyAttackContainer");
		_nodeEnemyAttackPanel = GetNode<Panel>("./EnemyAttackContainer/EnemyAttackPanel");
		_nodeBiggieAttackContainer = GetNode<MarginContainer>("./BiggieAttackContainer");
		_nodeBiggieAttackPanel = GetNode<Panel>("./BiggieAttackContainer/BiggieAttackPanel");

		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");

		_nodeFightPageBasePanel.SelectFight += HandleFightSelection;
		_nodeChatPageBasePanel.SelectChat += HandleChatSelection;
		_nodeBiggieCombatTextBox.EndBiggieTextTurn += HandleEndBiggieTextTurn;
		_nodeBiggieCombatTextBox.HideActionInfo += HideActionInfo;

		HideEnemyAttackContainer();
		HideBiggieAttackContainer();

		FirstFramePass = true;
	}

	private bool FirstFramePass { get; set; }
	private bool ProcessFirstPass()
	{
		if (FirstFramePass)
		{
			_nodeSubjectPanel.Show();
			FirstFramePass = false;
			return true;
		}
		return false;
	}
	public override void _Process(double delta)
	{
		if (_globalCombatSingleton.CombatState == Enumerations.CombatStates.TransitionToEnemyAttack)
		{
			if (!ProcessFirstPass())
			{
				HideBiggieAttackContainer();
				if (TransitionToEnemyAttack())
				{
					FirstFramePass = true;
					_nodeSubjectPanel.Hide();
					EmitSignal(SignalName.StartEnemyAttackTurn);
					ShowEnemyAttackContainer();
					_globalCombatSingleton.CombatState = Enumerations.CombatStates.EnemyAttack;
					//GD.Print("EmitSignal(SignalName.StartOpponentTurn);");
				}
			}
		}
		else if (_globalCombatSingleton.CombatState == Enumerations.CombatStates.TransitionToText)
		{
			GD.Print("TransitionToText");
			if (!ProcessFirstPass())
			{
				HideEnemyAttackContainer();
				if (TransitionToText())
				{
					FirstFramePass = true;
					_nodeSubjectPanel.Hide();
					EmitSignal(SignalName.StartBiggieTextTurn);
					ShowBiggieTextContainer();
					_globalCombatSingleton.CombatState = Enumerations.CombatStates.Text;
				}
			}
		}
		else if (_globalCombatSingleton.CombatState == Enumerations.CombatStates.TransitionToBiggieFight)
		{
			if (!ProcessFirstPass())
			{
				HideBiggieTextContainer();
				HideActionInfo();
				if (TransitionToBiggieAttack())
				{
					FirstFramePass = true;
					_nodeSubjectPanel.Hide();
					EmitSignal(SignalName.StartBiggieAttackTurn);
					ShowBiggieAttackContainer();
					_globalCombatSingleton.CombatState = Enumerations.CombatStates.BiggieFight;
				}
			}
		}
		else if (_globalCombatSingleton.CombatState == Enumerations.CombatStates.TransitionToBiggieChat)
		{
			if (!ProcessFirstPass())
			{
				HideBiggieTextContainer();
				HideActionInfo();
				if (TransitionToBiggieAttack())
				{
					FirstFramePass = true;
					_nodeSubjectPanel.Hide();
					EmitSignal(SignalName.StartBiggieAttackTurn);
					ShowBiggieAttackContainer();
					_globalCombatSingleton.CombatState = Enumerations.CombatStates.BiggieChat;
				}
			}
		}

		if ((_globalCombatSingleton.CombatState == Enumerations.CombatStates.BiggieChat
			|| _globalCombatSingleton.CombatState == Enumerations.CombatStates.BiggieFight)
			&& Input.IsActionJustPressed(_INTERACT_INPUT))
		{
			_globalCombatSingleton.CombatState = Enumerations.CombatStates.TransitionToEnemyAttack;
		}
	}

	[Signal]
	public delegate void StartEnemyAttackTurnEventHandler();
	[Signal]
	public delegate void StartBiggieTextTurnEventHandler();
	[Signal]
	public delegate void StartBiggieAttackTurnEventHandler();

	public bool TranslateHudEnemyAttack()
	{
		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_SLOW;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetEnemyAttack);
		return false;
	}

	public bool TranslateHudBiggieAttack()
	{
		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetBiggieAttack);
		return false;
	}

	public bool TranslateHudText()
	{
		if (HudAnimationHelper.CheckPosition(_nodeHudContainerSubject, _nodeHudContainerTargetText))
		{
			return true;
		}
		HudAnimationHelper.AnimationSpeed = _HUD_SPEED_FAST;
		HudAnimationHelper.TranslateOverTime(_nodeHudContainerSubject, _nodeHudContainerTargetText);
		return false;
	}

	public bool TransformToEnemyAttack()
	{
		//GD.Print("TransformToEnemyAttack");
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

	public bool TransformToBiggieAttack()
	{
		//GD.Print("TransformToBiggieAttack");
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

	public bool TransformToText()
	{
		//GD.Print("TransformToText");
		if (MainAnimationHelper.CheckPosition(_nodeSubjectPanel, _nodeBasePagePanel)
			&& MainAnimationHelper.CheckSize(_nodeSubjectPanel, _nodeBasePagePanel))
		{
			_nodeTextContainer.Show();
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
		_nodeEnemyAttackContainer.Show();
	}

	public void HideEnemyAttackContainer()
	{
		_nodeEnemyAttackContainer.Hide();
	}

	public void ShowBiggieAttackContainer()
	{
		_nodeBiggieAttackContainer.Show();
	}

	public void HideBiggieAttackContainer()
	{
		_nodeBiggieAttackContainer.Hide();
	}

	public void ShowBiggieTextContainer()
	{
		_nodeTextContainer.Show();
		_nodeBiggieCombatTextBox.StartTurn();
	}

	public void HideBiggieTextContainer()
	{
		_nodeTextContainer.Hide();
	}

	public void ShowActionInfo()
	{
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 1);
	}

	public void HideActionInfo()
	{
		_nodeActionInfo.Modulate = new Color(1, 1, 1, 0);
	}

	public void HandleFightSelection(int selectedIndex)
	{
		if (selectedIndex != (int)Enumerations.FightPagePanelOptions.Back)
		{
			_globalCombatSingleton.CombatState = Enumerations.CombatStates.TransitionToBiggieFight;
		}
	}

	public void HandleChatSelection(int selectedIndex)
	{
		if (selectedIndex != (int)Enumerations.ChatPagePanelOptions.Back)
		{
			_globalCombatSingleton.CombatState = Enumerations.CombatStates.TransitionToBiggieChat;
		}
	}

	private bool TransitionToBiggieAttack()
	{
		bool finished = false;
		finished = TranslateHudBiggieAttack();
		finished = TransformToBiggieAttack() && finished;
		return finished;
	}

	private bool TransitionToEnemyAttack()
	{
		bool finished = false;
		finished = TranslateHudEnemyAttack();
		finished = TransformToEnemyAttack() && finished;
		return finished;
	}

	private bool TransitionToText()
	{
		bool finished = false;
		finished = TranslateHudText();
		finished = TransformToText() && finished;
		return finished;
	}

	private void HandleEndBiggieAttackTurn(int damagePercentage)
	{
		switch (LastCombatOptionUsed)
		{
			case Enumerations.CombatOptions.Scratch:
				DealPhysicalDamage(1 * damagePercentage);
				break;
			case Enumerations.CombatOptions.Bite:
				DealPhysicalDamage(2 * damagePercentage);
				break;
			case Enumerations.CombatOptions.Ask:
				DealEmotionalDamage(1 * damagePercentage);
				break;
			case Enumerations.CombatOptions.Charm:
				DealEmotionalDamage(2 * damagePercentage);
				break;
			default:
				break;
		}
	}

	private void HandleEndBiggieTextTurn(int combatOption)
	{
		LastCombatOptionUsed = (Enumerations.CombatOptions)combatOption;
	}

	[Signal]
	public delegate void ProjectPhysicalDamageEventHandler();
	public void DealPhysicalDamage(int damage)
	{
		//GD.Print("DealPhysicalDamage");
		_globalCombatSingleton.BiggiePhysicalAttackProxy.DealDamage(damage);
		//GD.Print($"enemy health: {_globalCombatSingleton.BiggiePhysicalAttackProxy.GetTargetHealthPercentage()}");
		EmitSignal(SignalName.ProjectPhysicalDamage);
	}

	public void DealEmotionalDamage(int damage)
	{
		_globalCombatSingleton.BiggieEmotionalAttackProxy.DealDamage(damage);
	}
}
