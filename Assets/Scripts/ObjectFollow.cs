using UnityEngine;

public class ObjectFollow : MonoBehaviour {


	public Transform objectToFollow;
	public float smoothTime;

	private new Transform transform;
	private Vector2 velocity;

	private void Awake() {
		transform = GetComponent<Transform>();
	}

	private void LateUpdate() {
		Vector3 position = transform.position;
		
		Vector2 smoothPosition = Vector2.SmoothDamp(position, objectToFollow.position, ref velocity, smoothTime);
		
		transform.position = new Vector3(smoothPosition.x, smoothPosition.y, position.z);
	}
}