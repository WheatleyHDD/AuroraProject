extends CSGBox3D


func _on_timer_timeout() -> void:
	$Elevator/AnimationPlayer.play("DoorOpen")
