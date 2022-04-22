using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField]
    public GameObject instructionUI;
    [SerializeField]
    public GameObject creditsUI;

    public void OnStartGamePressed()
    {
        SceneManager.LoadScene("Game");
    }


    public void OnQuitPressed()
    {
        Application.Quit();
    }


    public void ToggleInsturtcions()
    {
        instructionUI.SetActive(!instructionUI.activeSelf);
    }

    public void ToggleCredits()
    {
        creditsUI.SetActive(!creditsUI.activeSelf);
    }
}
