using System;
using UniRx;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Subject<Unit> onGameStartSubject = new Subject<Unit>();
    private Subject<Unit> onGamePauseSubject = new Subject<Unit>();
    private Subject<Unit> onGameUnPauseSubject = new Subject<Unit>();
    private Subject<Unit> onDieSubject = new Subject<Unit>();

    public IObservable<Unit> OnGameStartObservable => onGameStartSubject;
    public IObservable<Unit> OnGamePauseObservable => onGamePauseSubject;
    public IObservable<Unit> OnGameUnPauseObservable => onGameUnPauseSubject;
    public IObservable<Unit> OnDieObservable => onDieSubject;

    public void StartGame()
    {
        onGameStartSubject.OnNext(Unit.Default);
    }

    public void PauseGame()
    {

        onGamePauseSubject.OnNext(Unit.Default);
    }

    public void UnPauseGame()
    {
        Debug.Log("ゲームをアンポーズ");
        onGameUnPauseSubject.OnNext(Unit.Default);
    }

    public void Die()
    {
        onDieSubject.OnNext(Unit.Default);
    }
}
