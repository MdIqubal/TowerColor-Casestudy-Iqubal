using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PnC.CasualGameKit;

//TODO :
///1. editor
//2. tick tween
//3. exit tween
public class ScreenTweener : MonoBehaviour, IScreenTweener
{
    public List<TweenEntity> tweenEntities;

    private void OnEnable()
    {
        Tween();
    }

    public void Tween()
    {
        //transform.dom
        for (int i = 0; i < tweenEntities.Count; i++)
        {
            TweenEntity entity = tweenEntities[i];
            if (entity.TweenPosition)
            {
                StartCoroutine(TweenPosition(entity));

            }
            if (entity.TweenRotation)
            {
                StartCoroutine(TweenRotation(entity));
            }
            if (entity.TweenScale)
            {
                StartCoroutine(TweenScale(entity));
            }
        }
    }



   IEnumerator TweenPosition(TweenEntity entity) {
        entity.RectTransform.position = entity.StartPosition;
        yield return new WaitForSeconds(entity.delay);
        entity.RectTransform.DOMove(entity.FinalPosition, entity.Duration).SetEase(entity.Ease);
    }

    IEnumerator TweenRotation(TweenEntity entity)
    {
        entity.RectTransform.eulerAngles = Vector3.forward * entity.StartRotation;
        yield return new WaitForSeconds(entity.delay);
        entity.RectTransform.DORotate(Vector3.forward * entity.FinalRotation, entity.Duration).SetEase(entity.Ease);
    }

    IEnumerator TweenScale(TweenEntity entity)
    {
        entity.RectTransform.localScale = entity.StartScale;
        yield return new WaitForSeconds(entity.delay);
        entity.RectTransform.DOScale(entity.FinalScale, entity.Duration).SetEase(entity.Ease);
    }


    //TODO : property drawer
    [System.Serializable]
    public class TweenEntity
    {
        [Header("TO DO : property drawer")]
        [Space]
        public RectTransform RectTransform;
        public Ease Ease;
        public float Duration;
        public float delay;

        [Header("Position")]
        public bool TweenPosition;
        public Vector3 StartPosition;
        public Vector3 FinalPosition;

        [Header("Rotation")]
        public bool TweenRotation;
        public float StartRotation;
        public float FinalRotation;

        [Header("Scale")]
        public bool TweenScale;
        public Vector3 StartScale = Vector3.one;
        public Vector3 FinalScale = Vector3.one;

    }
}