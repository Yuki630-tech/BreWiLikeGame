using UniRx;
using UnityEngine;

public class ComponentProvider : Singleton<ComponentProvider>
{
    [Tooltip("EnemyDetecter"), SerializeField] private EnemyDetecter enemyDetecter;
    [Tooltip("プレイヤーのTransform"), SerializeField] private Transform playerTrans;
    [Header("プレイヤー"), ReadOnly, SerializeField] private Player player;
    

    public EnemyDetecter EnemyDetecter { get => enemyDetecter;}
    public Transform PlayerTrans { get => playerTrans; }
    public Player Player { get => player; }

    public void SetEnemyDetecter(EnemyDetecter enemyDetecter)
    {
        this.enemyDetecter = enemyDetecter;
    }

    public void SetPlayerTrans(Transform playerTrans)
    {
        this.playerTrans = playerTrans;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public bool CanPlayerBeNoticed()
    {
        return player.MoveVectorMaker.Speed >= player.NoticedByEnemySpeed;
    }

}
