using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.Animations;

public class WeaponContainer : MonoBehaviour
{

    [Header("武器のParentConstraintに関する設定")]
    [Tooltip("武器を所持しているときの親オブジェクトのインデックス"), SerializeField] private int useWeaponIndex = 0;
    [Tooltip("武器を収めているときの親オブジェクトのインデックス"), SerializeField] private int putInWeaponIndex = 1;
    [Tooltip("プレイヤー(攻撃可能フラグを取得するため"), SerializeField] private Player player;

    [Header("武器を使っている状態かどうか"), SerializeField]
    private bool isUse;
    private bool canUse;

    private int currentIndex = 0;

    public ReactiveProperty<Weapon> CurrentWeapon { get; private set; } = new ReactiveProperty<Weapon>();
    public bool IsUse { get => isUse; }

    public class WeaponParemtConstraintData
    {

    }

    private void Awake()
    {
        canUse = true;
    }

    private void Update()
    {
        //if (InputManager.Instance.IsQInput)
        //{
        //    ReduceCurrentWeaponIndex();
        //}

        //else if(InputManager.Instance.IsEInput)
        //{
        //    AddCurrentWeaponIndex();
        //}
    }

    ////public void SetCurrentWeapon(int weaponIndex)
    ////{
    ////    if(weaponIndex < 0 || weaponIndex >= weapons.Count)
    ////    {
    ////        Debug.LogError("weaponIndexが不適切です");
    ////        return;
    ////    }
    ////    DisposeWeapon();

    ////    StopToUseWeapon();
    ////    CurrentWeapon.Value = weapons[weaponIndex];
    ////    CurrentWeapon.Value.gameObject.SetActive(true);
    ////    currentIndex = weaponIndex;
    ////}

    //private void AddCurrentWeaponIndex(int addIndex = 1)
    //{
    //    if(currentIndex >= weapons.Count - 1)
    //    {
    //        currentIndex = 0;
    //    }

    //    else
    //    {
    //        currentIndex += addIndex;
    //    }
    //    DisposeWeapon();
    //    StopToUseWeapon();
    //    EnableCurrentWeapon();
    //}

    public void StartToUseWeapon()
    {
        ParentConstraint constraint = CurrentWeapon.Value.GetComponentInParent<ParentConstraint>();
        if (constraint == null) return;

        ConstraintSource srcUse = constraint.GetSource(useWeaponIndex);
        ConstraintSource srcPutIn = constraint.GetSource(putInWeaponIndex);

        srcUse.weight = 1f;
        srcPutIn.weight = 0f;

        constraint.SetSource(useWeaponIndex, srcUse);
        constraint.SetSource(putInWeaponIndex, srcPutIn);

        isUse = true;
    }

    public void StopToUseWeapon()
    {
        ParentConstraint constraint = CurrentWeapon.Value != null ? CurrentWeapon.Value.GetComponentInParent<ParentConstraint>() : null;

        if(constraint == null) return;
        ConstraintSource srcUse = constraint.GetSource(useWeaponIndex);
        ConstraintSource srcPutIn = constraint.GetSource(putInWeaponIndex);

        srcUse.weight = 0f;
        srcPutIn.weight = 1f;

        constraint.SetSource(useWeaponIndex, srcUse);
        constraint.SetSource(putInWeaponIndex, srcPutIn);

        isUse = false;
    }
}
