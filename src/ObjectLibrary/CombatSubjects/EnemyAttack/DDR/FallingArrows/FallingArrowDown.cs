using Godot;
using System;

public partial class FallingArrowDown : CharacterBody2D
{
	private static readonly float speed = 150;

	public override void _Ready()
	{
		Velocity = new Vector2(0, -speed);
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
		//////GD.Print("Down Collide");
		if (collision.GetCollider().HasMethod("HandleCollisionDown"))
		{
			collision.GetCollider().Call("HandleCollisionDown");
		}
	}
}
