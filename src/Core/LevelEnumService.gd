extends Node

var level_enums = load("res://Core/LevelEnumerations.gd")

func _ready():
	pass

func GetLevelEnums():
	return level_enums.LevelEnumerations
