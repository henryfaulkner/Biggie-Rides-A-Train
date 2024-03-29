extends Button

const _OUTSIDE_STATION_SCENE = preload ("res://Pages/Levels/OutsideStation/LevelOutsideStation.tscn")
const _MAIN_STATION_SCENE = preload ("res://Pages/Levels/MainStation/LevelMainStation.tscn")
const _THERAPIST_OFFICE_SCENE = preload ("res://Pages/Levels/TherapistOffice/LevelTherapistOffice.tscn")
const _CLUB_SCENE = preload ("res://Pages/Levels/Club/LevelClub.tscn")
const _TEST_PERSPECTIVE_SCENE = preload ("res://Pages/Levels/TestPerspective/LevelTestPerspective.tscn")

const _RELOCATION_SERVICE_SCRIPT = preload ("res://Core/RelocationService/RelocationService.cs")
var _relocation_service = null

# Called when the node enters the scene tree for the first time.
func _ready():
	_relocation_service = _RELOCATION_SERVICE_SCRIPT.new()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _on_pressed():
	var scenePlay = decide_scene()
	scenePlay = _TEST_PERSPECTIVE_SCENE
	get_tree().change_scene_to_packed(scenePlay);
	pass ;

func decide_scene():
	var storedLocationSceneId = _relocation_service.GetStoredLocationSceneId()
	var result = _OUTSIDE_STATION_SCENE;
	match storedLocationSceneId:
		0:
			result = _OUTSIDE_STATION_SCENE
		1:
			result = _OUTSIDE_STATION_SCENE
		2:
			result = _MAIN_STATION_SCENE
		3:
			result = _THERAPIST_OFFICE_SCENE
		4:
			result = _CLUB_SCENE
		_:
			result = _OUTSIDE_STATION_SCENE
	return result
