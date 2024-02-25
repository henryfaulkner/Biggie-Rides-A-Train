using Godot;
using System;

public partial class FallingArrowLeft : CharacterBody2D
{
	private static readonly float speed = 50;

	public override void _Ready()
	{
		Velocity = new Vector2(speed, 0);
	}

	public override void _Process(double delta)
	{
		var collision = Movement(delta);
		if (collision != null)
		{
			Collide(collision);
		}
	}

	public KinematicCollision2D Movement(double delta)
	{
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			Velocity = Velocity.Slide(collision.GetNormal());
		}
		return collision;
	}

	public void Collide(KinematicCollision2D collision)
	{
		GD.Print("Left Collide");
		if (collision.GetCollider().HasMethod("HandleCollisionLeft"))
		{
			collision.GetCollider().Call("HandleCollisionLeft");
		}
	}
}
