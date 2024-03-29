﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private PlayerStats stat;

    [SerializeField] private StoredValue hp;
    [SerializeField] private StoredValue jump;
    [SerializeField] private StoredValue stamina;
    [SerializeField] private StoredValue coin;

    [SerializeField] private Image staminaBar;
    [SerializeField] private GameObject[] feathers;
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text hpDisplay;
    [SerializeField] private TMP_Text coinDisplay;
    [SerializeField] private float barSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        staminaBar.fillAmount = Mathf.Lerp(staminaBar.fillAmount, stamina.value / stat.maxStamina, barSpeed * Time.deltaTime);

        scoreDisplay.text = GameManager.Score.ToString();
        hpDisplay.text = "HP: " + hp.value;
        coinDisplay.text = "Yellow lemonade: " + coin.value;

        for (int i = 0; i < 3; i++)
        {
            bool check = i < jump.value;
            feathers[i].SetActive(check);
            if (!check) { break; }

            Vector3 position = feathers[i].transform.localPosition;
            position.y = Mathf.Sin(i * 2f + Time.time) * 5f;

            feathers[i].transform.localPosition = position;
        }
    }
}
