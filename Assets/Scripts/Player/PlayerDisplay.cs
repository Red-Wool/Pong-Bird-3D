using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private PlayerStats stat;

    [SerializeField] private Transform playerUITrack;
    [SerializeField] private Transform track;
    [SerializeField] private float trackSpeed;

    [SerializeField] private StoredValue hp;
    [SerializeField] private StoredValue jump;
    [SerializeField] private StoredValue stamina;
    [SerializeField] private StoredValue coin;

    [SerializeField] private Image health;

    [SerializeField] private GameObject staminaHold;
    [SerializeField] private Image staminaBar;
    
    [SerializeField] private GameObject[] feathers;
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text hpDisplay;
    [SerializeField] private TMP_Text coinDisplay;
    [SerializeField] private float barSpeed;

    private Vector3 curTrackPos;

    private Camera cam;

    private Vector3 coinTextPos;
    private IEnumerator coinAppear;

    // Start is called before the first frame update
    void Start()
    {
        coinTextPos = coinDisplay.transform.position;
        coinDisplay.text = "Lemonade: " + coin.value;
        PlayerMove.death.AddListener(UpdateCoin);

        cam = Camera.main;
        curTrackPos = cam.WorldToScreenPoint(track.position);
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = Mathf.Lerp(health.fillAmount, hp.value / stat.maxHP, barSpeed * Time.deltaTime);

        staminaBar.fillAmount = Mathf.Lerp(staminaBar.fillAmount, stamina.value / stat.maxStamina, barSpeed * Time.deltaTime);
        staminaHold.SetActive(stamina.value <= stat.maxStamina * .99f);

        scoreDisplay.text = GameManager.Score.ToString();
        hpDisplay.text = "HP: " + hp.value;

        curTrackPos = Vector3.Lerp(curTrackPos, cam.WorldToScreenPoint(track.position), trackSpeed * Time.deltaTime);
        playerUITrack.transform.position = curTrackPos;

        //if ()
        

        for (int i = 0; i < 3; i++)
        {
            bool check = i < jump.value;
            feathers[i].SetActive(check);
            if (!check) { break; }

            Vector3 position = feathers[i].transform.localPosition;
            position.y = Mathf.Sin(i * 2f + Time.time) * 1f;

            feathers[i].transform.localPosition = position;
        }
    }

    public void CoinGet(int num)
    {
        coinDisplay.transform.DOJump(coinTextPos, 10f, num, .15f*num);
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        coinDisplay.text = "Lemonade: " + coin.value;
        
        
        if (coinAppear != null)
            StopCoroutine(coinAppear);

        coinAppear = CoinAppear();
        StartCoroutine(coinAppear);
    }

    public IEnumerator CoinAppear()
    {
        CoinTextVisibility(true);
        yield return new WaitForSeconds(2f);

        Debug.Log(GameManager.GameStart + " a dw");
        if (GameManager.GameStart)
            CoinTextVisibility(false);
    }

    public void CoinTextVisibility(bool flag)
    {
        Debug.Log(flag);
        coinDisplay.DOFade(flag ? 1 : 0, .1f);
    }
}
