extends CollisionShape3D

signal openDoor

var _INTERACT_INPUT = "interact"
var _nodeSelf = null
var _nodeInteractableArea = null

func _ready():
	_nodeSelf = get_node(".")
	_nodeInteractableArea = get_node("..")

func _physics_process(delta):
	var overlapping_bodies = _nodeInteractableArea.get_overlapping_bodies()
	if _contains_biggie(overlapping_bodies) and Input.is_action_just_pressed(_INTERACT_INPUT):
		openDoor.emit()
		
func _contains_biggie(nodes) -> bool:
	for node in nodes:
		if node.name == "Biggie3D":
			return true
	return false
