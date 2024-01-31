extends StaticBody2D

const _MAIN_STATION_SCENE = preload("res://Levels/MainStation/LevelMainStation.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func Hit():
	get_tree().change_scene_to_packed(_MAIN_STATION_SCENE);	
	pass;
