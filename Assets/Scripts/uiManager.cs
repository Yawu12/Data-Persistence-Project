using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{

    public Text ScoreText;
    public GameObject GameOverText;
    public Text PlayerNameText;

    public Text HighScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainManager.instance.ScoreText = ScoreText;
        MainManager.instance.GameOverText = GameOverText;

        PlayerNameText.text = MainManager.instance.playerName;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
