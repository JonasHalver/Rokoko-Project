using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool allowLoad = false, loadingStarted = false;
    bool wannaSmash;

    // Update is called once per frame
    void Update()
    {
        if (wannaSmash && !loadingStarted)
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        loadingStarted = true;
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                if (allowLoad)
                    asyncOperation.allowSceneActivation = true;
                else
                    print("Ready");
            }
            yield return null;
        }
    }

    public void WannaSmash()
    {
        wannaSmash = true;
    }

    public void Smash()
    {
        allowLoad = true;
    }

    public void DontSmash()
    {
        Application.Quit();
    }
}
