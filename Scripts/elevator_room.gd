extends CSGBox3D

@export var next_level: PackedScene

var player

signal elevator_opened
signal elevator_closed

func _on_opener_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		$Elevator/AnimationPlayer.play("DoorOpen")
		emit_signal("elevator_opened")
		$Opener.queue_free()


func _on_closer_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		player = body
		$Elevator/AnimationPlayer.play_backwards("DoorOpen")
		emit_signal("elevator_closed")
		$CutsceneTimer.start()
		$Closer.queue_free()


func _on_cutscene_timer_timeout() -> void:
	player.get_node("Head/CameraContainer/Camera3D").Shake(0.05, 2)
	$CanvasLayer/fade/AnimationPlayer.play("fading")
	$CutsceneTimer2.start()


func _on_cutscene_timer_2_timeout() -> void:
	get_tree().change_scene_to_packed(next_level)
