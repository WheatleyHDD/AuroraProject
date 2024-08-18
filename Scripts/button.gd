extends Node3D

@export var door: Node3D
var precision = 0.15

func _ready() -> void:
	$Top.set_surface_override_material(0, $Top.get_surface_override_material(0).duplicate())

func _physics_process(delta: float) -> void:
	var has_cube = false
	for c in $Area3D.get_overlapping_bodies():
		var cube_scale = c.get_node("MeshInstance3D").scale.x
		if cube_scale < scale.x + precision and cube_scale > scale.y - precision:
			has_cube = true
			break
	
	var mat = $Top.get_surface_override_material(0)
	
	if has_cube:
		$Top.position.y = lerp($Top.position.y, 0.03, delta*60*0.04)
		mat.set(
			"albedo_color",
			lerp(mat.get("albedo_color"), Color.CHARTREUSE, delta*60*0.04)
		)
		door.send_open_state()
	else:
		$Top.position.y = lerp($Top.position.y, 0.096, delta*60*0.04)
		mat.set(
			"albedo_color",
			lerp(mat.get("albedo_color"), Color.CRIMSON, delta*60*0.04)
		)
		door.send_close_state()
