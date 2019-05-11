using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject game;
    private Animator animator;
    public GameObject enemyGenerator;
    private AudioSource audioPlayer;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;

    private float startY;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        bool inGrounded = transform.position.y == startY;
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameController.GameState.Playing;
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);

        if (gamePlaying && inGrounded && userAction)
        {
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameController.GameState.Ended;
            enemyGenerator.SendMessage("StopGenerator",true);
            game.SendMessage("ResetTimeScale",0.5f);
            dustStop();

            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();


        }else if(other.gameObject.tag == "Point")
        {
            game.SendMessage("IncreasePoint");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }

    }

    void GameReady()
    {
        game.GetComponent<GameController>().gameState = GameController.GameState.Ready;
    }

    void dustPlay()
    {
        dust.Play();
    
    }

    void dustStop()
    {
        dust.Stop();
    }
}
