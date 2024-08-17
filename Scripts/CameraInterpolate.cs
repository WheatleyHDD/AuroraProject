using Godot;
using System;

public partial class CameraInterpolate : Camera3D
{
	private Node3D _target;
	private Node3D _head;
	private Node3D _cameraContainer;
	
	private Timer _shakeTimer;

	private float _shakeIntensity = 0.0f;
	
	public override void _Ready()
	{
		_cameraContainer = GetParent<Node3D>();
		_head = _cameraContainer.GetParent<Node3D>();
		_target = _head.GetParent<Node3D>();
		
		_cameraContainer.TopLevel = true;

		_shakeTimer = new Timer();
		AddChild(_shakeTimer);
		_shakeTimer.OneShot = true;
		_shakeTimer.Timeout += () => _shakeIntensity = 0f;
	}

	public override void _Process(double delta)
	{
		var cameraGt = _cameraContainer.GlobalTransform;
		cameraGt.Origin = _cameraContainer.GlobalTransform.Origin.Lerp(
			_head.GlobalTransform.Origin, 
			50 * (float)delta
			);
		_cameraContainer.GlobalTransform = cameraGt;

		var camRot = _cameraContainer.Rotation;
		camRot.Y = _target.Rotation.Y;
		camRot.X = _head.Rotation.X;
		_cameraContainer.Rotation = camRot;

		CameraMove((float)delta);
		Shaking();
	}

	/// <summary>
	/// Наклон камеры в сторону при такогом движении
	/// </summary>
	/// <param name="delta">delta из _Process</param>
	private void CameraMove(float delta)
	{
		_cameraContainer.Rotation = new Vector3(_cameraContainer.Rotation.X, _cameraContainer.Rotation.Y,
			Mathf.Lerp(
				_cameraContainer.Rotation.Z,
				Input.GetAxis("move_right", "move_left") * 0.025f,
				60 * delta * 0.3f)
		);
	}

	private void Shaking()
	{
		GD.Randomize();
		Position = new Vector3(
			(float)GD.RandRange(-_shakeIntensity, _shakeIntensity),
			(float)GD.RandRange(-_shakeIntensity, _shakeIntensity),
			(float)GD.RandRange(-_shakeIntensity, _shakeIntensity)
		);
	}
	
	/// <summary>
	/// Тряска экрана
	/// </summary>
	/// <param name="intensity"></param>
	/// <param name="time"></param>
	public void Shake(float intensity, float time = 1f)
	{
		_shakeTimer.Stop();
		_shakeTimer.Start(time);
		_shakeIntensity = intensity;
	}
}
