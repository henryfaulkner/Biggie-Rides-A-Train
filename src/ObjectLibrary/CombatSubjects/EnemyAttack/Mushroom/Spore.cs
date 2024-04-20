using Godot;
using System;

public partial class Spore : RigidBody2D
{
	public static readonly Vector2I _windowSize = new Vector2I(2048, 1024);
	private Vector2 pos;
	private Vector2 vel;
	private Vector2 acc;
	private float angle;
	private int dir;
	private float xOff;
	private float r;

	public override void _Ready()
	{
		float x = Mathf.Lerp(0f, _windowSize.X, (float)GD.Randf());
		float y = Mathf.Lerp(-100f, -10f, (float)GD.Randf());
		this.pos = new Vector2(x, y);
		this.vel = Vector2.Zero;
		this.acc = Vector2.Zero;
		this.angle = Mathf.Lerp(0f, Mathf.Pi * 2, (float)GD.Randf());
		this.dir = GD.Randf() < 0.5 ? -1 : 1;
		this.xOff = 0;
		this.r = getRandomSize();
	}

	private float GravityScaleValue { get; set; }
	private float LinearDampValue { get; set; }
	private Vector2 LinearVelocityValue { get; set; }
	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		GravityScale = GravityScaleValue;
		LinearDamp = LinearDampValue;
		LinearVelocity = this.vel;
	}

	private float getRandomSize()
	{
		float r = Mathf.Pow((float)GD.Randf(), 3);
		return Mathf.Clamp(r * 32f, 2f, 32f);
	}

	public void ApplyForce(Vector2 force, int forceType)
	{
		Vector2 f = force * this.r;
		this.acc += f;
		ApplyForce(f);
	}

	public void ApplyGravity(float gravityScale)
	{
		GravityScaleValue = gravityScale;
	}

	public void ApplyWind(float damp, float velocityMultiplier)
	{
		LinearDampValue = damp;
		LinearVelocityValue = new Vector2(this.vel.X * velocityMultiplier, this.vel.Y * velocityMultiplier);
	}

	public void Randomize()
	{
		GD.Print("Spore: Call Randomize");
		float x = Mathf.Lerp(0f, _windowSize.X, (float)GD.Randf());
		float y = Mathf.Lerp(-100f, -10f, (float)GD.Randf());
		this.pos = new Vector2(x, y);
		this.vel = Vector2.Zero;
		this.acc = Vector2.Zero;
		this.r = getRandomSize();
	}

	public override void _Process(double _delta)
	{
		this.xOff = Mathf.Sin(this.angle * 2) * 2 * this.r;

		this.vel += this.acc;
		this.vel = this.vel.LimitLength(this.r * 0.2f);

		if (this.vel.Length() < 1)
		{
			this.vel = this.vel.Normalized();
		}

		this.pos += this.vel;

		if (this.pos.Y > _windowSize.Y + this.r)
		{
			Randomize();
		}

		// Wrapping Left and Right
		if (this.pos.X < -this.r)
		{
			this.pos.X = _windowSize.X + this.r;
		}
		if (this.pos.X > _windowSize.X + this.r)
		{
			this.pos.X = -this.r;
		}

		this.angle += this.dir * this.vel.Length() / 200;
	}

	public override void _Draw()
	{
		if (this.r > 16)
		{
			DrawTextureRect(
				GD.Load<Texture2D>("res://Assets/CombatScenes/BiggieCombat.png"),
				new Rect2(this.pos.X, this.pos.Y, this.r, this.r),
				false,
				new Color(1, 0, 0, 1)
			);
		}
		else
		{
			DrawTextureRect(
				GD.Load<Texture2D>("res://Assets/CombatScenes/BiggieCombat.png"),
				new Rect2(this.pos.X, this.pos.Y, this.r, this.r),
				false,
				new Color(0, 0, 1, 1)
			);
		}
	}
}
