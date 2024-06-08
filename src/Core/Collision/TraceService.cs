using System;
using Godot;

/// Service to simplify getting collision info
/// Implementation based on Q_Move's trace.gd (https://github.com/Btan2/Q_Move/tree/main)
/// Hoping to implement collision-based stair physics, detailed in the following article: https://thelowrooms.com/articledir/programming_stepclimbing.php
public partial class TraceService : Node3D
{
    // The end position of a motion or collision check.
    public Vector3 EndPosition { get; set; }
    // The fraction of the distance traveled before a collision occurs.
    public float Fraction { get; set; }
    // The normal vector at the point of collision.
    public Vector3 Normal { get; set; }
    // The type or group of the collider.
    public string Type { get; set; }
    // A list of collision groups.
    public string[] Groups { get; set; }
    // A boolean indicating if a collision occurred.
    public bool Hit { get; set; }

    public TraceService()
    {
        EndPosition = Vector3.Zero;
        Fraction = 0.0f;
        Normal = Vector3.Zero;
        Type = string.Empty;
        Groups = new string[0];
        Hit = false;
    }

    /// Performs a motion test from origin to dest with a specified shape, 
    /// excluding a specific object e. It sets the fraction of the distance 
    /// traveled before a collision occurs.
    public void Motion(Vector3 origin, Vector3 dest, Shape shape, dynamic e)
    {
        PhysicsShapeQueryParameters3D params;
        PhysicsDirectSpaceState3D spaceState;
        float[] results;

		params = new PhysicsShapeQueryParameters3D();
		params.SetShape(shape);
		params.Transform.Origin = origin;
		params.CollideWithBodies = true;
		params.Exclude = new dynamic[] { e };

        spaceState = GetWorld().DirectSpaceState;
        results = spaceState.CastMotion (params, dest -origin);
        Fraction = results[0];
    }

    /// Checks if a shape is at rest at a given origin position, with a 
    /// collision mask to filter specific types of collisions. 
    /// It sets the hit variable to true if a collision occurs and stores 
    /// the collision normal.
    public void Rest(Vector3 origin, Shape shape, dynamic e, dynamic mask)
    {
        PhysicsShapeQueryParameters3D params;
        PhysicsDirectSpaceState3D spaceState;
        Godot.Collections.Dictionary results;

		params = new PhysicsShapeQueryParameters3D();
		params.SetShape(shape);
		params.SetCollisionMask(mask); 
		params.Transform.Origin = origin;
		params.CollideWithBodies = true;
		params.Exclude = [e];

        Hit = false;

        spaceState = GetWorld().DirectSpaceState;
        results = spaceState.GetRestInfo (params);

        if (results.IsEmpty()) return;

        Hit = true;
        Normal = results.Get("normal");
    }

    /// Finds all collision groups that the shape intersects with at a 
    /// given origin. It updates the groups variable with the collision 
    /// groups and sets hit to true if any collisions are found.
    public void IntersectGroups(Vector3 origin, Shape shape, dynamic e, dynamic mask)
    {
        PhysicsShapeQueryParameters3D params;
        PhysicsDirectSpaceState3D spaceState;
        Godot.Collections.Dictionary[] results;

        Groups = new string[];
		
		params = new PhysicsShapeQueryParameters3D();
		params.SetShape(shape);
		params.SetCollisionMask(mask); 
		params.Transform.Origin = origin;
		params.CollideWithBodies = true;
		params.Exclude = [e];

        Hit = false;

        spaceState = GetWorld().DirectSpaceState;
        results = spaceState.IntersectShape (params, 8);

        if (results.IsEmpty()) return;

        Hit = true;

        foreach (var result in results)
        {
            var group = result.Get("collider").GetGroups();
            if (group.Length > 0) Groups = Groups.Concat(groups).ToArray();
        }
    }

    /// Performs a standard collision check from origin to dest, excluding a 
    /// specific object e. It calculates the fraction of the distance traveled 
    /// before a collision, the end position, and the collision normal.
    public void Standard(Vector3 origin, Vector3 dest, Shape shape, dynamic e)
    {
        PhysicsShapeQueryParameters3D params;
        PhysicsDirectSpaceState3D spaceState;
        float[] resultsFloat;
        Godot.Collections.Dictionary resultsDict;

		// Create collision parameters
		params = new PhysicsShapeQueryParameters3D();
		params.SetShape(shape);
		params.Transform.Origin = origin;
		params.CollideWithBodies = true;
		params.Exclude = [e];

        Hit = false;

        // Get distance fraction and position of first collision
        spaceState = GetWorld().DirectSpaceState;
        resultsFloat = spaceState.CastMotion (params, dest -origin);

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
		params.Transform.Origin = EndPosition;

        // Get collision normal
        resultsDict = spaceState.GetRestInfo (params);
        if (!resultsDict.IsEmpty())
        {
            Normal = resultsDict.Get("normal");
        }
        else
        {
            Normal = Vector3.Up;
        }
    }

    /// Performs a full collision check similar to standard but also retrieves 
    /// the collision group's type if a collision occurs. It updates the type 
    /// variable with the group's type.
    public void Full(Vector3 origin, Vector3 dest, Shape shape, dynamic e)
    {
        PhysicsShapeQueryParameters3D params;
        PhysicsDirectSpaceState3D spaceState;
        float[] resultsFloat;
        Godot.Collections.Dictionary resultsDict;
        Godot.Collections.Dictionary resultsDictArray;
        int colId = 0;

		// Create collision parameters
		params = new PhysicsShapeQueryParameters3D();
		params.SetShape(shape);
		params.Transform.Origin = origin;
		params.CollideWithBodies = true;
		params.Exclude = [e];

        Hit = false;

        // Get distance fraction and position of first collision
        spaceState = GetWorld().DirectSpaceState;
        resultsFloat = spaceState.CastMotion (params, dest -origin);

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
		params.Transform.Origin = EndPosition;

        // Get collision normal
        resultsDict = spaceState.GetRestInfo (params);
        if (!resultsDict.IsEmpty())
        {
            colId = resultsDict.Get("collider_id");
            Normal = resultsDict.Get("normal");
        }
        else
        {
            Normal = Vector3.Up;
        }

        // Get collision group
        if (colId == 0) return;
        resultsDictArray = spaceState.IntersectShape (params, 8);
        if (resultsDictArray.IsEmpty()) return;

        foreach (var result in resultsDictArray)
        {
            if (result.Get("collider_id") == colId)
            {
                var groups = result.Get("collider").GetGroups();
                if (groups.Length > 0) Type = groups[0];
            }
        }
    }
}
