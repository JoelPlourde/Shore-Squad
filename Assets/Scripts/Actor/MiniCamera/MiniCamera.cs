using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MiniCamera : MonoBehaviour
{
	private Camera _camera;

	private void Awake() {
		_camera = GetComponent<Camera>();
		_camera.clearFlags = CameraClearFlags.SolidColor;
		_camera.backgroundColor = new Color32(0x41, 0x3F, 0x3F, 0xFF);

		int mask = 0;
		mask |= (1 << LayerMask.NameToLayer("Actor"));
		_camera.cullingMask = mask;
	}

	public void Initialize(RenderTexture renderTexture) {
		_camera.targetTexture = renderTexture;
	}
}
