using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Image loadingBar; // Заполненный ползунок
    public float loadingSpeed = 0.1f; // Скорость заполнения (если нужно медленное заполнение)
    public float fakeLoadingTime = 3.0f; // Искусственное время загрузки для демонстрации
    public Image transitionImage; // Переходный черный экран
    public float transitionDuration = 1.0f; // Длительность анимации перехода

    private float currentFillAmount;

    void Start()
    {
        if (loadingBar == null)
        {
            Debug.LogError("Loading Bar is not assigned in the inspector!");
            return;
        }

        if (transitionImage == null)
        {
            Debug.LogError("Transition Image is not assigned in the inspector!");
            return;
        }

        // Убедитесь, что TransitionImage имеет начальную прозрачность
        Color startColor = transitionImage.color;
        startColor.a = 0f; // Начальная прозрачность
        transitionImage.color = startColor;

        currentFillAmount = 0f;
        loadingBar.fillAmount = 0f;

        // Запускаем процесс загрузки сцены
        StartCoroutine(LoadingProcess("Main"));
    }

    void Update()
    {
        // Обновление текущего заполнения
        if (currentFillAmount < 1f)
        {
            currentFillAmount += loadingSpeed * Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentFillAmount, 0.5f);
        }
    }

    private IEnumerator LoadingProcess(string sceneName)
    {
        // Заполняем индикатор загрузки
        yield return StartCoroutine(FakeLoadingBar());

        // Реальная загрузка сцены в фоне
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Отключаем автоматическую активацию

        // Затем показываем черный экран
        yield return StartCoroutine(FadeInTransition());

        // Проверяем, что сцена загружена
        while (!operation.isDone)
        {
            // Условие для активации сцены
            if (operation.progress >= 0.9f) // Значение близкое к 1
            {
                // Начинаем исчезновение затемнения
                yield return StartCoroutine(FadeOutTransition());

                // Разрешаем активацию сцены после завершения анимации перехода
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator FakeLoadingBar()
    {
        // Симуляция заполнения индикатора загрузки
        float elapsedTime = 0f;
        while (elapsedTime < fakeLoadingTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / fakeLoadingTime);
            currentFillAmount = progress;
            yield return null;
        }

        // Убедитесь, что ползунок заполняется до конца
        currentFillAmount = 1f;
        loadingBar.fillAmount = 1f;
    }

    private IEnumerator FadeInTransition()
    {
        Debug.Log("Starting FadeInTransition");
        float elapsedTime = 0f;
        Color startColor = transitionImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Непрозрачный цвет

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            transitionImage.color = Color.Lerp(startColor, endColor, elapsedTime / transitionDuration);
            yield return null;
        }

        // Убедитесь, что цвет окончательно стал непрозрачным
        transitionImage.color = endColor;
        Debug.Log("Finished FadeInTransition");
    }

    private IEnumerator FadeOutTransition()
    {
        Debug.Log("Starting FadeOutTransition");
        float elapsedTime = 0f;
        Color startColor = transitionImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Прозрачный цвет

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            transitionImage.color = Color.Lerp(startColor, endColor, elapsedTime / transitionDuration);
            yield return null;
        }

        // Убедитесь, что цвет окончательно стал прозрачным
        transitionImage.color = endColor;
        Debug.Log("Finished FadeOutTransition");
    }
}
