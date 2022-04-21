using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameStateController : MonoBehaviour
{
    [Header("Game Variables")]
    [SerializeField] private int currentNumberOfInfected;
    [SerializeField] private int numberOfInfectedForGameOver;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameStateUI;
    [SerializeField] private TextMeshProUGUI resultUI;
    [SerializeField] private TextMeshProUGUI currentNumberOfInfectedUI;
    [SerializeField] private TextMeshProUGUI numberOfInfectedForGameOverUI;



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNumberOfInfected >= numberOfInfectedForGameOver)
        {
            UpdateGameStateUI(false);
        }
    }


    public void UpdateGameStateUI(bool gameResult)
    {
        gameStateUI.SetActive(true);
        Time.timeScale = 0;

        if (gameResult)
        {
            resultUI.text = "There are too much infected\nYou Lose";
        }
        else
        {
            resultUI.text = "You destoryed all the infected\nYou Win";
        }

    }

    public void changeNumberOfInfected(int amountToChangeBy)
    {
        currentNumberOfInfected += amountToChangeBy;
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
