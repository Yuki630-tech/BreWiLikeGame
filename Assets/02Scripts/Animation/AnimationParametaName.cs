using UnityEngine;

public static class AnimationParametaName
{
    public static string Move { get; private set; } = "Move";

    public static string AnimationSpeedForBlendTree { get; private set; } = "AnimationSpeed";
    public static string IsGround { get; private set; } = "IsGround";
    public static string FallSpeed { get; private set; } = "FallSpeed";
    public static string PhysicalAttackTrigger { get; private set; } = "PhysicalAttack";
    public static string MagicalAttackTrigger { get; private set; } = "MagicalAttack";
    public static string EndlessAttackTrigger { get; private set; } = "EndlessAttack";
    public static string DragonAttack { get; private set; } = "Attack";
    public static string WaterWaveGunTrigger { get; private set; } = "WaterWaveGun";
    public static string TreasureDragonAttackTrigger { get; private set; } = "TreasureDragonAttack";
    public static string Die { get; private set; } = "Die";

    public static string HasShield { get; private set; } = "HasShield";
    public static string ShieldMoveX { get; private set; } = "ShieldMoveX";
    public static string ShieldMoveZ { get; private set; } = "ShieldMoveZ";
    public static string Jump { get; private set; } = "Jump";
    public static string MoveAnimSpeed { get; private set; } = "MoveAnimSpeed";

    public static float WalkAnimSpeed { get; private set; } = 2.609776f;
    public static float DAshAnimSpeed { get; private set; } = 5.096277f;

}
