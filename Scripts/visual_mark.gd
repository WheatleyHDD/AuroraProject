extends Decal

@export var red: Texture2D
@export var green: Texture2D

var value = false

func send_open_state():
	texture_albedo = green
	value = true

func send_close_state():
	texture_albedo = red
	value = false
