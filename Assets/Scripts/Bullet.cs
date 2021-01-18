using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

	public static readonly float LifeTime = 3;
	public static readonly float DefaultSpeed = 6;

	public float Speed { get; set; }

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Meteor")) {
			OnMeteorCollision(other.gameObject);
		}
	}

	private void OnMeteorCollision(GameObject meteorGameObject) {
		Meteor meteor = meteorGameObject.GetComponent<Meteor>();
		meteor.Break();
		ReturnToPool(); //return to pool
	}

	public void ResetBullet() {
		Rigidbody2D rb = GetComponent<Rigidbody2D>();

		rb.drag = 0;
		rb.isKinematic = true;
		rb.velocity = (DefaultSpeed + Speed) * transform.up;

		StartCoroutine(WaitAndDestroy());
	}

	private IEnumerator WaitAndDestroy() {
		yield return new WaitForSeconds(LifeTime);
		ReturnToPool();
	}

	private void ReturnToPool() {
		GameManager.Instance.BulletPool.Return(gameObject);
	}
}