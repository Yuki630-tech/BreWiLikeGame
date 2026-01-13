using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class EnemyDetecterForLockOn : MonoBehaviour
{
    [Tooltip("プレイヤー"), SerializeField] private Transform playerTrans;
    //[Tooltip("敵方向に向いたカメラの水平方向の角度"), SerializeField] private float horizontalAngle = 0;
    //[Tooltip("敵方向に向いたカメラの垂直方向の角度"), SerializeField] private float verticalAngle = 45f;
    [Tooltip("カメラを敵方向に向けるスピード"), SerializeField] private float cameraRotSpeed = 1080f;
    //[Tooltip("プレイヤーカメラ"), SerializeField] private PlayerCamera playerCamera;
    private int index = 0;

    [Header("敵のリスト"), ReadOnly, SerializeField] private List<GameObject> enemyList = new();
    [SerializeField, ReadOnly] private ReactiveProperty<int> enemyCount = new ReactiveProperty<int>();

    private ReactiveProperty<Vector2> changeEnemyInputProperty = new();

    public IReadOnlyList<GameObject> EnemyList => enemyList;

    [ReadOnly, SerializeField] private ReactiveProperty<GameObject> targetEnemy = new();

    public ReactiveProperty<GameObject> TargetEnemy { get => targetEnemy; }
    public ReactiveProperty<int> EnemyCount { get => enemyCount; }
    public float CameraRotSpeed { get => cameraRotSpeed; }

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
        
        changeEnemyInputProperty.Where(input => input.y > 0 && InputManager.Instance.IsShieldPushing && enemyCount.Value > 1).Subscribe(_ => AddEnemyIndex()).AddTo(gameObject);
        changeEnemyInputProperty.Where(input => input.y < 0 && InputManager.Instance.IsShieldPushing && enemyCount.Value > 1).Subscribe(_ => RemoveEnemyIndex()).AddTo(gameObject);

        enemyCount.Where(x => x == 0 && targetEnemy != null).Subscribe(_ =>
        {
            targetEnemy.Value = null;
        }).AddTo(gameObject);
        enemyCount.Where(x => x > 0 && !enemyList.Contains(targetEnemy.Value)).Subscribe(_ => AddEnemyIndex()).AddTo(gameObject);
    }

    private void OnEnable()
    {
        ComponentProvider.Instance.SetEnemyDetecter(this);
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
            targetEnemy.Value = null;
            return;
        }
        index++;

        if (index >= enemyList.Count)
        {
            index = 0;
        }

        targetEnemy.Value = enemyList[index];

        //playerCamera.SetSecondTarget(TargetEnemy.transform);
        //_ = playerCamera.LookAt(PlayerCamera.CameraKind.TargetGroup, playerTrans, TargetEnemy.transform, cameraRotSpeed);
    }

    private void RemoveEnemyIndex()
    {

        if (enemyList.Count == 0)
        {
            targetEnemy.Value = null;
            return;
        }
        index--;
        if(index < 0)
        {
            index = enemyList.Count - 1;
        }

        targetEnemy.Value = enemyList[index];
        //playerCamera.SetSecondTarget(TargetEnemy.transform);
        //_ = playerCamera.LookAt(PlayerCamera.CameraKind.TargetGroup, playerTrans, TargetEnemy.transform, cameraRotSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Enemy))
        {
            float distance = Vector3.Distance(playerTrans.position, other.transform.position);
            enemyList.Add(other.gameObject);
            if(targetEnemy.Value == null)
            {
                targetEnemy.Value = other.gameObject;
                index = enemyList.IndexOf(targetEnemy.Value);
            }

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
