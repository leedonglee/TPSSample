using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    // [SerializeField] Image progressBar;

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName) 
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene() 
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone) 
        {
            yield return null;
            timer += 1f;
            if (timer > 3f)
            {
                if (op.progress >= 0.9f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }                
            }
            /*
            if (op.progress < 0.9f) 
            {                
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress) 
                {
                    timer = 0f; 
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f) 
                {
                    op.allowSceneActivation = true;
                    yield break;
                }                
            }
            */
            yield return new WaitForSeconds(1f);
        }

    }

}