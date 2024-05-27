using Godot;
using System;

public partial class PauseMenu : Node2D
{
	private static readonly StringName _SCENE_MAIN_MENU = new StringName("res://Main.tscn");

	private static readonly StringName _PAUSE_INPUT = new StringName("escape");
	private static readonly StringName _TAB_INPUT = new StringName("tab");
	private static readonly StringName _ENTER_INPUT = new StringName("enter");
	private static readonly StringName _SPACE_INPUT = new StringName("interact");
	private static readonly StringName _UP_INPUT = new StringName("move_up");
	private static readonly StringName _RIGHT_INPUT = new StringName("move_right");
	private static readonly StringName _DOWN_INPUT = new StringName("move_down");
	private static readonly StringName _LEFT_INPUT = new StringName("move_left");

	private Panel _nodeBasePanel = null;
	private Panel _nodeUserSettingsPanel = null;
	private OpenClosePauseMenuListener _nodeOpenClosePauseMenuListener = null;

	private BaseButton _nodeResume = null;
	private BaseButton _nodeUserSettings = null;
	private BaseButton _nodeMainMenu = null;
	public BaseButton[] BasePanelButtons { get; set; }
	public int BasePanelFocusIndex { get; set; }

	private BaseButton _nodeFxSound = null;
	private BaseButton _nodeUserSettingsBack = null;
	private BaseButton[] UserSettingsButtons { get; set; }
	private int UserSettingsFocusIndex { get; set; }

	private AudioStreamPlayer _audioSelect = null;
	private AudioStreamPlayer _audioSwitch = null;


	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.WhenPaused;

		_nodeBasePanel = GetNode<Panel>("./MarginContainer/BasePanel");
		_nodeResume = GetNode<BaseButton>("./MarginContainer/BasePanel/PaddingContainer/VBoxContainer/ResumeButton");
		_nodeUserSettings = GetNode<BaseButton>("./MarginContainer/BasePanel/PaddingContainer/VBoxContainer/UserSettingsButton");
		_nodeMainMenu = GetNode<BaseButton>("./MarginContainer/BasePanel/PaddingContainer/VBoxContainer/MainMenuButton");
		BasePanelButtons = new BaseButton[] { _nodeResume, _nodeUserSettings, _nodeMainMenu };

		_nodeUserSettingsPanel = GetNode<Panel>("./MarginContainer/UserSettingsPanel");
		_nodeFxSound = GetNode<BaseButton>("./MarginContainer/UserSettingsPanel/PaddingContainer/VBoxContainer/FxSoundContainer/TextureButton");
		_nodeUserSettingsBack = GetNode<BaseButton>("./MarginContainer/UserSettingsPanel/PaddingContainer/VBoxContainer/UserSettingsBackButton");
		UserSettingsButtons = new BaseButton[] { _nodeFxSound, _nodeUserSettingsBack };

		_nodeOpenClosePauseMenuListener = GetNode<OpenClosePauseMenuListener>("./OpenClosePauseMenuListener");
		_nodeOpenClosePauseMenuListener.OpenMenu += HandleOpenMenu;

		_audioSelect = GetNode<AudioStreamPlayer>("./Select_AudioStreamPlayer");
		_audioSwitch = GetNode<AudioStreamPlayer>("./Switch_AudioStreamPlayer");
	}

	public override void _Process(double _delta)
	{
		if (Input.IsActionJustPressed(_UP_INPUT))
		{
			_audioSwitch.Play();
			if (_nodeBasePanel.Visible)
			{
				int len = BasePanelButtons.Length;
				if (BasePanelFocusIndex == 0)
				{
					BasePanelButtons[len - 1].GrabFocus();
					BasePanelFocusIndex = len - 1;
				}
				else
				{
					BasePanelButtons[BasePanelFocusIndex - 1].GrabFocus();
					BasePanelFocusIndex -= 1;
				}
			}
			if (_nodeUserSettingsPanel.Visible)
			{
				int len = UserSettingsButtons.Length;
				if (UserSettingsFocusIndex == 0)
				{
					UserSettingsButtons[len - 1].GrabFocus();
					UserSettingsFocusIndex = len - 1;
				}
				else
				{
					UserSettingsButtons[UserSettingsFocusIndex - 1].GrabFocus();
					UserSettingsFocusIndex -= 1;
				}
			}
		}

		if (Input.IsActionJustPressed(_TAB_INPUT)
			|| Input.IsActionJustPressed(_DOWN_INPUT))
		{
			_audioSwitch.Play();
			if (_nodeBasePanel.Visible)
			{
				int len = BasePanelButtons.Length;
				if (BasePanelFocusIndex == len - 1)
				{
					BasePanelButtons[0].GrabFocus();
					BasePanelFocusIndex = 0;
				}
				else
				{
					BasePanelButtons[BasePanelFocusIndex + 1].GrabFocus();
					BasePanelFocusIndex += 1;
				}
			}
			if (_nodeUserSettingsPanel.Visible)
			{
				int len = UserSettingsButtons.Length;
				if (UserSettingsFocusIndex == len - 1)
				{
					UserSettingsButtons[0].GrabFocus();
					UserSettingsFocusIndex = 0;
				}
				else
				{
					UserSettingsButtons[UserSettingsFocusIndex + 1].GrabFocus();
					UserSettingsFocusIndex += 1;
				}
			}
		}
	}

	private void _on_resume_button_pressed()
	{
		_audioSelect.Play();
		_nodeBasePanel.Hide();
		_nodeUserSettingsPanel.Hide();
		GetTree().Paused = false;
	}


	private void _on_user_settings_button_pressed()
	{
		_audioSelect.Play();
		_nodeBasePanel.Hide();
		_nodeUserSettingsPanel.Show();
		UserSettingsButtons[0].GrabFocus();
		UserSettingsFocusIndex = 0;
	}


	private void _on_main_menu_button_pressed()
	{
		_audioSelect.Play();
		GetTree().Paused = false;
		var nextScene = (PackedScene)ResourceLoader.Load(_SCENE_MAIN_MENU);
		GetTree().ChangeSceneToPacked(nextScene);
	}


	private void _on_user_settings_back_button_pressed()
	{
		_audioSelect.Play();
		_nodeBasePanel.Show();
		_nodeUserSettingsPanel.Hide();
		BasePanelButtons[0].GrabFocus();
		BasePanelFocusIndex = 0;
	}

	private void HandleOpenMenu()
	{
		BasePanelButtons[0].GrabFocus();
		BasePanelFocusIndex = 0;
	}
}
