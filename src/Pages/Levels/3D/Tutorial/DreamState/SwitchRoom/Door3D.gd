extends Node3D

const _SCENE = preload ("res://Pages/Levels/3D/Tutorial/DreamState/ButtonRoom/Scene_ButtonRoom.tscn")

var _nodeBarrier = null

func _ready():
		var sceneInstance = _SCENE.instantiate()
		var collision = get_node("./InteractableArea3D/CollisionShape3D")
		_nodeBarrier = get_node("../Barrier") 
		collision.openDoor.connect(navigate)
		

func navigate():
	if (_nodeBarrier.IsOpen):
		get_tree().change_scene_to_packed(_SCENE)
