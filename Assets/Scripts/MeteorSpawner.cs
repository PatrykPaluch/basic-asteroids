using UnityEngine;

public class MeteorSpawner : MonoBehaviour {
	
	private void Update() {
		if (Input.GetButtonDown("Fire2")) {
			CreateMeteor();
		}
	}

	private void CreateMeteor() {
		Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(-5, 5), 0);
		float randomAngle = Random.Range(0, 2 * Mathf.PI);
		Vector2 randomVelocity = new Vector2(
			Mathf.Cos(randomAngle),
			Mathf.Sin(randomAngle));
		GameObject newGo = Instantiate(GameManager.Instance.MeteorPrefab, pos, Quaternion.identity);
		newGo.GetComponent<Meteor>().InitNewMeteor(2, randomVelocity);
	}
}