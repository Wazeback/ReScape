using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject Background;
    public GameObject PauseMenuUI;
    public GameObject SettingsMenuUI;
    public GameObject ControlsMenuUI;
    public GameObject PlayerCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    void Resume()
    {
        Background.SetActive(false);
        PauseMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(false);
        ControlsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerCamera.GetComponent<PlayerCam>().paused = false;
        PlayerCamera.GetComponent<HandleCursor>().paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        Background.SetActive(true);
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlayerCamera.GetComponent<PlayerCam>().paused = true;
        PlayerCamera.GetComponent<HandleCursor>().paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
