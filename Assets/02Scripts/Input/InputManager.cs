using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputManager : Singleton<InputManager>
{
    [Tooltip("フィールドのPlayerInput"), SerializeField] private PlayerInput fieldPlayerInput;
    [Tooltip("メニューのPlayerInput"), SerializeField] private PlayerInput menuPlayerInput;

    public Vector2 LeftStickInput { get; private set; }
    public Vector2 RightStickInput { get; private set; }

    public Vector2 ChangeEnemyInput { get; private set; } 
    public bool IsJumpInput { get; private set; }
    public bool IsDashInput { get; private set; }
    public bool WasDashInputThisFrame { get; private set; }
    public bool IsAttackInput { get; private set; }
    public bool IsQInput {  get; private set; }
    public bool IsEInput { get; private set; }
    /// <summary>
    /// Eが押し続けられている
    /// </summary>
    public ReactiveProperty<bool> IsEPushing { get; private set; }
    public bool MenuOpen { get; private set; }
    public bool IsRInput { get; private set; }
    public bool IsCInput { get; private set; }
    public ReactiveProperty<bool> IsFPushing { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> IsFReleased { get; private set; } = new ReactiveProperty<bool>();
    public bool IsFInput {  get; private set; }
    public bool IsRightInput { get; private set; }
    public bool IsLeftInput {  get; private set; }

    public bool IsShieldPushing { get; private set; }
    public bool IsSheildInput { get; private set; }
    public bool IsSheildReleased { get; private set; }
    public ReactiveProperty<bool> IsDecided { get; internal set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> IsGoToNextSerif { get; internal set; } = new ReactiveProperty<bool>();

    private void Update()
    {
        LeftStickInput = fieldPlayerInput.currentActionMap[InputMapName.LeftStick].ReadValue<Vector2>();
        RightStickInput = fieldPlayerInput.currentActionMap[InputMapName.RightStick].ReadValue<Vector2>();
        ChangeEnemyInput = fieldPlayerInput.currentActionMap[InputMapName.ChangeTarget].ReadValue<Vector2>();
        IsJumpInput = fieldPlayerInput.currentActionMap[InputMapName.Jump].WasPressedThisFrame();
        IsDashInput = fieldPlayerInput.currentActionMap[InputMapName.Dash].IsPressed();
        WasDashInputThisFrame = fieldPlayerInput.currentActionMap[InputMapName.Dash].WasPressedThisFrame();
        IsAttackInput = fieldPlayerInput.currentActionMap[InputMapName.Attack].WasPressedThisFrame();
        IsQInput = fieldPlayerInput.currentActionMap[InputMapName.Q].WasPressedThisFrame();
        IsEInput = fieldPlayerInput.currentActionMap[InputMapName.E].WasPressedThisFrame();
        //IsEPushing.Value = fieldPlayerInput.currentActionMap[InputMapName.E].IsPressed();
        IsRInput = fieldPlayerInput.currentActionMap[InputMapName.R].WasPressedThisFrame();
        IsCInput = fieldPlayerInput.currentActionMap[InputMapName.C].WasPressedThisFrame();
        IsFInput = fieldPlayerInput.currentActionMap[InputMapName.F].WasPressedThisFrame();
        IsFPushing.Value = fieldPlayerInput.currentActionMap[InputMapName.F].IsPressed();
        IsFReleased.Value = fieldPlayerInput.currentActionMap[InputMapName.F].WasReleasedThisFrame();

        IsShieldPushing = fieldPlayerInput.currentActionMap[InputMapName.Shield].IsPressed();
        IsSheildInput = fieldPlayerInput.currentActionMap[InputMapName.Shield].WasPressedThisFrame();
        IsSheildReleased = fieldPlayerInput.currentActionMap[InputMapName.Shield].WasReleasedThisFrame();
        //IsRightInput = menuPlayerInput.currentActionMap[InputMapName.Right].WasPressedThisFrame();
        //IsLeftInput = menuPlayerInput.currentActionMap[InputMapName.Left].WasPressedThisFrame();
        //IsGoToNextSerif.Value = menuPlayerInput.currentActionMap[InputMapName.RightClick].WasPressedThisFrame();
    }
}
