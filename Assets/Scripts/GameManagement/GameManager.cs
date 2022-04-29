using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMove player;
    [SerializeField] private PaddleObject paddle;

    private static int score; public static int Score { get { return score; } }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PaddleObject.PointScored += PointScored;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PointScored()
    {
        score++;
    }
}
