using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage = 3;
    int currentPage = 1;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime = 0.5f;
    [SerializeField] LeanTweenType tweenType = LeanTweenType.easeInOutQuad;
    float dragThreshold;
    private Vector3 targetPos;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshold = Screen.width / 15;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos.x -= 720; // Перемещение на 720 пикселей влево
            MovePage();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos.x += 720; // Перемещение на 720 пикселей вправо
            MovePage();
        }
    }

    void MovePage()
    {
        LeanTween.cancel(levelPagesRect); // Отменяем любые текущие анимации для предотвращения конфликтов
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;

        if (Mathf.Abs(difference) > dragThreshold)
        {
            if (difference > 0) // Свайп влево
            {
                Next();
            }
            else // Свайп вправо
            {
                Previous();
            }
        }
        else
        {
            // Остаемся на текущей странице
            targetPos.x = (currentPage - 1) * -720;
            MovePage();
        }
    }
}
