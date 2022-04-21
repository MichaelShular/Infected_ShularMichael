using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GameStateController : MonoBehaviour
{
    [Header("Game Variables")]
    [SerializeField] private int currentNumberOfInfected;
    [SerializeField] private int numberOfInfectedForGameOver;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameStateUI;
    [SerializeField] private GameObject pauseUI;

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
        if(currentNumberOfInfected == 0)
        {
            UpdateGameStateUI(true);

        }
    }


    public void UpdateGameStateUI(bool gameResult)
    {
        gameStateUI.SetActive(true);
        Time.timeScale = 0;

        if (gameResult)
        {
           resultUI.text = "You destoryed all the infected\nYou Win";
        }
        else
        { 
            resultUI.text = "There are too much infected\nYou Lose";
        }

    }

    public void changeNumberOfInfected(int amountToChangeBy)
    {
        currentNumberOfInfected += amountToChangeBy;
        currentNumberOfInfectedUI.text = currentNumberOfInfected.ToString();
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnContinuePressed()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }


    public void OnPause(InputValue value)
    {
        Debug.Log("a");
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }
}
