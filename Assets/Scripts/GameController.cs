using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [Range(0f, 0.20f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    private AudioSource musicPlayer;

    public Text recordText;
    public Text pointsText;

    public GameObject UIidle;
    public GameObject UIScore;

    public enum GameState { Idle, Playing, Ended, Ready };
    public GameState gameState = GameState.Idle;

    public GameObject player;
    public GameObject enemyGenerator;


    public float scaleTime = 6f;
    public float scaleInc = .25f;
    private int points = 0;

    // Use this for initialization
    void Start () {
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "BEST: "+ GetMaxScore().ToString();
	}
	
	// Update is called once per frame
	void Update () {
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);
        if (gameState == GameState.Idle && (userAction))
        {
            gameState = GameState.Playing;
            
            UIScore.SetActive(true);
            UIidle.SetActive(false);
            player.SendMessage("UpdateState", "PlayerRun");
            player.SendMessage("dustPlay");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("gameTimeScale", scaleTime, scaleTime);
            //Juego en marcha
        }
        else if (gameState == GameState.Playing)
        {
            Parallax();
        } else if (gameState == GameState.Ready)
        {
            if (userAction)
            {
                RestartGame();
            }
        }

    }

    void Parallax()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
    }

    public void RestartGame()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Guy");
    }

    void gameTimeScale()
    {
        Time.timeScale += scaleInc;

    }

    public void ResetTimeScale(float newTimeScale = 1f)
    {
        CancelInvoke("gameTimeScale");
        Time.timeScale = newTimeScale;
    }


    public void IncreasePoint()
    {
        pointsText.text = (++points).ToString();
        if (points >= GetMaxScore())
        {
            recordText.text = "BEST: "+points.ToString();
            SaveScore(points);
        }

    }

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("Max Points",0);
    }

    public void SaveScore(int points)
    {
        PlayerPrefs.SetInt("Max Points", points);
    }
}
