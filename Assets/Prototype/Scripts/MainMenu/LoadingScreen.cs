using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Transform loadingIcon;
    public float rotationSpeed = 100f;

    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);
        gameLevel.allowSceneActivation = false;

        while (!gameLevel.isDone)
        {
            float progress = Mathf.Clamp01(gameLevel.progress / 0.9f); // 0.9 is the maximum value progress reaches

            loadingIcon.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime); // Rotate the loading icon

            if (progress >= 0.9f) // If the loading is almost complete
            {
                gameLevel.allowSceneActivation = true; // Activate the loaded scene
            }

            yield return null;
        }
    }
}
