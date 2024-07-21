extends Node3D

var _INTERACT_INPUT := "interact"
const _CALLING_DOOR_NODE_PATH = "./InteractableArea3D"
const _CALLING_DOOR_COLLISION_NODE_PATH = "./InteractableArea3D/CollisionShape3D"
const _TARGET_SCENE = preload ("res://Pages/Levels/3D/Tutorial/DreamState/DreamRoom1/Scene_Dream_Room1.tscn")
const _TARGET_SCENE_DOOR_NODE_PATH = "."
const _RELOCATION_SERVICE_SCRIPT = preload ("res://Core/Services/RelocationService/RelocationService.cs")

var _nodeSelf: Node3D = null
var _nodeCallingDoor: Area3D = null
var _nodeCallingDoorCollision: CollisionShape3D = null
var _nodeTargetDoor: Node = null
var _relocation_service = null
var _door_animation_helper = null
var _is_redirecting = false
var start_redirecting = func():
	_is_redirecting = true

# Called when the node enters the scene tree for the first time.
func _ready():
	_nodeSelf = get_node(".")
	_nodeCallingDoor = get_node(_CALLING_DOOR_NODE_PATH)
	_nodeCallingDoorCollision = get_node(_CALLING_DOOR_COLLISION_NODE_PATH)
	var sceneInstance = _TARGET_SCENE.instantiate()
	_nodeTargetDoor = sceneInstance.get_node(_TARGET_SCENE_DOOR_NODE_PATH)
	_relocation_service = _RELOCATION_SERVICE_SCRIPT.new()
	_nodeCallingDoorCollision.openDoor.connect(start_redirecting)

func _process(delta):
	if (_is_redirecting):
		redirect()
	
func redirect() -> void:
	print("Door Redirect")
	_relocation_service.SetState_MainStation(_nodeTargetDoor.position.x, _nodeTargetDoor.position.y)
	get_tree().change_scene_to_packed(_TARGET_SCENE)
