extends Node3D

const _SCENE = preload ("res://Pages/Levels/3D/Tutorial/DreamState/MushroomFight/Scene_MushroomFight.tscn")

func _ready():
		var sceneInstance = _SCENE.instantiate()
		var collision = get_node("./InteractableArea3D/CollisionShape3D")
		collision.openDoor.connect(navigate)
		

func navigate():
	get_tree().change_scene_to_packed(_SCENE)
