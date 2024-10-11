using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRect : MonoBehaviour
{
    [SerializeField] private GameObject rectanglePrefab;

    private List<UIRectObject> rectObjects = new();
    private List<int> openIndices = new();

    public void CreateRect(Rect rect, Color color, string text)
    {
        if(openIndices.Count == 0)
        {
            var newRect = Instantiate(rectanglePrefab, parent: transform).GetComponent<UIRectObject>();
            openIndices.Add(rectObjects.Count - 1);
        }

        int index = openIndices[0];

        openIndices.Remove(item: 0);

        UIRectObject rectObject = rectObjects[index];
        rectObject.SetRectTranfsorm(rect);
        rectObject.SetText(text);
        rectObject.gameObject.SetActive(true);
    }

    public void ClearRects()
    {
        for (int i = 0; i < rectObjects.Count; i++)
        {
            rectObjects[i].gameObject.SetActive(false);
            openIndices.Add(i);
        }
    }
}
