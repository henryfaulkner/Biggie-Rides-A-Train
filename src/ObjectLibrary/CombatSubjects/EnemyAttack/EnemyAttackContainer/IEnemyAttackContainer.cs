using Godot;

public abstract partial class EnemyAttackContainer : Node2D
{
    /// <summary>
    /// Set some variable indicating this Attack Container should be Active
    /// </summary>
    public abstract void StartTurn();
    /// <summary>
    /// Do the active work of the Attack Container
    /// </summary>
    public abstract void ProcessTurn();
    /// <summary>
    /// Set some variable indicating this Attack Container should be Inactive
    /// </summary>
    public abstract void EndTurn();
}