extends Node3D

@export_enum("Always", "Pressed", "One-time", "Waiting For") var door_type = 0
@export var waiting_for: Array[Node3D]

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
			$AudioStreamPlayer3D.play()
		if state == 1:
			state_changing = false
			$AudioStreamPlayer3D.play()
			$AnimationPlayer.play("open")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	match door_type:
		0:
			_open_always()
		3:
			_waiting()

func _open_always():
	for c in $Area3D.get_overlapping_bodies():
		if c.is_in_group("player"):
			send_open_state()
			return
	# Закрываем
	send_close_state()

func _waiting():
	for c in waiting_for:
		if not c.value:
			send_close_state()
			return
	# Открываем
	send_open_state()

func send_open_state():
	if state == 1: return
	state = 1
	state_changing = true

func send_close_state():
	if state == 0: return
	state = 0
	state_changing = true
