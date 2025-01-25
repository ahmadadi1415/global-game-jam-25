using System;
using UnityEngine;

public enum BubbleState { UNPOPPED, POPPING, POPPED }

public class GridTileBase : MonoBehaviour
{
    // public static event Action<OnGridClickEvent> OnGridClick;
    public BubbleState State = BubbleState.UNPOPPED;
    public struct OnGridClickEvent
    {
        public GridTileBase GridTileBase;
    }

    private Vector3 _position;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector3 t)
    {
        transform.position = t;
        _position = t;
    }

    public Vector3 GetPositionTile()
    {
        return _position;
    }

    public void StartPop()
    {
        // DO: Prevent popping if already popped
        if (State == BubbleState.POPPED) return;

        SetState(BubbleState.POPPING);
        spriteRenderer.color = Color.red;
        LeanTween.delayedCall(0.5f, SetPopped);
    }

    public void SetPopped()
    {
        SetState(BubbleState.POPPED);
        if (spriteRenderer != null) spriteRenderer.color = Color.black;
    }

    public void SetUnpopped()
    {
        SetState(BubbleState.UNPOPPED);
    }

    private void SetState(BubbleState state)
    {
        // DO: Play animation and set state
        State = state;
    }

    public void Destroy()
    {
        // Debug.Log("Destryo" + GetPositionTile().ToString());
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        // OnGridClick?.Invoke(new OnGridClickEvent { GridTileBase = this });
        if (State != BubbleState.UNPOPPED) return;
        EventManager.Publish<OnBubbleClickedMessage>(new() { Bubble = this });
    }
}
