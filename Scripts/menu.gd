extends Control


func _ready() -> void:
	var mp: AudioStreamPlayer = $"/root/MusicPlayer"
	mp.get_stream_playback().switch_to_clip_by_name("menu")


func _on_new_game_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/level_1.tscn")


func _on_exit_pressed() -> void:
	get_tree().quit()


func _on_settings_pressed() -> void:
	$"/root/Overlays/Settings".show()


func _on_about_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/readme.tscn")
