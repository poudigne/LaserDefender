using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
  public GameObject projectile;
  public float health = 2;
  public float missileSpeed = 10.0f;
  public float shotPerSecond = 0.5f;
  public int scoreValue = 150;

  public AudioClip laserSFX;
  public AudioClip loseSFX;

  ScoreKeeper scoreKeeper;

  void Start()
  {
    scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
  }
  void Update()
  {
    float probability = shotPerSecond * Time.deltaTime;
    if (Random.value < probability)
      Fire();
  }


  void Fire()
  {
    GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
    missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -missileSpeed);
    AudioSource.PlayClipAtPoint(laserSFX, transform.position);
  }
  void OnTriggerEnter2D(Collider2D collider)
  {
    Projectile missile = collider.gameObject.GetComponent<Projectile>();

    if (missile)
    {
      health -= missile.GetDamage();
      missile.Hit();
      if (health <= 0)
      {
        Die();
      }
    }
  }

  void Die()
  {
    Destroy(gameObject);
    scoreKeeper.Score(scoreValue);
    AudioSource.PlayClipAtPoint(loseSFX, transform.position);
  }
}
