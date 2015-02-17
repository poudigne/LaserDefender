using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

  public static int totalScore = 0;

  Text myText;

  void Start()
  {
    myText = GetComponent<Text>();
    myText.text = totalScore.ToString();
  }

  public void Score(int score)
  {
    totalScore += score;
    myText.text = totalScore.ToString();
  }

  public static void Reset()
  {
    totalScore = 0;
  }

}
