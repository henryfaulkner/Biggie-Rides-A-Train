using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class ChatMove : Node2D
{
	private static readonly StringName _INTERACT_INPUT = new StringName("interact");

	[Export]
	public float Speed { get; set; }
	[Export]
	public int NumberOfPoints { get; set; }

	[Export]
	public Path2D _nodePath { get; set; }
	[Export]
	public Line2D _nodeLine { get; set; }
	[Export]
	private PathFollow2D _nodeBiggiePathFollow2D = null;
	[Export]
	private Sprite2D _nodeBiggieSprite = null;
	[Export]
	private PathFollow2D _nodeTargetOnePathFollow2D = null;
	[Export]
	private Sprite2D _nodeTargetOneSprite = null;
	[Export]
	private PathFollow2D _nodeTargetTwoPathFollow2D = null;
	[Export]
	private Sprite2D _nodeTargetTwoSprite = null;
	[Export]
	private PathFollow2D _nodeTargetThreePathFollow2D = null;
	[Export]
	private Sprite2D _nodeTargetThreeSprite = null;

	public Area2D BiggieCollisionArea { get; set; }
	protected List<ChatMoveTarget> TargetList { get; set; }

	public override void _Ready()
	{
		BiggieCollisionArea = ApplyTargetCollision(_nodeBiggiePathFollow2D, _nodeBiggieSprite);
		TargetList = new List<ChatMoveTarget>();

		var curve = new Curve2D();
		var vectorList1 = GetSineVectorList(150, 75);
		// Generate the second sine wave with a phase shift to start where the first one ends
		double phaseShift = 2 * Math.PI;
		var vectorList2 = GetSineVectorList(150, 75, phaseShift);

		// Adjust the x-coordinates of the second sine wave
		for (int i = 0; i < vectorList2.Count; i++)
		{
			vectorList2[i] = new Vector2(vectorList2[i].X + 150, vectorList2[i].Y);
		}

		var concatVectorList = vectorList1.Concat(vectorList2).ToArray();
		foreach (Vector2 vector in concatVectorList)
		{
			curve.AddPoint(vector);
		}
		_nodePath.Curve = curve;
		DrawLineAlongPath(_nodePath, _nodeLine);

		var locations = new List<float>();
		float tempLocation;

		tempLocation = GetRandomTargetLocation(locations);
		_nodeTargetOnePathFollow2D.ProgressRatio = tempLocation;
		locations.Add(tempLocation);
		TargetList.Add(new ChatMoveTarget()
		{
			PathFollow = _nodeTargetOnePathFollow2D,
			Sprite = _nodeTargetOneSprite,
			LocationRatio = tempLocation,
			CollisionArea = ApplyTargetCollision(_nodeTargetOnePathFollow2D, _nodeTargetOneSprite),
			IsMarkedForDeletion = false,
			IsDeleted = false,
		});

		tempLocation = GetRandomTargetLocation(locations);
		_nodeTargetTwoPathFollow2D.ProgressRatio = tempLocation;
		locations.Add(tempLocation);
		TargetList.Add(new ChatMoveTarget()
		{
			PathFollow = _nodeTargetTwoPathFollow2D,
			Sprite = _nodeTargetTwoSprite,
			LocationRatio = tempLocation,
			CollisionArea = ApplyTargetCollision(_nodeTargetTwoPathFollow2D, _nodeTargetTwoSprite),
			IsMarkedForDeletion = false,
			IsDeleted = false,
		});

		tempLocation = GetRandomTargetLocation(locations);
		_nodeTargetThreePathFollow2D.ProgressRatio = tempLocation;
		TargetList.Add(new ChatMoveTarget()
		{
			PathFollow = _nodeTargetThreePathFollow2D,
			Sprite = _nodeTargetThreeSprite,
			LocationRatio = tempLocation,
			CollisionArea = ApplyTargetCollision(_nodeTargetThreePathFollow2D, _nodeTargetThreeSprite),
			IsMarkedForDeletion = false,
			IsDeleted = false,
		});
	}

	public override void _Process(double delta)
	{
		// move biggie
		_nodeBiggiePathFollow2D.Progress += Speed * (float)delta;

		// handle target hitting
		if (Input.IsActionJustReleased(_INTERACT_INPUT))
		{
			foreach (var target in TargetList)
			{
				if (target.IsDeleted || target.IsMarkedForDeletion) continue;
				if (HelperFunctions.ContainsAreas(target.CollisionArea, BiggieCollisionArea.GetOverlappingAreas()))
				{
					EmitSignal(SignalName.ChatTargetHit);
					target.IsMarkedForDeletion = true;
				}
			}
		}

		// handle target deletion
		for (int i = 0; i < TargetList.Count; i++)
		{
			if (TargetList[i].IsMarkedForDeletion)
			{
				ShrinkTargetToDeath(TargetList[i]);
			}
			if (TargetList[i].IsDeleted)
			{
				TargetList[i].QueueFree();
				TargetList.RemoveAt(i);
				i -= 1;
			}
		}
	}

	[Signal]
	public delegate void ChatTargetHitEventHandler();

	private float GetRandomTargetLocation(List<float> otherLocations)
	{
		float result = 0.0f;
		bool firstThird = false;
		bool secondThird = false;
		bool thirdThird = false;
		Random random = new Random();

		foreach (float otherLocation in otherLocations)
		{
			if (otherLocation <= 0.33f)
			{
				firstThird = true;
			}
			else if (otherLocation <= 0.66f)
			{
				secondThird = true;
			}
			else
			{
				thirdThird = true;
			}
		}

		if (!firstThird)
		{
			result = (float)random.NextDouble() * 0.33f;
		}
		else if (!secondThird)
		{
			result = (float)random.NextDouble() * 0.33f + 0.33f;
		}
		else if (!thirdThird)
		{
			result = (float)random.NextDouble() * 0.33f + 0.66f;
		}

		return result;
	}

	private Area2D ApplyTargetCollision(PathFollow2D nodeTarget, Sprite2D sprite)
	{
		var collisionArea = new Area2D();
		var collisionShape = new CollisionShape2D();
		var circle = new CircleShape2D();
		circle.Radius = sprite.Scale.X * sprite.Texture.GetWidth() / 2;
		collisionShape.Shape = circle;
		collisionArea.AddChild(collisionShape);
		nodeTarget.AddChild(collisionArea);
		return collisionArea;
	}


	private List<Vector2> GetSineVectorList(float width, float height, double phaseShift = 0)
	{
		GD.Print($"Call GetSineCurve2D");
		GD.Print($"NumberOfPoints: {NumberOfPoints}");
		var result = new List<Vector2>();
		for (int i = 0; i < NumberOfPoints; i += 1)
		{
			// Normalize the x-coordinate to range from 0 to 2π
			double x = (i / (double)NumberOfPoints) * 2 * Math.PI;
			// Apply phase shift to the x-coordinate
			x += phaseShift;
			// Calculate the y-coordinate based on the sine function
			double y = Math.Sin(x);
			// Scale the x and y coordinates to fit the given width and height
			double xScaled = (x - phaseShift) / (2 * Math.PI) * width;
			double yScaled = (y + 1) / 2 * height;  // shift sine from [-1, 1] to [0, 1]
			GD.Print($"xScaled: {xScaled}, yScaled: {yScaled}");
			result.Add(new Vector2((float)xScaled, (float)yScaled));
		}
		GD.Print($"End GetSineCurve2D");
		return result;
	}

	private void ShrinkTargetToDeath(ChatMoveTarget target)
	{
		target.Sprite.Scale -= new Vector2(0.0005f, 0.0005f);
		if (target.Sprite.Scale.X < 0.0001f) target.IsDeleted = true;
	}

	private void DrawLineAlongPath(Path2D path, Line2D line)
	{
		line.DefaultColor = new Color(1, 1, 1, 1);
		line.Width = 2;
		foreach (Vector2 point in path.Curve.GetBakedPoints())
		{
			line.AddPoint(point + path.Position);
		}
	}

	protected class ChatMoveTarget
	{
		public PathFollow2D PathFollow { get; set; }
		public Sprite2D Sprite { get; set; }
		public float LocationRatio { get; set; }
		public Area2D CollisionArea { get; set; }
		// Is deleted from gameplay (typically will occur first)
		public bool IsMarkedForDeletion { get; set; }
		// Is deleted from screen (hidden)
		public bool IsDeleted { get; set; }

		public void QueueFree()
		{
			PathFollow.QueueFree();
			Sprite.QueueFree();
			CollisionArea.QueueFree();
		}
	}
}
