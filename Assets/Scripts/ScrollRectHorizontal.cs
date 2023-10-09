using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectHorizontal : MonoBehaviour
{
    public ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Scroll(1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Scroll(-1);
        }
    }

    public void Scroll(int scrollFactor)
    {
        if (scrollRect != null)
        {
            if (((scrollRect.horizontalNormalizedPosition <= 1f) && (scrollFactor >= 0)) || ((scrollRect.horizontalNormalizedPosition >= 0f && (scrollFactor <= 0))))
            {
                scrollRect.horizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition + (scrollFactor * scrollSpeed);
            }
        }
    }
}
