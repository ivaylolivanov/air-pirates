using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeedTune = 5f;
    [SerializeField] float playerPaddingX = 1f;
    [SerializeField] float playerPaddingY = 1f;
    [SerializeField] float health = 200f;
    [SerializeField] GameObject hitParticle;
    [SerializeField] float hitVFXDuration = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject Bullet;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float firingPeriod = 0.1f;

    [Header("Audio")]
    [SerializeField] [Range(0,1)]float fireVolume = 0.1f;
    [SerializeField] AudioClip fireClip;
    [SerializeField] [Range(0,1)]float destroyVolume = 0.7f;
    [SerializeField] AudioClip destroyClip;


    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer =
            other.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        GameObject hitVFX = Instantiate(
            hitParticle,
            transform.position,
            transform.rotation
        );
        Destroy(hitVFX, hitVFXDuration);

        if(health <= 0) {
            FindObjectOfType<SceneLoader>().LoadGameOver();
            AudioSource.PlayClipAtPoint(
                destroyClip,
                Camera.main.transform.position,
                destroyVolume
            );
            Destroy(gameObject);
        }
    }

    public float GetHealth() {
        return health;
    }

    private void Move() {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeedTune;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeedTune;

        Vector2 newPosition = new Vector2(
            Mathf.Clamp(transform.position.x + deltaX, xMin, xMax),
            Mathf.Clamp(transform.position.y + deltaY, yMin, yMax)
        );

        transform.position = newPosition;
    }

    private void Fire(){
        if(Input.GetButtonDown("Fire1")) {
            firingCoroutine = StartCoroutine(RepeatFire());
        }
        if(Input.GetButtonUp("Fire1")) {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator RepeatFire() {
        while (true)
        {
            GameObject bullet = Instantiate(
                Bullet,
                transform.position,
                Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
                0, projectileSpeed
            );
            AudioSource.PlayClipAtPoint(
                fireClip,
                Camera.main.transform.position,
                fireVolume
            );
            yield return new WaitForSeconds(firingPeriod);
        }
    }

    private void SetUpMoveBoundaries() {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + playerPaddingX;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - playerPaddingX;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + playerPaddingY;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - playerPaddingY;

    }
}
