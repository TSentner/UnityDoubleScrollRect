using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(ScrollRect))]
public class SecondScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ScrollRect OtherScrollRect;
    private ScrollRect _myScrollRect;
    //This tracks if the other one should be scrolling instead of the current one.
    private bool scrollOther;
    //This tracks wether the other one should scroll horizontally or vertically.
    private bool scrollOtherHorizontally;

    void Awake()
    {
        //Получаем текущий scrollRect чтобы отключать его если он не должен быть использован
        _myScrollRect = this.GetComponent<ScrollRect>();
        //если этот скролл вертикальный другой должен быть горизонтальным.
        scrollOtherHorizontally = _myScrollRect.vertical;
        //Даём обратку если это не так.
        if (scrollOtherHorizontally)
        {
            if (_myScrollRect.horizontal)
                Debug.Log("You have added the SecondScrollRect to a scroll view that already has both directions selected");
            if (!OtherScrollRect.horizontal)
                Debug.Log("The other scroll rect doesnt support scrolling horizontally");
        }
        else if (!OtherScrollRect.vertical)
        {
            Debug.Log("The other scroll rect doesnt support scrolling vertically");
        }
    }
    //IBeginDragHandler
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Определяем длину движения по x и y, через разницу начальной и конечной точек
        float horizontal = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
        float vertical = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
        if (scrollOtherHorizontally)
        {
            if (horizontal > vertical)
            {
                scrollOther = true;
                //двигая внешний отключаем движение внутреннего
                _myScrollRect.enabled = false;
                OtherScrollRect.OnBeginDrag(eventData);
            }
        }
        else if (vertical > horizontal)
        {
            scrollOther = true;
            //двигая внешний отключаем движение внутреннего
            _myScrollRect.enabled = false;
            OtherScrollRect.OnBeginDrag(eventData);
        }
    }
    //IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrollOther)
        {
            scrollOther = false;
            _myScrollRect.enabled = true;
            OtherScrollRect.OnEndDrag(eventData);
        }
    }
    //IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        if (scrollOther)
        {
            OtherScrollRect.OnDrag(eventData);
        }
    }
}