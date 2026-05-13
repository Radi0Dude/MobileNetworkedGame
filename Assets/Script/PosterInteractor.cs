using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PosterInteractor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [HideInInspector]
    public string url;

    bool isHolding;
    [HideInInspector]
    public string showName;



    [SerializeField]
    GameObject hoverUI;

    GameObject currentAliveHoverUI;

    Coroutine hoverCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.IsScrolling()) return;
        hoverCoroutine = StartCoroutine(HoldCheck(eventData.position));

        isHolding = false;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentAliveHoverUI != null) { Destroy(currentAliveHoverUI); }
        if (eventData.IsScrolling()) return;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isHolding && !eventData.dragging)
        {
            Application.OpenURL(url);
        }
    }

    IEnumerator HoldCheck(Vector3 pos)
    {
        yield return new WaitForSeconds(.4f);
        isHolding = true;
        Debug.Log("Clicked on " + showName);
        // Show Hover UI
        if (currentAliveHoverUI == null)
        {
            currentAliveHoverUI = Instantiate(hoverUI, transform.root);
            RectTransform thisRect = GetComponent<RectTransform>();
            RectTransform currentAliveHoverUIRectTransform = currentAliveHoverUI.GetComponent<RectTransform>();
            if (thisRect.position.x > 500)
            {
                currentAliveHoverUIRectTransform.position = new Vector3(pos.x - currentAliveHoverUIRectTransform.rect.width / 2, pos.y, pos.z);
            }
            else
            {
                currentAliveHoverUIRectTransform.position = new Vector3(pos.x + currentAliveHoverUIRectTransform.rect.width / 2, pos.y, pos.z);
            }

            GetCorrectInfoOnHover getCor = currentAliveHoverUI.GetComponent<GetCorrectInfoOnHover>();
            getCor.SetInfo(showName, url);

        }
    }
}
