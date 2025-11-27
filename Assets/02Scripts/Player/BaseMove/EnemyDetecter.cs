using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class EnemyDetecter : MonoBehaviour
{
    [Tooltip("プレイヤー"), SerializeField] private Transform playerTrans;
    [Tooltip("プレイヤーカメラ"), SerializeField] private PlayerCamera playerCamera;
    private int index = 0;

    [Header("敵のリスト"), ReadOnly, SerializeField] private List<GameObject> enemyList = new();
    private ReactiveProperty<int> enemyCount = new ReactiveProperty<int>();

    private ReactiveProperty<Vector2> changeEnemyInputProperty = new();

    public IReadOnlyList<GameObject> EnemyList => enemyList;

    public GameObject TargetEnemy { get; private set; }
    public ReactiveProperty<int> EnemyCount { get => enemyCount; }

    //[System.Serializable]
    //public class EnemyInfo
    //{
    //    public GameObject EnemyObj;
    //    public float Distance;


    //    public EnemyInfo(GameObject enemyObj, float distance)
    //    {
    //        EnemyObj = enemyObj;
    //        Distance = distance;
    //    }
    //}

    private void Awake()
    {
        changeEnemyInputProperty.Where(input => input.y > 0).Subscribe(_ => AddEnemyIndex()).AddTo(gameObject);
        changeEnemyInputProperty.Where(input => input.y < 0).Subscribe(_ => RemoveEnemyIndex()).AddTo(gameObject);

        enemyCount.Where(x => x == 0).Subscribe(_ => TargetEnemy = null);
    }

    private void Update()
    {
        //foreach(var enemyInfo in enemyInfoList)
        //{
        //    enemyInfo.Distance = Vector3.Distance(playerTrans.position, enemyInfo.EnemyObj.transform.position);
        //}

        changeEnemyInputProperty.Value = InputManager.Instance.ChangeEnemyInput;
        enemyCount.Value = enemyList.Count;
    }

    private void AddEnemyIndex()
    {
        if(enemyList.Count == 0)
        {
            TargetEnemy = null;
            return;
        }
        index++;

        if (index >= enemyList.Count)
        {
            index = 0;
        }

        TargetEnemy = enemyList[index];
        playerCamera.SetSecondTarget(TargetEnemy.transform);
    }

    private void RemoveEnemyIndex()
    {
        if(enemyList.Count == 0)
        {
            TargetEnemy = null;
            return;
        }
        index--;
        if(index < 0)
        {
            index = enemyList.Count - 1;
        }

        TargetEnemy = enemyList[index];
        playerCamera.SetSecondTarget(TargetEnemy.transform);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Enemy))
        {
            float distance = Vector3.Distance(playerTrans.position, other.transform.position);
            enemyList.Add(other.gameObject);
            TargetEnemy = enemyList[index];

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagName.Enemy))
        {
            enemyList.Remove(other.gameObject);
        }
    }
}
