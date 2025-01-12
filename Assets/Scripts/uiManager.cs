using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{

    public Text ScoreText;
    public GameObject GameOverText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainManager.instance.ScoreText = ScoreText;
        MainManager.instance.GameOverText = GameOverText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
