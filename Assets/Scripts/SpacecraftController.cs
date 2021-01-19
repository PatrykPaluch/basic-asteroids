using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpacecraftController : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 180;
    [SerializeField]
    private float acceleration = 0.5f;
    [SerializeField]
    private float bulletShootInterval = 0.25f;

    [SerializeField]
    private ParticleSystem playerDeathExplosionEffect;
    
    public Vector2 Position => transform.position;
    public Vector2 RigidbodyPosition => rigidbody.position;
    public float CurrentRotation { get; private set; }
    public Vector2 InputMovement { get; private set; }
    public bool InputShoot { get; private set; }

    public float BulletShootInterval => bulletShootInterval;
    public float Acceleration => acceleration;
    public float RotationSpeed => rotationSpeed;

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
        
        Vector2 forwardVelocity = Time.fixedDeltaTime * acceleration * InputMovement.y * transform.up;

        Vector2 targetVelocity = velocity + forwardVelocity;

        rigidbody.velocity = targetVelocity;
        rigidbody.rotation = CurrentRotation;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Meteor")) {
            GameManager.Instance.GameOver();
        }
    }

    private void Shoot() {
        GameObject bullet = GameManager.Instance.BulletPool.Get();
        if (bullet) {
            Transform thisTransform = transform;
            Transform bulletTransform = bullet.transform;
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            
            bulletTransform.position = thisTransform.position;
            bulletTransform.rotation = thisTransform.rotation;
            
            bulletComponent.Speed = rigidbody.velocity.magnitude;
            bulletComponent.ResetBullet();
        }
    }

    public void Disable() {
        CameraShaker shaker = GameManager.Instance.Shaker;
        shaker.Shake(shaker.shakeAmplitude * 3f, 0.75f);
        gameObject.SetActive(false);
        playerDeathExplosionEffect.transform.position = transform.position;
        playerDeathExplosionEffect.gameObject.SetActive(true);
        playerDeathExplosionEffect.Play();
    }
}
