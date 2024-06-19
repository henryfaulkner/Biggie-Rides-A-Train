using System;
using Godot;

public class BasicEnemyAppearance : IEnemyAppearance
{
	private Sprite2D _nodeSpirite;

	public BasicEnemyAppearance(Sprite2D nodeSprite)
	{
		_nodeSpirite = nodeSprite;
	}

	public void ApplyActiveStyles()
	{
		_nodeSpirite.Modulate = new Color(
			_nodeSpirite.SelfModulate.R,
			_nodeSpirite.SelfModulate.G,
			_nodeSpirite.SelfModulate.B,
			1.0f
		);
	}

	public void ApplyNeutralStyles()
	{
		_nodeSpirite.Modulate = new Color(
			_nodeSpirite.SelfModulate.R,
			_nodeSpirite.SelfModulate.G,
			_nodeSpirite.SelfModulate.B,
			0.75f
		);
	}

	public void ApplyInactiveStyles()
	{
		_nodeSpirite.Modulate = new Color(
			_nodeSpirite.SelfModulate.R,
			_nodeSpirite.SelfModulate.G,
			_nodeSpirite.SelfModulate.B,
			0.5f
		);
	}
}
