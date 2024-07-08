using Godot;
using System;

public class DoorAnimationHelper
{
	private static readonly double _TIME_BETWEEN_FRAMES = 20;

	public DoorAnimationHelper()
	{
		SpriteMeshInstance = null;
		FrameArray = new int[0] { };
		StartDelta = null;
		CurrentFrameIndex = 0;
	}

	public DoorAnimationHelper(Node spriteMeshInstance, int[] frameArray)
	{
		SpriteMeshInstance = spriteMeshInstance;
		FrameArray = frameArray;
		StartDelta = null;
		CurrentFrameIndex = 0;
	}

	public Node SpriteMeshInstance { get; set; }
	public int[] FrameArray { get; set; }
	public double? StartDelta { get; set; }
	public int CurrentFrameIndex { get; set; }

	// return true once final frame is played
	public bool AnimateOpen(double delta)
	{
		bool finished = false;
		if (StartDelta == null) StartDelta = delta;

		double elapsedTime = delta - (StartDelta ?? delta);
		int lastFrameIndex = CurrentFrameIndex;
		CurrentFrameIndex = (int)Math.Floor(elapsedTime / _TIME_BETWEEN_FRAMES);
		if (CurrentFrameIndex >= FrameArray.Length) finished = true;
		else if (CurrentFrameIndex != lastFrameIndex) SpriteMeshInstance.Call("set_frame", CurrentFrameIndex);

		if (finished)
		{
			StartDelta = null;
			return true;
		}
		return false;
	}
}
