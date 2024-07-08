using System;
using Godot;

public partial class GravityService : Node
{
	public Vector3 CurrentGravity => Force * CurrentGravityNormalizedVector;
	public float Force => 9.8f;
	public Vector3 CurrentGravityNormalizedVector { get; set; }

	private Vector3 DefaultGravityNormalizedVector => new Vector3(0, -1, 0);
	private Vector3 ForwardGravityNormalizedVector => new Vector3(0, 0, -1);

	public GravityService()
	{
		SetDefaultGravity();
	}

	public void SetDefaultGravity()
	{
		CurrentGravityNormalizedVector = DefaultGravityNormalizedVector;
	}

	public void SetForwardGravity()
	{
		CurrentGravityNormalizedVector = ForwardGravityNormalizedVector;
	}
}
