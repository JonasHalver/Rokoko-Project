using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused;
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            paused = !paused;

        menu.SetActive(paused);

        if (paused)
            Time.timeScale = 0.1f;
        else
            Time.timeScale = 1;
    }

    public void Continue()
    {
        paused = false;
    }

    public void Reset()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
