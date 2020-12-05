using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100f;
    [SerializeField] float deathVFXDuration = 1f;
    [SerializeField] int scoreValue = 100;

    [Header("Projectile")]
    float shotCounter = 0;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile = null;
    [SerializeField] GameObject hitParticle;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Audio")]
    [SerializeField] [Range(0,1)]float fireVolume = 0.3f;
    [SerializeField] AudioClip fireClip;
    [SerializeField] [Range(0,1)]float destroyVolume = 0.7f;
    [SerializeField] AudioClip destroyClip;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot() {
        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0f) {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            AudioSource.PlayClipAtPoint(
                fireClip,
                Camera.main.transform.position,
                fireVolume
            );
        }
    }

    private void Fire() {
        if (!projectile) { return; }
        GameObject bullet = Instantiate(
            projectile,
            transform.position,
            Quaternion.identity
        ) as GameObject;

        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        GameObject hitVFX = Instantiate(
            hitParticle,
            transform.position,
            Quaternion.identity
        );

        Destroy(hitVFX, deathVFXDuration);

        if(health <= 0) {
            Die();
        }
    }

    private void Die() {
        FindObjectOfType<GameSession>().AddPoints(scoreValue);

        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(
            destroyClip,
            Camera.main.transform.position,
            destroyVolume
        );
    }
}
