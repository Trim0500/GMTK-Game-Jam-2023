using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controlsMenu;
    public GameObject creditsMenu;
    public GameObject mainMenuDefaultSelected;
    public GameObject controlsDefaultSelected;
    public GameObject creditsDefaultSelected;
    public EventSystem eventSystem;
    public GameObject backEffect;
    public GameObject clickSoundEffectPrefab;
    public GameObject navigationSoundEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonOnClick()
    {
        SceneManager.LoadScene("Main_Game");
    }

    public void ControlsButtonOnClick()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(controlsDefaultSelected);
    }

    public void CreditsButtononClick()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(creditsDefaultSelected);
    }

    public void QuitButtonOnClick()
    {
        if (Application.isPlaying && !Application.isEditor)
        {
            Application.Quit();
        }
        #if false //To use it in the editor, set to true
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        #endif
    }

    public void BackButtonControlsOnClick()
    {
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(mainMenuDefaultSelected);
    }

    public void BackButtonCreditsOnClick()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(mainMenuDefaultSelected);
    }

    public void CreateBackEffect()
    {
        if (backEffect)
        {
            Instantiate(backEffect, transform.position, Quaternion.identity, null);
        }
    }

    /// <summary>
    /// Description:
    /// Creates a click effect if one is set
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    public void CreateClickEffect()
    {
        Debug.Log("Creating effect...");

        if (clickSoundEffectPrefab)
        {
            Instantiate(clickSoundEffectPrefab, transform.position, Quaternion.identity, null);
        }
    }

    /// <summary>
    /// Description:
    /// Creates a navigation effect if one is set
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    public void CreateNavigationEffect()
    {
        Debug.Log("Creating effect...");

        if (navigationSoundEffectPrefab)
        {
            Instantiate(navigationSoundEffectPrefab, transform.position, Quaternion.identity, null);
        }
    }
}
