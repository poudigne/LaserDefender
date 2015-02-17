using UnityEngine;
using System.Collections;

public class FormationController : MonoBehaviour {

  public GameObject enemyPrefab;
  public float width = 10.0f;
  public float height = 5.0f;
  public float speed = 5.0f;
  public float padding = 1.0f;
  public float spawnDelaySeconds = 0.5f;

  private int direction = 1;
  private float boundaryRightEdge, boundaryLeftEdge;

	// Use this for initialization
	void Start () {
    Camera camera = Camera.main;
    float distance = transform.position.z - camera.transform.position.z;
    boundaryLeftEdge = camera.ViewportToWorldPoint(new Vector3(0, 0, distance)).x - padding;
    boundaryRightEdge = camera.ViewportToWorldPoint(new Vector3(1, 1, distance)).x + padding;
    SpawnUntilFull();
	}
	
	// Update is called once per frame
	void Update () {

    float formationRightEdge = transform.position.x + 0.5f * width;
    float formationLeftEdge = transform.position.x - 0.5f * width;
    if (formationRightEdge > boundaryRightEdge)
    {
      direction = -1;
    }
    if (formationLeftEdge < boundaryLeftEdge)
    {
      direction = 1;
    }
    transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);

    if (AllMembersAreDead())
    {
      SpawnUntilFull();
    }
	}

  void OnDrawGizmos()
  {
    float xMin, xMax, yMin, yMax;
    xMin = transform.position.x - 0.5f * width;
    xMax = transform.position.x + 0.5f * width;
    yMin = transform.position.y - 0.5f * height;
    yMax = transform.position.y + 0.5f * height;
    Gizmos.DrawLine(new Vector3(xMin, yMin, 0), new Vector3(xMin, yMax));
    Gizmos.DrawLine(new Vector3(xMin, yMax, 0), new Vector3(xMax, yMax));
    Gizmos.DrawLine(new Vector3(xMax, yMax, 0), new Vector3(xMax, yMin));
    Gizmos.DrawLine(new Vector3(xMax, yMin, 0), new Vector3(xMin, yMin));
  }

  void SpawnUntilFull()
  {
    Transform freePos = NextFreePosition();
    if (freePos == null)
      return;
    GameObject enemy = Instantiate(enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
    enemy.transform.parent = freePos;
    if (FreePositionExist())
      Invoke("SpawnUntilFull", spawnDelaySeconds);
  }

  bool FreePositionExist()
  {
    foreach (Transform position in transform)
    {
      if (position.childCount > 0)
        return true;
    }
    return false;
  }

  Transform NextFreePosition()
  {
    foreach (Transform position in transform)
    {
      if (position.childCount == 0)
        return position;
    }
    return null;
  }

  bool AllMembersAreDead()
  {
    foreach (Transform position in transform)
      if (position.childCount > 0)
        return false;
    return true;
  }
  
}
