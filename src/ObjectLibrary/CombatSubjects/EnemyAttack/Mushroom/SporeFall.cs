using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public partial class SporeFall : Node2D
{
	private static readonly float _GRAVITY_MULTIPLIER = 3f;
	private static readonly float _WIND_MULTIPLIER = 4f;

	private List<Spore> _nodeSporeList = new List<Spore>();
	private Vector2 gravity;
	private float zOff = 0;

	private Texture spritesheet;
	private List<Texture> textures = new List<Texture>();
	private int FrameIndex { get; set; }

	private CombatSingleton _globalCombatSingleton = null;

	public override void _Ready()
	{
		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");

		gravity = new Vector2(0, 0.3f * _GRAVITY_MULTIPLIER);
		FrameIndex = 0;

		for (int i = 0; i < 400; i++)
		{
			float x = (float)GD.RandRange(_globalCombatSingleton.EnemyAttackPanelService.Position.X,
				_globalCombatSingleton.EnemyAttackPanelService.Position.X + _globalCombatSingleton.EnemyAttackPanelService.Size.X);
			float y = (float)GD.RandRange(0, GetViewportRect().Size.Y);
			Spore spore = new Spore()
			{
				Position = new Vector2(x, y)
			};
			AddChild(spore);
			_nodeSporeList.Add(spore);
		}
	}

	public override void _Process(double _delta)
	{
		zOff += 0.1f;

		foreach (Spore spore in _nodeSporeList)
		{
			try
			{
				// float xOff = spore.Position.X / GetViewportRect().Size.X;
				// float yOff = spore.Position.Y / GetViewportRect().Size.Y;
				float xOff = spore.Position.X / _globalCombatSingleton.EnemyAttackPanelService.Size.X;
				float yOff = spore.Position.Y / _globalCombatSingleton.EnemyAttackPanelService.Size.Y;
				float wAngle = GetFastNoiseLite().GetNoise2D(xOff, yOff) * Mathf.Tau;

				int windSign = (FrameIndex / 120) % 2 == 0 ? 1 : -1;
				Vector2 wind = new Vector2(Mathf.Cos(wAngle) * _WIND_MULTIPLIER, Mathf.Sin(wAngle)) * 0.1f * windSign;

				spore.ApplyForceWrapper(gravity);
				spore.ApplyGravity(1f);
				spore.ApplyForceWrapper(wind);
				spore.ApplyWind(0.3f, wind.X * 10);
				//spore.Update();
			}
			catch (Exception ex)
			{
				GD.Print($"SporeFall exception: {ex.Message}");
			}
		}

		FrameIndex += 1;
	}

	private FastNoiseLite GetFastNoiseLite()
	{
		return new FastNoiseLite()
		{
			NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
		};
	}
}
