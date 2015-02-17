using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
  public GameObject healthPanel;
  public GameObject missilePrefab;

  public float speed = 15.0f;
  public float beamSpeed = 10.0f;
  public float padding = 1.0f;
  public float firingDelay = 0.25f;
  public float health = 3;
  public AudioClip laserSFX;
  public AudioClip loseSFX;
  public float invincibleTime = 3.0f;
  
  private float xMax = -5;
  private float xMin = 5;
  private bool isInvincible = false;
  private float invincibleTimeElapsed = 0;

  // Called first when the Game object is created into the scene
  void Start()
  {
    Camera camera = Camera.main;
    float distance = transform.position.z - camera.transform.position.z;
    xMin = camera.ViewportToWorldPoint(new Vector3(0, 0, distance)).x + padding;
    xMax = camera.ViewportToWorldPoint(new Vector3(1, 1, distance)).x - padding;
  }

  // Update is called once per frame
  void Update()
  {

    if (IsMovingLeft())
    {
      float NewPos = Mathf.Clamp(transform.position.x - speed * Time.deltaTime, xMin, xMax);
      transform.position = new Vector3(NewPos, transform.position.y, transform.position.z);
    }

    if (IsMovingRight())
    {
      float NewPos = Mathf.Clamp(transform.position.x + speed * Time.deltaTime, xMin, xMax);
      transform.position = new Vector3(NewPos, transform.position.y, transform.position.z);
    }
    if (IsShooting())
    {
      InvokeRepeating("FireLaser", 0.0001f, firingDelay);
    }
    if (IsStopShooting())
    {
      CancelInvoke("FireLaser");
    }
    if (isInvincible)
    {
      SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
      sr.color = new Color(255, 255, 255, 0);

      invincibleTimeElapsed += Time.deltaTime;
      Debug.Log("Invincible time : " + invincibleTimeElapsed);
      if (invincibleTimeElapsed > invincibleTime)
      {
        Debug.Log("No more invincible!"); 
        sr.color = new Color(255, 255, 255, 255);
        isInvincible = false;
      }
    }
    

  }
  void OnTriggerEnter2D(Collider2D collider)
  {
    if (isInvincible)
      return;
    Projectile missile = collider.gameObject.GetComponent<Projectile>();

    if (missile)
    {
      health -= missile.GetDamage();
      missile.Hit();
      
      if (health <= 0)
      {
        Die();
      }
      else
      {
        Debug.Log("now invincible !");
        isInvincible = true;
        invincibleTimeElapsed = 0;
        NotifyHealthChange();
      }
    }
  }
  bool IsShooting()
  {
    return Input.GetKeyDown(KeyCode.Space);
  }
  bool IsStopShooting()
  {
    return Input.GetKeyUp(KeyCode.Space);
  }
  // If A or Left Arrow down then moving to Left
  bool IsMovingLeft()
  {
    return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
  }
  // If D or Right Arrow down then moving to Left
  bool IsMovingRight()
  {
    return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
  }

  void FireLaser()
  {
    GameObject beam = Instantiate(missilePrefab, transform.position, Quaternion.identity) as GameObject;
    beam.rigidbody2D.velocity = new Vector2(0, beamSpeed);
    AudioSource.PlayClipAtPoint(laserSFX, transform.position);
  }

  void Die()
  {
    Destroy(gameObject);
    AudioSource.PlayClipAtPoint(loseSFX, transform.position);
    LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    man.LoadLevel("Win Screen");
  }
  public float GetHealth()
  {
    return health;
  }

  void NotifyHealthChange()
  {
    var hpDisplay = healthPanel.GetComponent<HealthDisplay>();
    hpDisplay.HealthChanged(health);
  }
}
