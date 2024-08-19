extends CanvasLayer

@export var voices: Array[VoiceAndTitle]

var tween: Tween

func _process(delta: float) -> void:
	if tween:
		if Engine.time_scale < 1:
			tween.set_speed_scale(10.0)
		else:
			tween.set_speed_scale(1.0)

func play_voice(voice_name: String) -> void:
	if tween:
		tween.stop()
	
	var v = voices.filter(func (voice): return voice.voice_name == voice_name)
	$AudioStreamPlayer.stream = v[0].voice
	$AudioStreamPlayer.play()
	
	$Label.text = v[0].subtitle
	$Label.visible_characters = 0
	tween = get_tree().create_tween().set_process_mode(Tween.TWEEN_PROCESS_IDLE)
	tween.tween_property($Label, "visible_ratio", 1, v[0].voice.get_length())
	tween.tween_interval(1)
	tween.tween_callback(func (): $Label.text = "")
