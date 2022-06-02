using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static bool created = false;
    private AsyncOperation operation;
    public GameObject logo;

    void Start()
    {
        if (created == false)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator LoadAsync(string scene)
    {
        logo.SetActive(true);
        while (this.GetComponent<CanvasGroup>().alpha < 1)
        {
            this.GetComponent<CanvasGroup>().alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(3f);
        this.GetComponent<CanvasGroup>().alpha = 1;
        operation = SceneManager.LoadSceneAsync(scene);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        while (this.GetComponent<CanvasGroup>().alpha > 0)
        {
            this.GetComponent<CanvasGroup>().alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        this.GetComponent<CanvasGroup>().alpha = 0;
        logo.SetActive(false);
    }
}
