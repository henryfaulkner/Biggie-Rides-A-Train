using Godot;
using System;

public partial class AquariumGiftShop : Node3D
{
    private static readonly int _FPS = 60;
    private static readonly StringName _INTERACT_INPUT = new StringName("interact");

    private Node3D _nodeAquariumDoor = null;
    private Area3D _nodeInteractableArea = null;
    private Node _nodeOutsideGiftShopDoor = null;

    private int FrameIndex { get; set; }

    public override void _Ready()
    {
        _nodeAquariumDoor = GetNode<Node3D>("./OuterAquarium/AquariumDoor");
        _nodeOutsideGiftShopDoor = GetNode<Node>("./OuterAquarium/OutsideGiftShopDoor");
        _nodeInteractableArea = GetNode<Area3D>("./OuterAquarium/AquariumDoor/InteractableArea3D");
    }

    public override void _PhysicsProcess(double _delta)
    {
        if (Input.IsActionJustPressed(_INTERACT_INPUT)
            && HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
        {
            if (ProcessOpenDoor()) NavigateToGiftShop();
        }
    }

    //
    private bool ProcessOpenDoor()
    {
        // Negative y rotation to open door
        if (FrameIndex % 30 == 0)
        {
            _nodeAquariumDoor.Rotation = new Vector3(
                _nodeAquariumDoor.Rotation.X,
                _nodeAquariumDoor.Rotation.Y - 15,
                _nodeAquariumDoor.Rotation.Z
            );
        }

        if (FrameIndex % 120 == 0)
        {
            NavigateToGiftShop();
        }

        FrameIndex += 1;
        return false;
    }

    private void NavigateToGiftShop()
    {
        _nodeOutsideGiftShopDoor.Call("redirect");
    }
}
