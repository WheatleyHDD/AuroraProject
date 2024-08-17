extends Node3D

@export_enum("Always", "Pressed", "One-time") var door_type = 0
var player

var state: int = 0
var state_changing = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


func _process(delta: float) -> void:
	if state_changing:
		if state == 0:
			state_changing = false
			$AnimationPlayer.play("close")
		if state == 1:
			state_changing = false
			$AnimationPlayer.play("open")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	match door_type:
		0:
			_open_always()


func _open_always():
	for c in $Area3D.get_overlapping_bodies():
		if c.is_in_group("player"):
			if state == 0:
				player = c
				state = 1
				state_changing = true
			return
	# Закрываем
	if state == 1:
		player = null
		state = 0
		state_changing = true
