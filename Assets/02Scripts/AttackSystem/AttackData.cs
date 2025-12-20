using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    [Tooltip("ƒ_ƒ[ƒW"), SerializeField] private float damage;

    public float Damage { get => damage; }
}
