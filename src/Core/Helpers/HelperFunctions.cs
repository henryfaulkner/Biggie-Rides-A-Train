using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;
using System.Threading;

public static class HelperFunctions
{
	public static bool Contains(Node2D node, Array<Node2D> nodes)
	{
		foreach (var _node in nodes)
		{
			if (_node == node) return true;
		}
		return false;
	}

	public static bool Contains(string name, Array<Node2D> nodes)
	{
		foreach (var node in nodes)
		{
			if (node.Name == name) return true;
		}
		return false;
	}

	public static bool ContainsAreas(Area2D area, Array<Area2D> areas)
	{
		foreach (var _area in areas)
		{
			if (_area == area) return true;
		}
		return false;
	}

	public static bool ContainsBiggie(Array<Node3D> nodes)
	{
		foreach (var node in nodes)
		{
			if (node.Name == "Biggie3D") return true;
		}
		return false;
	}

	public static bool ContainsSpore(Array<Node3D> nodes)
	{
		foreach (var node in nodes)
		{
			if (node.Name == "Spore") return true;
		}
		return false;
	}

	public static void SetTimeout(Action action)
	{
		var cancellationTokenSource = new CancellationTokenSource();
		var cancellationToken = cancellationTokenSource.Token;

		Task.Delay(2000).ContinueWith(async (t) =>
		{
			action();
		}, cancellationToken);
	}

	public static float RandfRange(this Random random, float min = 0.0f, float max = 1.0f)
	{
		return (float)(random.NextDouble() * (max - min) + min);
	}
}
