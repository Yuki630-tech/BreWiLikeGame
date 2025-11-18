using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    [Header("アニメーションイベントとして登録するイベント"), SerializeField] private UnityEvent animationEvent;

    public void Invoke()
    {
        animationEvent.Invoke();
    }
}
