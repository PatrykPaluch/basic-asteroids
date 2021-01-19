using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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

	[SerializeField]
	private CameraShaker shaker;
	
	public GameObject MeteorPrefab => meteorPrefab;
	public GameObject BulletPrefab => bulletPrefab;
	
	public SpacecraftController Spacecraft => spacecraft;
	public Camera MainCamera => mainCamera;
	public Vector2 SpacecraftPosition => spacecraft.Position;

	public CameraShaker Shaker => shaker;

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

	public void GameOver() {
		ApplicationData.LastScore = ScoreManager.Instance.Score;
		Spacecraft.Disable();
		StartCoroutine(WaitToEnd());
	}

	private IEnumerator WaitToEnd() {
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene("Scenes/GameEndScene");
	}
}