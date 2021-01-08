
using UnityEngine;

public class Meteor : MonoBehaviour {

	public static readonly MinMax SizeChange = new MinMax(0.5f, 0.75f);
	public static readonly MinMax InitialRotationSpeed = new MinMax(20, 120);
	public static readonly IntMinMax BreakCount = new IntMinMax(2, 4);
	public static readonly float MinimalSize = 0.5f;
	public float Size { get; private set; }
	public float RotationSpeed { get; private set; }

	public void InitNewMeteor(float size, Vector2 velocity) {
		Size = size;
		transform.localScale = Vector3.one * size;
		RotationSpeed = InitialRotationSpeed.GetRandomValue();
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.isKinematic = true;
		rb.freezeRotation = true;
		rb.drag = 0;
		rb.velocity = velocity;
	}

	private void Update() {
		transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
		AutoRemove();
	}

	public void Break() {
		float newSize = CalcNewSize();
		if (newSize >= MinimalSize) {
			int newMeteorCount = BreakCount.GetRandomValue();
			for (int i = 0; i < newMeteorCount; i++) {
				float randomAngle = Random.Range(0, 2 * Mathf.PI);
				Vector2 randomVelocity = new Vector2(
						Mathf.Cos(randomAngle),
						Mathf.Sin(randomAngle));

				Vector2 newVelocity = randomVelocity + GetComponent<Rigidbody2D>().velocity;
				
				GameObject meteorGameObject = Instantiate(
					GameManager.Instance.MeteorPrefab, 
					transform.position, 
					Quaternion.identity);
				Meteor meteor = meteorGameObject.GetComponent<Meteor>();
				meteor.InitNewMeteor(newSize, newVelocity);
			}
		}
		Destroy(gameObject);
	}

	private void AutoRemove() {
		Vector2 spacecraftPosition = GameManager.Instance.SpacecraftPosition;
		Vector2 thisPosition = transform.position;
		float distance = Vector2.Distance(spacecraftPosition, thisPosition);
		
		Camera camera = GameManager.Instance.MainCamera;
		float orthoSize = camera.orthographicSize;
		float aspect = camera.aspect;
		float screenWidth = orthoSize * aspect;

		if (distance > screenWidth * 2) {
			Destroy(gameObject);
		}
	}

	private float CalcNewSize() {
		return Size * SizeChange.GetRandomValue();
	}
}