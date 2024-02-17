extends Area2D

var _INTERACT_INPUT := "interact"
const _THERAPIST_OFFICE_SCENE = preload("res://Levels/TherapistOffice/LevelTherapistOffice.tscn")
const _SCENE_DOOR_NODE_PATH = "./MainStationDoor"
const _RELOCATION_SERVICE_SCRIPT = preload("res://RelocationService/RelocationService.cs")

var _nodeSelf : Area2D = null
var _nodeDoor : Area2D = null
var node_relocation_service = null

# Called when the node enters the scene tree for the first time.
func _ready():
	_nodeSelf = get_node(".")
	var nextSceneInstance = _THERAPIST_OFFICE_SCENE.instantiate()
	_nodeDoor = nextSceneInstance.get_node(_SCENE_DOOR_NODE_PATH)
	node_relocation_service = _RELOCATION_SERVICE_SCRIPT.new()
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if should_redirect():
		redirect()

func should_redirect() -> bool:
	return _nodeSelf.get_overlapping_bodies().size() > 0 and Input.is_action_just_pressed(_INTERACT_INPUT)

func redirect() -> void:
	#node_relocation_service.SetState_TherapistOffice_MainStationDoor()
	node_relocation_service.SetState_StoredLocation(_nodeDoor.position.x, _nodeDoor.position.y)
	get_tree().change_scene_to_packed(_THERAPIST_OFFICE_SCENE)

func Hit():
	print('Hit')
	#node_relocation_service.SetState_TherapistOffice_MainStationDoor()
	node_relocation_service.SetState_StoredLocation(_nodeDoor.position.x, _nodeDoor.position.y)
	get_tree().change_scene_to_packed(_THERAPIST_OFFICE_SCENE)
