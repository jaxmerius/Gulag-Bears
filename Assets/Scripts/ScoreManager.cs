using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Text endScore;
    public Text highScore;
    public Text newHighScore;

    void Start()
    {
        endScore.text = "Score: " + PlayerPrefs.GetInt("score").ToString();
        highScore.text = "High Score: " + PlayerPrefs.GetInt("highScore").ToString();

        if (PlayerPrefs.HasKey("isHigh"))
        {
            if (PlayerPrefs.GetString("isHigh") == "true")
            {
                newHighScore.text = "New High Score!";
            }
            else
            {
                newHighScore.text = "";
            }
        }
        else
        {
            newHighScore.text = "";
        }
    }
}
