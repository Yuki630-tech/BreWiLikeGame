using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackStrategyFactory<T> : IStrategyFactory<Weapon.WeaponType, T> where T : MonoBehaviour
{
    Dictionary<Weapon.WeaponType, IStrategy<T>> strategies = new Dictionary<Weapon.WeaponType, IStrategy<T>>();
    IStrategy<T> currentAttackStrategy;
    
    /// <summary>
    /// 現在の手法
    /// </summary>
    /// <returns></returns>
    public IStrategy<T> GetStrategy()
    {
        return currentAttackStrategy;
    }

    /// <summary>
    /// 手法を選択する関数。中でChangeStrategy(weaponContainer.CurrentWeapon.Value.Type)を呼び出す
    /// </summary>
    public void CreateStrategy(T owner, Weapon.WeaponType type)
    {
        if(currentAttackStrategy != null)
        {
            currentAttackStrategy.Exit(owner);
        }
        if (strategies.ContainsKey(type))
        {
            Debug.Log($"攻撃方法を切り替えました : {type.ToString()}");
            currentAttackStrategy = strategies[type];
            //currentWeaponType = type;
        }
    }

    /// <summary>
    /// strategyファクトリーが保持する手法のリストに要素を追加する関数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="strategy"></param>
    public void AddStrategy(Weapon.WeaponType type, IStrategy<T> strategy)
    {
        strategies[type] = strategy;
    }

    public void ChangeStrategy(T owner, Weapon.WeaponType type)
    {
        //if(currentWeaponType == type) return;
        if(currentAttackStrategy != null)
        {
            currentAttackStrategy.Exit(owner);
        }

        currentAttackStrategy = strategies[type];
        //currentWeaponType = type;
        currentAttackStrategy.Enter(owner);
    }
}
