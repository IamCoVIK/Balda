using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollFit : MonoBehaviour
{
    [SerializeField] RectTransform content;
    [SerializeField] RectTransform text;

    void Update()
    {
        Vector2 size = content.sizeDelta;
        size.y = text.sizeDelta.y;
        content.sizeDelta = size;
    }
}
