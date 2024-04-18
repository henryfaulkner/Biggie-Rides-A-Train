using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;
using System.Threading;

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

    public static void SetTimeout(Action action)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        Task.Delay(2000).ContinueWith(async (t) =>
        {
            action();
        }, cancellationToken);
    }
}