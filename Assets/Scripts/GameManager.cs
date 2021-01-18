using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }
	
	
	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private SpacecraftController spacecraft;

	[SerializeField]
	private GameObject meteorPrefab;
	[SerializeField]
	private GameObject bulletPrefab;

	public GameObject MeteorPrefab => meteorPrefab;
	public GameObject BulletPrefab => bulletPrefab;
	
	public SpacecraftController Spacecraft => spacecraft;
	public Camera MainCamera => mainCamera;
	public Vector2 SpacecraftPosition => spacecraft.Position;


	private GameObjectPool bulletPool;
	public GameObjectPool BulletPool => bulletPool;


	private void Awake() {
		if (Instance != null) {
			Debug.LogError("Multiple instances of GameManager", this);
			Destroy(this);
			return;
		}

		Instance = this;
		if (mainCamera == null) {
			Debug.LogError("MainCamera is null", this);
		}
		if (spacecraft == null) {
			Debug.LogError("Spacecraft is null", this);
		}
		
		bulletPool = new GameObjectPool(
			bulletPrefab, 
			Mathf.CeilToInt(1.0f / spacecraft.BulletShootInterval * Bullet.LifeTime + 0.1f));
	}
}