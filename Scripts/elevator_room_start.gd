extends CSGBox3D


func _ready() -> void:
	$CanvasLayer/fade/AnimationPlayer.play("fading")


func _on_timer_timeout() -> void:
	$Elevator/AnimationPlayer.play("DoorOpen")
	$"/root/ElevatorSound".stop()
	$DoorOpen.play()
