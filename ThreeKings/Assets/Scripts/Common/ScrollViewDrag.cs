using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scroll;
    public Transform content;
    private Vector2 startDrag;
    int currentPage = 0;
    public System.Action<int> onPageChanged;

    public void Start()
    {
        //ScrollToPage(currentPage);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 endDrag = eventData.position;
        float dragDistance = endDrag.x - startDrag.x;

        if (Mathf.Abs(dragDistance) > Screen.width / 4)
        {
            if (dragDistance > 0 && currentPage > 1)
            {
                ScrollToPageSmooth(currentPage - 1);
            }
            else if (dragDistance < 0 && currentPage < content.transform.childCount)
            {
                ScrollToPageSmooth(currentPage + 1);
            }
        }
        else
        {
            ScrollToPageSmooth(currentPage);
        }
    }

    public void ScrollToPage(int pageNumber)
    {
        float location = (pageNumber - 1) / (float)(content.transform.childCount - 1);
        //scroll.horizontalNormalizedPosition = location;
        //float location = pageNumber / (float)content.transform.childCount;
        scroll.horizontalNormalizedPosition = location;
        currentPage = pageNumber;
        onPageChanged?.Invoke(currentPage);
    }

    public void ScrollToPageSmooth(int pageNumber)
    {
        //float location = pageNumber / (float)content.transform.childCount;
        float location = (pageNumber - 1) / (float)(content.transform.childCount - 1);
        DOTween.To(() => scroll.horizontalNormalizedPosition, x => scroll.horizontalNormalizedPosition = x, location, 0.3f).OnComplete(() => {
            currentPage = pageNumber;
            onPageChanged?.Invoke(currentPage);
        });
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDrag = eventData.position;
    }
    public int GetCurrentPage()
    {
        return currentPage;
    }
}
