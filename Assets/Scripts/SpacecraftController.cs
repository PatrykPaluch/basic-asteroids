using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpacecraftController : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 180;
    [SerializeField]
    private float acceleration = 0.5f;
    [SerializeField]
    private float bulletShootInterval = 0.25f;
    
    public Vector2 Position => transform.position;
    public Vector2 RigidbodyPosition => rigidbody.position;
    public float CurrentRotation { get; private set; }
    public Vector2 InputMovement { get; private set; }
    public bool InputShoot { get; private set; }

    private new Rigidbody2D rigidbody;
    private float currentBulletDelay;
    
    private void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        rigidbody.drag = 0;
    }
    
    private void Update() {
        InputMovement = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        InputShoot = Input.GetButton("Fire1");

        currentBulletDelay += Time.deltaTime;
        if (InputShoot) {
            if (currentBulletDelay >= bulletShootInterval) {
                currentBulletDelay = 0;
                Shoot();
            }
        }
    }

    private void FixedUpdate() {
        CurrentRotation -= InputMovement.x * rotationSpeed * Time.fixedDeltaTime;

        Vector2 velocity = rigidbody.velocity;
        
        Vector2 forwardVelocity = Time.fixedDeltaTime * acceleration * transform.up * InputMovement.y;

        Vector2 targetVelocity = velocity + forwardVelocity;

        rigidbody.velocity = targetVelocity;
        rigidbody.rotation = CurrentRotation;
    }

    private void Shoot() {
        Transform thisTransform = transform; //TODO Object pool
        Instantiate(GameManager.Instance.BulletPrefab, thisTransform.position, thisTransform.rotation);
    }
}
