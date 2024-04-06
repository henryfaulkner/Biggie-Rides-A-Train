extends CollisionShape3D

signal openDoor

var _COMBAT_SCENE_TEST_BATTLE = "res://Pages/CombatScenes/DjBattle/CombatSceneDjBattle.tscn"
var _INTERACT_INPUT = "interact"
var _nodeSelf = null
var _nodeInteractableArea = null

func _ready():
	_nodeSelf = get_node(".")
	_nodeInteractableArea = get_node("./InteractableArea3D")

func _process(delta):
	if _nodeInteractableArea.get_overlapping_bodies().size() > 1 and Input.is_action_just_pressed(_INTERACT_INPUT):
		openDoor.emit()
