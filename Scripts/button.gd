extends Node3D

@export var door: Node3D
var precision = 0.15

var activated_cube: RigidBody3D

func _ready() -> void:
	$Top.set_surface_override_material(0, $Top.get_surface_override_material(0).duplicate())

func _process(delta: float) -> void:
	var mat = $Top.get_surface_override_material(0)
	
	if activated_cube:
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


func _on_area_3d_body_entered(body: Node3D) -> void:
	if body is RigidBody3D and body.is_in_group("cube"):
		var cube_scale = body.get_node("MeshInstance3D").scale.x
		if cube_scale < scale.x + precision and cube_scale > scale.y - precision:
			activated_cube = body
			activated_cube.rotation = Vector3.ZERO
			activated_cube.global_position = $Marker3D.global_position
			activated_cube.freeze = true
