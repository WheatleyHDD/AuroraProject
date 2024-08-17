extends Label

@export var player: Node3D
@export var cube_creator: Node3D

func _process(delta: float) -> void:
	var info = ""
	if cube_creator.CubeCreating:
		info = "    + Rotation: " + str(cube_creator.CubeCreating.rotation_degrees) + "\n" +\
			   "    + Position: " + str(cube_creator.CubeCreating.position) + "\n"
	
	text = \
	"Cube Creator:\n" +\
	" - TimeUse: " + str(cube_creator.TimeUse) + "\n" +\
	" - CubeCreating: \n" + info
