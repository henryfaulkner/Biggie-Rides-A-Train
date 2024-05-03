using Godot;
using System;

public partial class Spore : RigidBody2D
{
	//public static readonly Vector2I _windowSize = new Vector2I(2048, 1024);
	private Vector2 pos;
	private Vector2 vel;
	private Vector2 acc;
	private float angle;
	private int dir;
	private float xOff;
	private float r;

	private Random Rand { get; set; }
	private CollisionShape2D CollisionShape { get; set; }

	private float ManifestGravity { get; set; }
	private Vector2 ManifestWind { get; set; }
	private CombatSingleton _globalCombatSingleton = null;

	public override void _Ready()
	{
		_globalCombatSingleton = GetNode<CombatSingleton>("/root/CombatSingleton");
		// GD.Print($"Position {_globalCombatSingleton.EnemyAttackPanelService.Position.X} {_globalCombatSingleton.EnemyAttackPanelService.Position.Y}");
		// GD.Print($"Size {_globalCombatSingleton.EnemyAttackPanelService.Size.X} {_globalCombatSingleton.EnemyAttackPanelService.Size.Y}");
		Rand = new Random();
		this.vel = Vector2.Zero;
		this.acc = Vector2.Zero;
		this.angle = Mathf.Lerp(0f, Mathf.Pi * 2, (float)GD.Randf());
		this.dir = GD.Randf() < 0.5 ? -1 : 1;
		this.xOff = 0;
		this.r = getRandomSize();

		ManifestGravity = GetRandomGravity();
		ManifestWind = GetRandomWind();
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

	private float GetRandomGravity()
	{
		return GD.Randf();
	}

	private Vector2 GetRandomWind()
	{
		return new Vector2(HelperFunctions.RandfRange(Rand, -10f, 10f), 0);
	}

	public void ApplyForceWrapper(Vector2 force)
	{
		Vector2 f = force * this.r;
		this.acc += f;
		ApplyForce(f);

		//GD.Print($"ApplyForce this.acc {this.acc.X} {this.acc.Y}");
	}

	public void ApplyGravity(float gravityScale)
	{
		GravityScaleValue = gravityScale + this.ManifestGravity;
	}

	public void ApplyWind(float damp, float velocityMultiplier)
	{
		LinearDampValue = damp;
		var vectorX = (this.vel.X * velocityMultiplier) + this.ManifestWind.X;
		var vectorY = this.vel.Y + this.ManifestWind.Y;
		LinearVelocityValue = new Vector2(vectorX, vectorY);
	}

	public void Randomize()
	{
		//GD.Print("Spore: Call Randomize");
		float x = Mathf.Lerp(_globalCombatSingleton.EnemyAttackPanelService.Position.X,
			_globalCombatSingleton.EnemyAttackPanelService.Position.X + _globalCombatSingleton.EnemyAttackPanelService.Size.X,
			(float)GD.Randf());
		float y = Mathf.Lerp(-100f, -10f, (float)GD.Randf());
		this.pos = new Vector2(x, y);
		this.vel = Vector2.Zero;
		this.acc = Vector2.Zero;
		this.r = getRandomSize();
	}

	[Signal]
	public delegate void SporeHitBiggieEventHandler(Spore spore);
	public override void _Process(double _delta)
	{
		if (GetContactCount() > 0 &&
			HelperFunctions.Contains("BiggieMushroomCombat", GetCollidingBodies()))
		{
			GD.Print("Emit Spore Interact");
			EmitSignal(SignalName.SporeHitBiggie, this);
			return;
		}

		this.xOff = Mathf.Sin(this.angle * 2) * 2 * this.r;
		this.vel += this.acc;

		if (this.vel.Length() < 1)
		{
			this.vel = this.vel.Normalized();
		}

		this.pos += this.vel;

		if (this.pos.Y > _globalCombatSingleton.EnemyAttackPanelService.Position.Y + _globalCombatSingleton.EnemyAttackPanelService.Size.Y + this.r)
		{
			Randomize();
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
			ApplyCollision(this.r);
		}
		else
		{
			DrawTextureRect(
				GD.Load<Texture2D>("res://Assets/CombatScenes/BiggieCombat.png"),
				new Rect2(this.pos.X, this.pos.Y, this.r, this.r),
				false,
				new Color(1, 1, 1, 1)
			);
			ContactMonitor = false;
			MaxContactsReported = 0;
		}
	}

	private void ApplyCollision(float radius)
	{
		ContactMonitor = true;
		MaxContactsReported = 5;
		CollisionShape = new CollisionShape2D();
		CircleShape2D shape = new CircleShape2D();
		shape.Radius = radius;
		CollisionShape.Shape = shape;
		AddChild(CollisionShape);
	}
}
