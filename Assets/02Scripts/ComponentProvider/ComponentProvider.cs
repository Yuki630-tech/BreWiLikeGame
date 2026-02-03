using UniRx;
using UnityEngine;

public class ComponentProvider : Singleton<ComponentProvider>
{
    [Tooltip("EnemyDetecter"), SerializeField] private EnemyDetecterForLockOn enemyDetecter;
    [Tooltip("ƒvƒŒƒCƒ„[‚ÌTransform"), SerializeField] private Transform playerTrans;
    private INoiseSource noiseSource;

    

    public EnemyDetecterForLockOn EnemyDetecter { get => enemyDetecter;}
    public Transform PlayerTrans { get => playerTrans; }
    public INoiseSource PlayerNoiseSource { get => noiseSource; }

    public void SetEnemyDetecter(EnemyDetecterForLockOn enemyDetecter)
    {
        this.enemyDetecter = enemyDetecter;
    }

    public void SetPlayerTrans(Transform playerTrans)
    {
        this.playerTrans = playerTrans;
    }

    public void SetPlayer(Player player)
    {
        this.noiseSource = player;
    }

}
