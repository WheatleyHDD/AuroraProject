extends Node3D

@export var door: Node3D
var precision = 0.1

func _physics_process(delta: float) -> void:
	var has_cube = false
	for c in $Area3D.get_overlapping_bodies():
		var cube_scale = c.get_node("MeshInstance3D").scale.x
		if cube_scale < scale.x + precision and cube_scale > scale.y - precision:
			has_cube = true
			break
	
	if has_cube: door.send_open_state()
	else: door.send_close_state()
