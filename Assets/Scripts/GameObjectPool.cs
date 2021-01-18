
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameObjectPool {

	public delegate void ResetGameObjectFunction(GameObject pooledObject);

	private readonly List<GameObject> pool;
	public int PoolSize => pool.Count;

	public GameObjectPool(GameObject template, int poolSize) {

		pool = new List<GameObject>(poolSize);
		for (int i = 0; i < poolSize; i++) {
			GameObject clone = Object.Instantiate(template, Vector3.zero, quaternion.identity);
			clone.SetActive(false);
			pool.Add(clone);
		}
	}

	/// <summary>
	/// Returns first free GameObject from pool. SetActive on GameObject is executed after resetting.
	/// </summary>
	/// <returns>GameObject from pool or null if every GameObject is in use</returns>
	public GameObject Get() {
		foreach (GameObject pooledGameObject in pool) {
			if (!pooledGameObject.activeSelf) {
				
				pooledGameObject.SetActive(true);
				return pooledGameObject;
			}
		}

		return null;
	}

	/// <summary>
	/// Its just pooledGameObject.SetActive(false).
	/// </summary>
	/// <param name="pooledGameObject">gameObject to return to pool</param>
	public void Return(GameObject pooledGameObject) {
		pooledGameObject.SetActive(false);
	}

}