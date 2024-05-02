extends Node3D

const _SCENE = preload ("res://Pages/Levels/3D/Tutorial/DreamState/MushroomFight/Scene_MushroomFight.tscn")
const _SCENE_DOOR_NODE_PATH = "./LevelWrapper/TextBoxWrapper/NearDoor3D"
var RelocationService = null
var LevelEnumService = null
var _nodeDoor: Node3D = null

var _nodeBarrier = null

func _ready():
	RelocationService = get_node("/root/RelocationService")
	LevelEnumService = get_node("/root/LevelEnumService")

	var sceneInstance = _SCENE.instantiate()
	_nodeDoor = sceneInstance.get_node(_SCENE_DOOR_NODE_PATH)
	_nodeBarrier = get_node("../Barrier")
	var collision = get_node("./InteractableArea3D/CollisionShape3D")
	collision.openDoor.connect(navigate)

func navigate():
	RelocationService.SetLocation(LevelEnumService.GetLevelEnums().MushroomFightRoom, _nodeDoor.position.x, _nodeDoor.position.y, _nodeDoor.position.z)
	#if (_nodeBarrier.IsOpen):
	get_tree().change_scene_to_packed(_SCENE)
