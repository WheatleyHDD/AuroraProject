extends Sprite3D

@export var red: Texture2D
@export var green: Texture2D

var value = false

func send_open_state():
	texture = green
	value = true

func send_close_state():
	texture = red
	value = false
