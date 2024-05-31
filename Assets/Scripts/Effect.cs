using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Effect : MonoBehaviour
{
    private Vector2 orignalScale;
    private Vector2 scaleTo;

    private Vector3 orignalRotate;
    private Vector3 currentRot;
    [SerializeField] Ease ease;
    [SerializeField] Vector3 finalRot;


    public void ChangeScale()
    {
        orignalScale = transform.localScale;
        scaleTo = orignalScale * 1.4f;
        transform.DOScale(scaleTo, 1.4f);
        OnScale();
    }

   public void OnScale()
    {
        transform.DOScale(scaleTo, 1.4f).
            SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DOScale(orignalScale, 1.4f);
               // SetEase(Ease.OutBounce)
              // .SetDelay(1.0f)
              // .OnComplete(OnScale);
            });

    }

    public void Rotate()
    {

        orignalRotate = transform.rotation.eulerAngles;
        currentRot = finalRot;
        OnRotate(currentRot);
    }

    private void OnRotate(Vector3 angle)
    {
        transform.DORotate(angle, 1f).SetEase(ease).OnComplete(() => OnRotate(currentRot));
        currentRot = orignalRotate;


    }
}
