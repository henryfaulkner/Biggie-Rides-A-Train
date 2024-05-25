extends TextureButton

var fx_bus = AudioServer.get_bus_index("Fx")

func _on_TextureButton_pressed():
	AudioServer.set_bus_mute(fx_bus, not AudioServer.is_bus_mute(fx_bus))
