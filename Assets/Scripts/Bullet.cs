using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

	public static readonly float Speed = 6;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Meteor")) {
			OnMeteorCollision(other.gameObject);
		}
	}

	private void OnMeteorCollision(GameObject meteorGameObject) {
		Meteor meteor = meteorGameObject.GetComponent<Meteor>();
		meteor.Break();
		Destroy(gameObject); //TODO Object pool
	}

	private void Start() {
		Rigidbody2D rb = GetComponent<Rigidbody2D>();

		rb.drag = 0;
		rb.isKinematic = true;
		rb.velocity = transform.up * Speed;
		Debug.Log(rb.velocity);
		
		Destroy(gameObject, 5); //TODO Object pool
	}
}