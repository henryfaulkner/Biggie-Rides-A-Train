using Godot;
using System;

public static class LerpOvershootHelper
{
	// https://www.reddit.com/r/godot/comments/rk6hlz/how_did_you_solve_2d_camera_smoothing/
	public static float LerpOvershoot(float origin, float target, float weight, float overshoot)
	{
		var distance = (target - origin) * weight;
		if (IsEqualApprox(distance, 0.0f))
		{
			return target;
		}

		var distanceSign = Mathf.Sign(distance);
		var lerpValue = Mathf.Lerp(origin, target + (overshoot * distanceSign), weight);
		if (distanceSign == 1.0)
		{
			lerpValue = Mathf.Min(lerpValue, target);
		}
		else if (distanceSign == -1.0)
		{
			lerpValue = Mathf.Max(lerpValue, target);
		}
		return lerpValue;
	}

	public static Vector3 LerpOvershootV(Vector3 from, Vector3 to, float weight, Vector3 overshoot)
	{
		var x = LerpOvershoot(from.X, to.X, weight, overshoot.X);
		var y = LerpOvershoot(from.Y, to.Y, weight, overshoot.Y);
		var z = LerpOvershoot(from.Z, to.Z, weight, overshoot.Z);
		return new Vector3(x, y, z);
	}

	public static bool IsEqualApprox(float a, float b)
	{
		return a > b - float.Epsilon && a < b + float.Epsilon;
	}
}
