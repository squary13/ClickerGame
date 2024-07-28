using UnityEngine;
using UnityEngine.UI;

public class PixelLoadingManager : MonoBehaviour
{
    public Image loadingBar; // Это изображение заполненного ползунка
    public float loadingSpeed = 0.1f; // Скорость заполнения

    private float targetFillAmount;
    private float currentFillAmount;

    void Start()
    {
        if (loadingBar == null)
        {
            Debug.LogError("Loading Bar is not assigned in the inspector!");
            return;
        }

        targetFillAmount = 0f;
        currentFillAmount = 0f;
        loadingBar.fillAmount = 0f; // Убедитесь, что начальное значение заполнения равно 0
    }

    void Update()
    {
        // Обновление текущего заполнения
        if (currentFillAmount < targetFillAmount)
        {
            currentFillAmount += loadingSpeed * Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, currentFillAmount, 0.5f);
        }

        // Для тестирования, удалите этот блок, когда будет использоваться реальный прогресс загрузки
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetFillAmount += 0.1f;
            if (targetFillAmount > 1f) targetFillAmount = 1f;
        }
    }

    // Этот метод можно вызывать для обновления загрузки
    public void UpdateLoadingBar(float progress)
    {
        targetFillAmount = progress;
    }
}
