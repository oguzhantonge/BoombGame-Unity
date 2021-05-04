using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject playGuidePanel;
  
    public void ClickHowToPlayButton()
    {
        mainPanel.SetActive(false);
        playGuidePanel.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackButton()
    {
        mainPanel.SetActive(true);
        playGuidePanel.SetActive(false);
    }

} // Class

