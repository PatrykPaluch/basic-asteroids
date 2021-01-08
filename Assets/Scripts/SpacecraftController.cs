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
        Vector2 inputDirection = transform.up * acceleration;
        
        Vector2 forwardInputVelocity = inputDirection * InputMovement.y * Time.fixedDeltaTime;

        Vector2 targetVelocity = velocity + forwardInputVelocity;
        if (InputMovement.y < 0 && SameVectorDirection(targetVelocity, velocity )) {
            targetVelocity = Vector2.zero;
        }

        rigidbody.velocity = targetVelocity;
        rigidbody.rotation = CurrentRotation;
    }

    private bool SameVectorDirection(Vector2 a, Vector2 b) {
        return (!IsFloatZero(a.x) || !IsFloatZero(a.y))
               && (!SameSign(a.x, b.x) || !SameSign(a.y, b.y));
    }

    private bool IsFloatZero(float f) {
        return Math.Abs(f) < 0.001;
    }

    private bool SameSign(float a, float b) {
        return Math.Sign(a) == Math.Sign(b);
    }
    
    
    private void Shoot() {
        Transform thisTransform = transform; //TODO Object pool
        Instantiate(GameManager.Instance.BulletPrefab, thisTransform.position, thisTransform.rotation);
    }
}
