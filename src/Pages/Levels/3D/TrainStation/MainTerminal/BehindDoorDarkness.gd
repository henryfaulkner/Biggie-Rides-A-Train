extends MeshInstance3D

const _SCENE = preload ("res://Pages/Levels/3D/TrainStation/Aquarium/Scene_GiftShop.tscn")
const _SCENE_DOOR_NODE_PATH = "./LevelWrapper/TextBoxWrapper/AquariumGiftShop/OuterAquarium/AquariumDoor"
const _RELOCATION_SERVICE_SCRIPT = preload ("res://Core/Services/RelocationService/RelocationService.cs")

var _nodeDoor: MeshInstance3D = null
var _relocation_service = null

func _ready():
	var sceneInstance = _SCENE.instantiate()
	_nodeDoor = sceneInstance.get_node(_SCENE_DOOR_NODE_PATH)
	_relocation_service = _RELOCATION_SERVICE_SCRIPT.new()

func redirect() -> void:
	_relocation_service.SetLocation(_nodeDoor.position.x, _nodeDoor.position.y, _nodeDoor.position.z)
	get_tree().change_scene_to_packed(_SCENE)
