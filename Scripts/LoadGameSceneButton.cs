using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private MMF_Player feedback;
    [SerializeField] private MMF_Player musicFeedback;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("SceneLoaderAsync: Scene name is not set or is empty.");
            yield break;
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        if (asyncOperation == null)
        {
            Debug.LogError($"SceneLoaderAsync: Failed to load scene '{sceneName}'. Make sure the scene is added to Build Settings.");
            yield break;
        }

        asyncOperation.allowSceneActivation = false;
        feedback?.PlayFeedbacks();
       // musicFeedback?.StopFeedbacks();
        SceneManager.LoadScene(sceneName);
    }
}