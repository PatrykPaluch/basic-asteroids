
using UnityEngine;

public class Meteor : MonoBehaviour {

	public static readonly MinMax SizeChange = new MinMax(0.5f, 0.75f);
	public static readonly MinMax InitialRotationSpeed = new MinMax(20, 120);
	public static readonly IntMinMax BreakCount = new IntMinMax(2, 4);
	public static readonly float MinimalSize = 0.5f;
	public float Size { get; private set; }
	public float RotationSpeed { get; private set; }

	public void InitNewMeteor(float size, Vector2 position, Vector2 velocity) {
		Size = size;
		
		Transform thisTransform = transform;
		thisTransform.localScale = Vector3.one * size;
		thisTransform.position = position;
		
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
				Vector2 newVelocity = GetRandomVelocity() + GetComponent<Rigidbody2D>().velocity;
				Vector2 position = transform.position;

				GameObject meteorGameObject = MeteorSpawner.Instance.MeteorPool.Get();
				if (meteorGameObject) 
					meteorGameObject.GetComponent<Meteor>().InitNewMeteor(newSize, position, newVelocity);
			}
		}

		ScoreManager.Instance.Score += 1;
		
		ReturnToPool();
	}

	private Vector2 GetRandomVelocity() {
		float randomAngle = Random.Range(0, 2 * Mathf.PI);
		
		return new Vector2(
			Mathf.Cos(randomAngle),
			Mathf.Sin(randomAngle));
	}

	private void ReturnToPool() {
		MeteorSpawner.Instance.MeteorPool.Return(gameObject);
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
			ReturnToPool();
		}
	}

	private float CalcNewSize() {
		return Size * SizeChange.GetRandomValue();
	}
}