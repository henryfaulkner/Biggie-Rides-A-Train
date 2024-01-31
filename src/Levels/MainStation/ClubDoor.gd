extends Area2D

var _INTERACT_INPUT := "interact"
const _CLUB_SCENE = preload("res://Levels/Club/LevelClub.tscn")

var _nodeSelf : Area2D = null

# Called when the node enters the scene tree for the first time.
func _ready():
	_nodeSelf = get_node(".")
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if should_redirect():
		redirect()

func should_redirect() -> bool:
	return _nodeSelf.get_overlapping_bodies().size() > 0 and Input.is_action_just_pressed(_INTERACT_INPUT)

func redirect() -> void:
	get_tree().change_scene_to_packed(_CLUB_SCENE)
