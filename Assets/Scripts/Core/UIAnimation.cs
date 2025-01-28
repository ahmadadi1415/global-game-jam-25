using System;
using UnityEngine;

public static class UIAnimation
{
    public static readonly float BUTTON_CLICKED_DURATION = 0.2f;

    public static LTDescr Pop(GameObject target)
    {
        return Pop(target, 1.1f, BUTTON_CLICKED_DURATION);
    }

    public static LTDescr Pop(GameObject target, float scale, float duration)
    {
        LeanTween.cancel(target);
        return LeanTween
                .scale(target, new Vector3(scale, scale, 1f), duration / 2)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeOutQuint)
                .setOnComplete(() =>
                {
                    LeanTween
                            .scale(target, new Vector3(1f, 1f, 1f), duration / 2)
                            .setIgnoreTimeScale(true)
                            .setEase(LeanTweenType.easeOutBack);
                });
    }

    public static LTDescr SlideIn(GameObject target, Vector3 src, Vector3 dst, float duration, bool ignoreTimeScale = true)
    {
        LeanTween.cancel(target);
        LeanTween.moveLocal(target, src, 0);
        return LeanTween.moveLocal(target, dst, duration).setEase(LeanTweenType.easeOutCirc).setIgnoreTimeScale(ignoreTimeScale);
    }

    public static LTDescr SlideOut(GameObject target, Vector3 src, Vector3 dst, float duration, bool ignoreTimeScale = true)
    {
        LeanTween.cancel(target);
        LeanTween.moveLocal(target, src, 0);
        return LeanTween.moveLocal(target, dst, duration).setEase(LeanTweenType.easeInCubic).setIgnoreTimeScale(ignoreTimeScale);
    }

    public static LTDescr ZoomIn(GameObject target, float duration, bool ignoreTimeScale = true)
    {
        LeanTween.cancel(target);
        LeanTween.scale(target, new Vector3(0, 0, 1), 0);
        return LeanTween.scale(target, new Vector3(1, 1, 1), duration).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(ignoreTimeScale);
    }

    public static LTDescr ZoomIn(RectTransform target, float duration, bool ignoreTimeScale = true)
    {
        LeanTween.cancel(target);
        LeanTween.scale(target, new Vector3(0, 0, 1), 0);
        return LeanTween.scale(target, new Vector3(1, 1, 1), duration).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(ignoreTimeScale);
    }

    public static LTDescr ZoomOut(GameObject target, float duration, bool ignoreTimeScale = true)
    {
        LeanTween.cancel(target);
        LeanTween.scale(target, new Vector3(1, 1, 1), 0);
        return LeanTween.scale(target, new Vector3(0, 0, 1), duration).setEase(LeanTweenType.easeOutQuart).setIgnoreTimeScale(ignoreTimeScale);
    }

    public static LTDescr ZoomOut(RectTransform target, float duration, bool ignoreTimeScale = true)
    {
        LeanTween.cancel(target);
        LeanTween.scale(target, new Vector3(1, 1, 1), 0);
        return LeanTween.scale(target, new Vector3(0, 0, 1), duration).setEase(LeanTweenType.easeOutQuart).setIgnoreTimeScale(ignoreTimeScale);
    }
}