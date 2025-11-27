using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Tooltip("WeaponData‚ÌƒŠƒXƒg"), SerializeField] private List<WeaponDataValue> weaponDataValueList = new List<WeaponDataValue>();
    [Serializable]
    public class WeaponDataValue
    {
        public string WeaponName;
        public GameObject WeaponObj;

    }
}
