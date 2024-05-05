using Godot;
using Godot.Collections;
using System;

public class HoverTextBoxHelper
{
	private Node3D _nodeCaller = null;
	private Area3D _nodeInteractableArea = null;
	private HoverTextBox _nodeHoverTextBox = null;
	public bool RecentlyOpen { get; set; }
	private string Text { get; set; }

	public HoverTextBoxHelper() { }
	public HoverTextBoxHelper(
		Node3D nodeCaller,
		Area3D nodeInteractableArea,
		HoverTextBox nodeHoverTextBox,
		string text)
	{
		_nodeCaller = nodeCaller;
		_nodeInteractableArea = nodeInteractableArea;
		_nodeHoverTextBox = nodeHoverTextBox;
		RecentlyOpen = false;
		SetText(text);
	}

	public void SetText(string text)
	{
		Text = text;
	}

	public void Process()
	{
		if (HelperFunctions.ContainsBiggie(_nodeInteractableArea.GetOverlappingBodies()))
		{
			HandleInteractableAreaHover();
			RecentlyOpen = true;
		}
		else if (RecentlyOpen)
		{
			HandleInteractableAreaLeave();
			RecentlyOpen = false;
		}
	}

	public void ForceClose()
	{
		HandleInteractableAreaLeave();
	}

	private void HandleInteractableAreaHover()
	{
		_nodeHoverTextBox.SetText(Text);
		_nodeHoverTextBox.ShowTextBox();
	}

	private void HandleInteractableAreaLeave()
	{
		_nodeHoverTextBox.HideTextBox();
		_nodeHoverTextBox.ClearText();
	}
}
