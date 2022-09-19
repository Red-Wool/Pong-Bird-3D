using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMove player;
    [SerializeField] private PaddleObject paddle;
    [SerializeField] private GameObject spawnPlatform;

    private static bool gameStart; public static bool GameStart { get { return gameStart; } }
    private static int score; public static int Score { get { return score; } }

    public delegate void GameOverHandler(int score);
    public static event GameOverHandler GameOver;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PaddleObject.PointScored += PointScored;

        PlayerMove.death.AddListener(ResetGame);
        gameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        
        score = 0;
        gameStart = true;

        player.Heal();

        spawnPlatform.transform.DOKill();
        spawnPlatform.transform.DOMoveY(-100f, 10f).SetEase(Ease.InOutSine).OnComplete(() => spawnPlatform.SetActive(false));
    }

    public void ResetGame()
    {
        gameStart = false;

        

        spawnPlatform.SetActive(true);
        spawnPlatform.transform.DOKill();
        spawnPlatform.transform.DOMoveY(0, 4f).SetEase(Ease.InOutSine);
    }

    private void PointScored()
    {
        if (gameStart)
        {
            score++;
        }
        else
        {
            StartGame();
        }
    }
}
