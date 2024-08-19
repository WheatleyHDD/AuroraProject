extends Node3D

@export var door: Node3D

func _on_area_3d_body_entered(body: Node3D) -> void:
	if body is RigidBody3D:
		var rb: RigidBody3D = body
		if door:
			door.send_open_state()
