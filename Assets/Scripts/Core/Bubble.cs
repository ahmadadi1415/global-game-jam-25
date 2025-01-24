using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private bool isDestroyed = false;

    private void OnMouseDown()
    {
        if (isDestroyed) return; // Prevent double clicks
        DestroyBubble();
    }

    void DestroyBubble()
    {
        isDestroyed = true;
        GameManager.Instance.BubbleDestroyed();
        Destroy(gameObject); // Destroy the bubble
    }
}

