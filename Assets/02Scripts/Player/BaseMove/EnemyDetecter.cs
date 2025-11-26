using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class EnemyDetecter : MonoBehaviour
{
    [Tooltip("プレイヤー"), SerializeField] private Transform playerTrans;
    private int index = -1;

    [Header("敵のリスト"), SerializeField] private List<EnemyInfo> enemyInfoList = new();

    public IReadOnlyList<EnemyInfo> EnemyInfoList => enemyInfoList;

    [System.Serializable]
    public class EnemyInfo
    {
        public GameObject EnemyObj;
        public float Distance;

        
        public EnemyInfo(GameObject enemyObj, float distance)
        {
            EnemyObj = enemyObj;
            Distance = distance;
        }
    }

    private void Update()
    {
        foreach(var enemyInfo in enemyInfoList)
        {
            enemyInfo.Distance = Vector3.Distance(playerTrans.position, enemyInfo.EnemyObj.transform.position);
        }
    }

    public GameObject GetEnemy()
    {
        index++;
        if(index == enemyInfoList.Count)
        {
            index = 0;
        }
        return enemyInfoList[index].EnemyObj;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Enemy))
        {
            float distance = Vector3.Distance(playerTrans.position, other.transform.position);
            enemyInfoList.Add(new(other.gameObject, distance));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagName.Enemy))
        {
            EnemyInfo enemyInfo = enemyInfoList.Find(x => x.EnemyObj == other.gameObject);
            enemyInfoList.Remove(enemyInfo);
        }
    }
}
