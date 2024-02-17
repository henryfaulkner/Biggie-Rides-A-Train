extends Area2D

var _INTERACT_INPUT := "interact"
const _MAIN_STATION_SCENE = preload("res://Levels/MainStation/LevelMainStation.tscn")
const _RELOCATION_SERVICE_SCRIPT = preload("res://RelocationService/RelocationService.cs")

var _nodeSelf : Area2D = null
var node_relocation_service = null

# Called when the node enters the scene tree for the first time.
func _ready():
	_nodeSelf = get_node(".")
	node_relocation_service = _RELOCATION_SERVICE_SCRIPT.new()
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if should_redirect():
		redirect()

func should_redirect() -> bool:
	return _nodeSelf.get_overlapping_bodies().size() > 0 and Input.is_action_just_pressed(_INTERACT_INPUT)

func redirect() -> void:
	#node_relocation_service.SetState_MainStation_TherapistOfficeDoor()
	get_tree().change_scene_to_packed(_MAIN_STATION_SCENE)

func Hit():
	print('Hit')
	#node_relocation_service.SetState_MainStation_TherapistOfficeDoor()
	get_tree().change_scene_to_packed(_MAIN_STATION_SCENE)
