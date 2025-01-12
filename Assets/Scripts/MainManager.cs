using JetBrains.Annotations;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public string playerName;
    public TMP_InputField inputName;

    public Text ScoreText;
    public GameObject GameOverText;

    public Text highScoreText;
    public Text highScoreTextmain;


    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static MainManager instance;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (LoadFile() !=null)
        {
            inputName.text = LoadFile().lastUsedPlayerName;

            highScoreText.text = "Best Score: " + LoadFile().highScorePlayerName + " : " + LoadFile().highScore;
        }


        

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_GameOver = false;
                m_Started = false;
                m_Points = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > LoadFile().highScore)
        {
            SaveHighScore();
            UpdateHighScore();
        }

        m_GameOver = true;
        GameOverText.SetActive(true);
    }


    
    public void SpawnBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }




    public void Button_Start()
    {
        playerName = inputName.text;
        SaveName();

        SceneManager.LoadScene("main");
    }


    //___SAVEDATA_________________________________________________________________________
    [System.Serializable]
    public class SaveData
    {
        public string highScorePlayerName;
        public int highScore;
        public string lastUsedPlayerName;
    }


    //SAVE ON STARTGAME
    public void SaveName()
    {
        SaveData dataToSave = new SaveData();

        if (LoadFile() != null)
        {
            dataToSave.lastUsedPlayerName = playerName;

            dataToSave.highScorePlayerName = LoadFile().highScorePlayerName;
            dataToSave.highScore = LoadFile().highScore;

        }
        else
        {   
            dataToSave.lastUsedPlayerName = playerName;
        }

        string json = JsonUtility.ToJson(dataToSave); //convert savedData to string
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }

    //SAVE ON HIGHSCORE
    public void SaveHighScore()
    {
        SaveData dataToSave = new SaveData();
        dataToSave.highScorePlayerName = playerName;
        dataToSave.highScore = m_Points;
        dataToSave.lastUsedPlayerName = playerName;

        string json = JsonUtility.ToJson(dataToSave); //convert savedData to string
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    //LOAD FILE
    public SaveData LoadFile()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);
            return loadedData;
        }
        else
        {
            return null;
        }
    }


    public void UpdateHighScore()
    {
        highScoreTextmain.text = "Best Score: " + LoadFile().highScorePlayerName + " : " + LoadFile().highScore;
    }


}
