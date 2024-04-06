extends Node3D

var _INTERACT_INPUT := "interact"
const _CALLING_DOOR_NODE_PATH = "./InteractableArea3D" 
const _CALLING_DOOR_COLLISION_NODE_PATH = "./InteractableArea3D/CollisionShape3D" 
const _TARGET_SCENE = preload ("res://Pages/Levels/MainStation/LevelMainStation.tscn")
const _TARGET_SCENE_DOOR_NODE_PATH = "."
const _RELOCATION_SERVICE_SCRIPT = preload ("res://Core/RelocationService/RelocationService.cs")

var _nodeSelf: Node3D = null
var _nodeCallingDoor: Area3D = null
var _nodeCallingDoorCollision: CollisionShape3D = null
var _nodeTargetDoor: Node = null
var _relocation_service = null

# Called when the node enters the scene tree for the first time.
func _ready():
	_nodeSelf = get_node(".")
	_nodeCallingDoor = get_node(_CALLING_DOOR_NODE_PATH)
	_nodeCallingDoorCollision = get_node(_CALLING_DOOR_COLLISION_NODE_PATH)
	var sceneInstance = _TARGET_SCENE.instantiate()
	_nodeTargetDoor = sceneInstance.get_node(_TARGET_SCENE_DOOR_NODE_PATH)
	_relocation_service = _RELOCATION_SERVICE_SCRIPT.new()
	_nodeCallingDoorCollision.openDoor.connect(redirect)
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if should_redirect():
		redirect()

func should_redirect() -> bool:
	return _nodeCallingDoor.get_overlapping_bodies().size() > 1 and Input.is_action_just_pressed(_INTERACT_INPUT)

func redirect() -> void:
	print("Cum")
	#_relocation_service.SetState_MainStation(_nodeDoor.position.x, _nodeDoor.position.y)
	#get_tree().change_scene_to_packed(_SCENE)
