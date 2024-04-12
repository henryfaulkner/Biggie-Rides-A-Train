using Godot;
using Godot.Collections;
using System;

public static class HelperFunctions
{
    public static bool ContainsBiggie(Array<Node3D> nodes)
    {
        foreach (var node in nodes)
        {
            if (node.Name == "Biggie3D") return true;
        }
        return false;
    }
}