using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int money;
    public int total_money;
    public Text moneyText;
    public AudioSource audioSource;

    private void Start(){
    audioSource = GetComponent<AudioSource>();
    money = PlayerPrefs.GetInt("money");
    total_money = PlayerPrefs.GetInt("total_money");
    }

    public void ButtonClick() {
    money++;
    total_money++;
    PlayerPrefs.SetInt("money", money);
    PlayerPrefs.SetInt("total_money", total_money);
    audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        moneyText.text = money.ToString();
    }
}
