using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Text loadingText; // Перетащите сюда UI элемент для отображения статуса загрузки
    public Slider loadingSlider; // Перетащите сюда UI элемент для отображения прогресса загрузки

    private void Start()
    {
        Debug.Log("Start method called.");

        // Проверка на null для loadingText и loadingSlider
        if (loadingText == null)
        {
            Debug.LogError("Loading Text is not assigned in the inspector!");
            return;
        }

        if (loadingSlider == null)
        {
            Debug.LogError("Loading Slider is not assigned in the inspector!");
            return;
        }

        Debug.Log("Loading Text and Slider assigned successfully.");

        // Запустите корутину для загрузки основной сцены
        StartCoroutine(LoadMainMenuAsync());
    }

    private IEnumerator LoadMainMenuAsync()
    {
        Debug.Log("Starting to load the Main scene asynchronously.");

        // Начинаем асинхронную загрузку основной сцены
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false; // Пока загрузка не завершена, не активируем сцену

        if (operation == null)
        {
            Debug.LogError("Failed to start scene loading operation.");
            yield break;
        }

        float displayProgress = 0f;
        float toProgress = 0f;

        // Пока сцена не загружена, обновляем текст и ползунок загрузки
        while (!operation.isDone)
        {
            // Если прогресс загрузки меньше 90%, обновляем ползунок и текст
            if (operation.progress < 0.9f)
            {
                toProgress = operation.progress / 0.9f;
            }
            else
            {
                // Если прогресс загрузки достиг 90%, начинаем финальную стадию загрузки
                toProgress = 1f;
            }

            // Плавное обновление отображаемого прогресса
            while (displayProgress < toProgress)
            {
                displayProgress += Time.deltaTime;
                if (displayProgress > toProgress)
                {
                    displayProgress = toProgress;
                }

                if (loadingSlider != null)
                {
                    loadingSlider.value = displayProgress;
                }

                if (loadingText != null)
                {
                    loadingText.text = "Loading... " + (int)(displayProgress * 100) + "%";
                }

                yield return null;
            }

            // Если прогресс достиг 100%, активируем сцену
            if (toProgress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log("Main scene loaded successfully.");
    }
}
