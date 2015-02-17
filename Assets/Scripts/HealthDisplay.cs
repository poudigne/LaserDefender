using UnityEngine;
using System.Collections;

public class HealthDisplay : MonoBehaviour {

	public void HealthChanged(float newHealth)
  {
    for (int i = 0; i < gameObject.transform.childCount; i++ )
    {
      var healthComponent = gameObject.transform.GetChild(i);
      if (i >= newHealth)
      {
        healthComponent.gameObject.SetActive(false);
      }
    }
  }
}
