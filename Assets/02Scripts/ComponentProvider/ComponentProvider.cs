using UnityEngine;

public class ComponentProvider : Singleton<ComponentProvider>
{
    [Tooltip("EnemyDetecter"), SerializeField] private EnemyDetecter enemyDetecter;

    public EnemyDetecter EnemyDetecter { get => enemyDetecter;}

    public void SetEnemyDetecter(EnemyDetecter enemyDetecter)
    {
        this.enemyDetecter = enemyDetecter;
    }
}
