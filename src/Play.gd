extends Button

const _OUTSIDE_STATION_SCENE = preload("res://Levels/OutsideStation/LevelOutsideStation.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_pressed():
	get_tree().change_scene_to_packed(_OUTSIDE_STATION_SCENE);	
	pass;
