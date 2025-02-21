using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class UIRectObject : MonoBehaviour
{
    [SerializeField] XROrigin _xrOrigin;

    private RectTransform rectangleRectTransform;
    private Image rectangleImage;
    private TMP_Text rectText;

    public void Awake()
    {
        rectangleRectTransform = GetComponent<RectTransform>();
        rectangleImage = GetComponent<Image>();
        rectText = GetComponentInChildren<TMP_Text>();
    }

    public void SetRectTransform(Rect rect)
    {
        rectangleRectTransform.anchoredPosition = new Vector2(rect.x, rect.y);
        rectangleRectTransform.sizeDelta = new Vector2(rect.width, rect.height);
    }

    public void SetColor(Color color)
    {
        rectangleImage.color = color;
    }

    public void SetText(string text)
    {
        rectText.text = text;
    }

    public RectTransform getRectTransform(){
        return rectangleRectTransform;
    }
}