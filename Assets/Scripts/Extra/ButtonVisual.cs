using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.15f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        enabled = false;
        StartCoroutine(ResetFlag());
    }

    private IEnumerator ResetFlag()
    {
        yield return new WaitForSeconds(0.05f);

        enabled = true;
    }
}
