extends Node

var enums = load("res://Core/Enumerations.gd")

func _ready():
	pass

func GetLevelEnums():
	return enums.Levels
	
func GetRotationEnums():
	return enums.Rotations
