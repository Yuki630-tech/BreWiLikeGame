using UnityEngine;

public class ComponentProvider : Singleton<ComponentProvider>
{
    [Tooltip("EnemyDetecter"), SerializeField] private EnemyDetecter enemyDetecter;
    [Tooltip("ƒvƒŒƒCƒ„[‚ÌTransform"), SerializeField] private Transform playerTrans;

    public EnemyDetecter EnemyDetecter { get => enemyDetecter;}
    public Transform PlayerTrans { get => playerTrans; }

    public void SetEnemyDetecter(EnemyDetecter enemyDetecter)
    {
        this.enemyDetecter = enemyDetecter;
    }

    public void SetPlayerTrans(Transform playerTrans)
    {
        this.playerTrans = playerTrans;
    }
}
