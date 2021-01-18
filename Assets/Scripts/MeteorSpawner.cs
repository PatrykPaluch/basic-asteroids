using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorSpawner : MonoBehaviour {

	public static MeteorSpawner Instance { get; private set; }

	public float initialMeteorSize = 2;
	public GameObjectPool MeteorPool => meteorPool;
	
	private GameObjectPool meteorPool;
	
	public float meteorSpawnInterval = 3;
	private float currentDelay;
	
	
	private void Awake() {
		if (Instance != null) {
			Debug.LogError("Multiple instances of MeteorSpawner", this);
			Destroy(this);
			return;
		}
		Instance = this;

		meteorPool = new GameObjectPool(GameManager.Instance.MeteorPrefab, 100);
		currentDelay = 0;
	}

	private void Update() {
		currentDelay += Time.deltaTime;
		if (currentDelay >= meteorSpawnInterval) {
			currentDelay = 0;
			CreateMeteor();
		}
	}

	private void CreateMeteor() {
		
		Vector2 position = GetRandomPositionOutsideCamera();

		Vector2 randomVelocity = GetVelocityToSpacecraft(position);
		
		GameObject newGo = meteorPool.Get();
		if(newGo)
			newGo.GetComponent<Meteor>().InitNewMeteor(initialMeteorSize, position, randomVelocity);
	}

	private Vector2 GetVelocityToSpacecraft(Vector2 meteorPosition) {
		float margin = 2;
		
		Vector2 spacecraftPosition = GameManager.Instance.SpacecraftPosition;
		Vector2 randomPositionAroundSpacecraft = new Vector2(
			Random.Range(spacecraftPosition.x - margin, spacecraftPosition.x + margin),
			Random.Range(spacecraftPosition.y - margin, spacecraftPosition.y + margin));
		
		return (randomPositionAroundSpacecraft - meteorPosition).normalized;
	}

	private Vector2 GetRandomPositionOutsideCamera() {
		Bounds cameraBounds = CameraBounds();
		float margin = initialMeteorSize * 1.5f;
		

		Vector2 pos = new Vector2();
		// true - left/right; false - top/bottom
		if (RandomBool()) {
			pos.y = Random.Range(cameraBounds.min.y - margin, cameraBounds.max.y + margin);

			// true - left; false - right
			if (RandomBool())
				pos.x = cameraBounds.min.x - margin;
			else
				pos.x = cameraBounds.max.x + margin;
		}
		else {
			pos.x = Random.Range(cameraBounds.min.x - margin, cameraBounds.max.x + margin);

			// true - bottom; false - top
			if (RandomBool())
				pos.y = cameraBounds.min.y - margin;
			else
				pos.y = cameraBounds.max.y + margin;
		}

		return pos;
	}

	private Bounds CameraBounds() {
		Camera mainCamera = GameManager.Instance.MainCamera;
		float screenAspect = mainCamera.aspect;
		float cameraHeight = mainCamera.orthographicSize * 2;
		
		Bounds bounds = new Bounds(
			mainCamera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
		
		return bounds;
	}

	private bool RandomBool() {
		return Random.Range(0, 2) == 1;
	}
}