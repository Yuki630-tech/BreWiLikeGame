using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    [Tooltip("武器の種類"), SerializeField] private WeaponType type;
    [Tooltip("武器の名前"), SerializeField] private string weaponName = "テッキュウクラッシャー";
    [Tooltip("武器の写真"), SerializeField] private Sprite sprite;
    [Tooltip("エフェクトの出現位置のトランスフォーム"), SerializeField] private Transform spawnTrans;
    [Tooltip("プレイヤーのトランスフォーム"), SerializeField] private Transform playerTrans;

    [SerializeField] private GameObject effectObj;


    private void Awake()
    {

           // Debug.Log("にんじんしりしり");
    }

    private void OnEnable()
    {
        
    }

    public string WeaponName { get => weaponName;}
    public WeaponType Type { get => type; }
    public Sprite Sprite { get => sprite;}

    public enum WeaponType
    {
        Physical,
        Magic,
        DeclinePhysical,
        WaveGunPhysical,
        TrearuteDragonPhysical,
    }

}
