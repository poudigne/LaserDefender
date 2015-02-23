using UnityEngine;
using HighscoreManager.Highscore;

namespace Assets.Scripts
{
  public class HighscoreController : MonoBehaviour {
     
    // Use this for initialization
    void Start () {
      var highscore = new HighscoreManager.Highscore.HighscoreManager("laserdefender");
      highscore.PostHighscore("test", ScoreKeeper.totalScore);

      var highScoreList = highscore.RetrieveHighscore();
      Debug.Log(highScoreList);
    }
	
    // Update is called once per frame
    void Update () {
	
    }
  }
}
