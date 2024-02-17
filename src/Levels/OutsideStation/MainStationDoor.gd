extends StaticBody2D

const _MAIN_STATION_SCENE = preload("res://Levels/MainStation/LevelMainStation.tscn")
const _RELOCATION_SERVICE_SCRIPT = preload("res://RelocationService/RelocationService.cs")

var node_relocation_service = null

# Called when the node enters the scene tree for the first time.
func _ready():
	node_relocation_service = _RELOCATION_SERVICE_SCRIPT.new()
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func Hit():
	print('Hit')
	#node_relocation_service.SetState_MainStation_MainEntranceDoor()
	get_tree().change_scene_to_packed(_MAIN_STATION_SCENE)
	pass;

# https://gamedev.stackexchange.com/questions/199165/how-to-print-out-the-properties-and-methods-of-a-class-in-gdscript
func dir(class_instance):
	var output = {}
	var methods = []
	for method in class_instance.get_method_list():
		methods.append(method.name)
	
	output["METHODS"] = methods
	
	var properties = []
	for prop in class_instance.get_property_list():
		if prop.type == 3:
			properties.append(prop.name)
	output["PROPERTIES"] = properties

	return output
