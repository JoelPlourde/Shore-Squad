using UnityEngine;

public class WaveGenerator : MonoBehaviour, IUpdatable {

	public float scale = 2f;
	public float speed = 1.5f;
	public float noiseStrength = 3f;
	public float noiseWalk = 1f;

	private Vector3[] _baseHeight;
	private Vector3[] _vertices;
	private Vector3 _vertex;
	private Mesh _mesh;

	private void Start() {
		_mesh = GetComponent<MeshFilter>().mesh;
		_baseHeight = _mesh.vertices;

		_vertices = new Vector3[_baseHeight.Length];

		GameController.Instance.RegisterUpdatable(this);
	}

	public void OnUpdate() {
		for (int i = 0; i < _vertices.Length; i++) {
			_vertex = _baseHeight[i];
			_vertex.y += Mathf.Sin(Time.time * speed + _baseHeight[i].x + _baseHeight[i].y + _baseHeight[i].z) * scale;
			_vertex.y += Mathf.PerlinNoise(_baseHeight[i].x + noiseWalk, _baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
			_vertices[i] = _vertex;
		}
		_mesh.vertices = _vertices;
		_mesh.RecalculateNormals();
	}

	private void OnDestroy() {
		if (GameController.Instance.Alive) {
			GameController.Instance.DeregisterUpdatable(this);
		}
	}
}