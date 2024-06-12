using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

/// Service to simplify getting collision info
/// Implementation based on Q_Move's trace.gd (https://github.com/Btan2/Q_Move/tree/main)
/// Hoping to implement collision-based stair physics, detailed in the following article: https://thelowrooms.com/articledir/programming_stepclimbing.php
public partial class TraceBusiness : Node3D
{
	// The end position of a motion or collision check.
	public Vector3 EndPosition { get; set; }
	// The fraction of the distance traveled before a collision occurs.
	public float Fraction { get; set; }
	// The normal vector at the point of collision.
	public Vector3 Normal { get; set; }
	// The type or group of the collider.
	public StringName Type { get; set; }
	// A list of collision groups.
	public List<StringName> Groups { get; set; }
	// A boolean indicating if a collision occurred.
	public bool Hit { get; set; }

	public TraceBusiness()
	{
		EndPosition = Vector3.Zero;
		Fraction = 0.0f;
		Normal = Vector3.Zero;
		Type = string.Empty;
		Groups = new List<StringName>();
		Hit = false;
	}

	/// Performs a motion test from origin to dest with a specified shape, 
	/// excluding a specific object e. It sets the fraction of the distance 
	/// traveled before a collision occurs.
	public void Motion(Vector3 origin, Vector3 dest, Shape3D shape, Rid e)
	{
		PhysicsShapeQueryParameters3D shapeParams;
		PhysicsDirectSpaceState3D spaceState;
		float[] results;

		shapeParams = new PhysicsShapeQueryParameters3D();
		shapeParams.Shape = shape;
		shapeParams.Transform = new Transform3D(Basis.Identity, origin);
		shapeParams.CollideWithBodies = true;
		shapeParams.Exclude = new Godot.Collections.Array<Rid> { e };
		shapeParams.Motion = dest - origin;

		spaceState = GetWorld3D().DirectSpaceState;
		results = spaceState.CastMotion(shapeParams);
		Fraction = results[0];
	}

	/// Checks if a shape is at rest at a given origin position, with a 
	/// collision mask to filter specific types of collisions. 
	/// It sets the hit variable to true if a collision occurs and stores 
	/// the collision normal.
	public void Rest(Vector3 origin, Shape3D shape, Rid e, dynamic mask)
	{
		PhysicsShapeQueryParameters3D shapeParams;
		PhysicsDirectSpaceState3D spaceState;
		Godot.Collections.Dictionary results;

		shapeParams = new PhysicsShapeQueryParameters3D();
		shapeParams.Shape = shape;
		shapeParams.CollisionMask = mask;
		shapeParams.Transform = new Transform3D(Basis.Identity, origin);
		shapeParams.CollideWithBodies = true;
		shapeParams.Exclude = new Godot.Collections.Array<Rid> { e };

		Hit = false;

		spaceState = GetWorld3D().DirectSpaceState;
		results = spaceState.GetRestInfo(shapeParams);

		if (results.Count == 0) return;

		Hit = true;
		Normal = (Vector3)results["normal"];
	}

	/// Finds all collision groups that the shape intersects with at a 
	/// given origin. It updates the groups variable with the collision 
	/// groups and sets hit to true if any collisions are found.
	public void IntersectGroups(Vector3 origin, Shape3D shape, Rid e, dynamic mask)
	{
		PhysicsShapeQueryParameters3D shapeParams;
		PhysicsDirectSpaceState3D spaceState;
		Godot.Collections.Array<Godot.Collections.Dictionary> results;

		Groups = new List<StringName>();

		shapeParams = new PhysicsShapeQueryParameters3D();
		shapeParams.Shape = shape;
		shapeParams.CollisionMask = mask;
		shapeParams.Transform = new Transform3D(Basis.Identity, origin);
		shapeParams.CollideWithBodies = true;
		shapeParams.Exclude = new Godot.Collections.Array<Rid> { e };

		Hit = false;

		spaceState = GetWorld3D().DirectSpaceState;
		results = spaceState.IntersectShape(shapeParams, 8);

		if (results.Count == 0) return;

		Hit = true;

		foreach (var result in results)
		{
			var groups = ((Node)result["collider"]).GetGroups();
			//if (groups.Length > 0) Groups.Add(groups);
			foreach (StringName group in groups)
			{
				Groups.Add(group);
			}
		}
	}

	/// Performs a standard collision check from origin to dest, excluding a 
	/// specific object e. It calculates the fraction of the distance traveled 
	/// before a collision, the end position, and the collision normal.
	public void Standard(Vector3 origin, Vector3 dest, Shape3D shape, Rid e)
	{
		PhysicsShapeQueryParameters3D shapeParams;
		PhysicsDirectSpaceState3D spaceState;
		float[] resultsFloat;
		Godot.Collections.Dictionary resultsDict;

		// Create collision parameters
		shapeParams = new PhysicsShapeQueryParameters3D();
		shapeParams.Shape = shape;
		shapeParams.Transform = new Transform3D(Basis.Identity, origin);
		shapeParams.CollideWithBodies = true;
		shapeParams.Exclude = new Godot.Collections.Array<Rid> { e };
		shapeParams.Motion = dest - origin;

		Hit = false;

		// Get distance fraction and position of first collision
		spaceState = GetWorld3D().DirectSpaceState;

		// Check the transform matrix
		var det = shapeParams.Transform.Basis.Determinant();
		GD.Print($"Transform determinant: {det}");
		GD.Print($"Transform Basis: {shapeParams.Transform.Basis}");
		GD.Print($"Transform Origin: {shapeParams.Transform.Origin}");
		if (det == 0)
		{
			GD.PrintErr("The transform basis is non-invertible.");
			return;
		}

		resultsFloat = spaceState.CastMotion(shapeParams);

		if (resultsFloat.Length > 0)
		{
			Fraction = resultsFloat[0];
			EndPosition = origin + (dest - origin).Normalized()
				* (origin.DistanceTo(dest) * Fraction);
		}
		else
		{
			Fraction = 1;
			EndPosition = dest;
			return; // did not hit anything
		}

		Hit = true;

		// Get collision normal
		resultsDict = spaceState.GetRestInfo(shapeParams);
		if (resultsDict.Count > 0)
		{
			Normal = (Vector3)resultsDict["normal"];
		}
		else
		{
			Normal = Vector3.Up;
		}
	}

	/// Performs a full collision check similar to standard but also retrieves 
	/// the collision group's type if a collision occurs. It updates the type 
	/// variable with the group's type.
	public void Full(Vector3 origin, Vector3 dest, Shape3D shape, Rid e)
	{
		PhysicsShapeQueryParameters3D shapeParams;
		PhysicsDirectSpaceState3D spaceState;
		float[] resultsFloat;
		Godot.Collections.Dictionary resultsDict;
		Godot.Collections.Array<Godot.Collections.Dictionary> resultsDictArray;
		int colId = 0;

		// Create collision parameters
		shapeParams = new PhysicsShapeQueryParameters3D();
		shapeParams.Shape = shape;
		shapeParams.Transform = new Transform3D(Basis.Identity, origin);
		shapeParams.CollideWithBodies = true;
		shapeParams.Exclude = new Godot.Collections.Array<Rid> { e };
		shapeParams.Motion = dest - origin;

		Hit = false;

		// Get distance fraction and position of first collision
		spaceState = GetWorld3D().DirectSpaceState;
		resultsFloat = spaceState.CastMotion(shapeParams);

		if (resultsFloat.Length > 0)
		{
			Fraction = resultsFloat[0];
			EndPosition = origin + (dest - origin).Normalized()
				* (origin.DistanceTo(dest) * Fraction);
		}
		else
		{
			Fraction = 1;
			EndPosition = dest;
			return; // did not hit anything
		}

		Hit = true;

		// Set next parameter position to endpos
		shapeParams.Transform = new Transform3D(Basis.Identity, EndPosition);

		// Get collision normal
		resultsDict = spaceState.GetRestInfo(shapeParams);
		if (resultsDict.Count > 0)
		{
			colId = (int)resultsDict["collider_id"];
			Normal = (Vector3)resultsDict["normal"];
		}
		else
		{
			Normal = Vector3.Up;
		}

		// Get collision group
		if (colId == 0) return;
		resultsDictArray = spaceState.IntersectShape(shapeParams, 8);
		if (resultsDictArray.Count == 0) return;

		foreach (var result in resultsDictArray)
		{
			if ((int)result["collider_id"] == colId)
			{
				var groups = ((Node)result["collider"]).GetGroups();
				if (groups.Count > 0) Type = groups[0];
			}
		}
	}
}
