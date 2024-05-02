extends Node3D

const _SCENE = preload ("res://Pages/Levels/3D/Tutorial/DreamState/DreamRoom1/Scene_Dream_Room1.tscn")
const _SCENE_DOOR_NODE_PATH = "./LevelWrapper/TextBoxWrapper/Door3D"
var RelocationService = null
var LevelEnumService = null
var _nodeDoor: Node3D = null

func _ready():
	RelocationService = get_node("/root/RelocationService")
	LevelEnumService = get_node("/root/LevelEnumService")

	var sceneInstance = _SCENE.instantiate()
	_nodeDoor = sceneInstance.get_node(_SCENE_DOOR_NODE_PATH)
	var collision = get_node("./InteractableArea3D/CollisionShape3D")
	collision.openDoor.connect(navigate)

func navigate():
	RelocationService.SetLocation(LevelEnumService.GetLevelEnums().DreamRoomOne, _nodeDoor.position.x, _nodeDoor.position.y, _nodeDoor.position.z)
	get_tree().change_scene_to_packed(_SCENE)
