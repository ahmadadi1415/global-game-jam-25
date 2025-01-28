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

    private Vector3 _bubblePosition;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Init(Vector3 t, BubbleState state, Vector2 offset)
    {
        _bubblePosition = t;
        transform.position = t + (Vector3)offset;

        switch (state)
        {
            case BubbleState.UNPOPPED:
                SetUnpopped();
                break;
            case BubbleState.POPPED:
                SetPopped();
                break;
            default:
                break;
        }
    }

    public Vector3 GetTilePosition()
    {
        return _bubblePosition;
    }

    public void StartPop()
    {
        // DO: Prevent popping if already popped
        if (State == BubbleState.POPPED) return;

        SetState(BubbleState.POPPING);
        // spriteRenderer.color = Color.red;
        animator.SetTrigger("popping");
        // EventManager.Publish<OnBubblePoppedMessage>(new() { BubblePosition = transform.position });
        LeanTween.delayedCall(0.5f, () =>
        {
            SetState(BubbleState.POPPED);
        });
    }

    public void SetPopped()
    {
        SetState(BubbleState.POPPED);
        // if (spriteRenderer != null) spriteRenderer.color = Color.black;
        LeanTween.delayedCall(RandomizeStartDelay(), () =>
        {
            animator.SetTrigger("popped");
            // EventManager.Publish<OnBubblePoppedMessage>(new() { BubblePosition = transform.position });
        });
    }

    public void SetUnpopped()
    {
        SetState(BubbleState.UNPOPPED);
        LeanTween.delayedCall(RandomizeStartDelay(), () => animator.SetTrigger("unpopped"));
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

    private float RandomizeStartDelay() => UnityEngine.Random.Range(0, 0.6f);
}
