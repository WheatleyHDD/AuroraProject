extends CSGBox3D

signal elevator_opened
signal elevator_closed

func _on_opener_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		$Elevator/AnimationPlayer.play("DoorOpen")
		emit_signal("elevator_opened")
		$Opener.queue_free()


func _on_closer_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		$Elevator/AnimationPlayer.play_backwards("DoorOpen")
		emit_signal("elevator_closed")
		$Closer.queue_free()
