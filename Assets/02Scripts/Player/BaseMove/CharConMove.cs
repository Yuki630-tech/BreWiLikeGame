using UnityEngine;

public class CharConMove
{
    private CharacterController characterController;
    private MoveVectorMaker moveVectorMaker;
    private VerticalMoveMaker verticalMoveMaker;
    private bool isGround;
    private GroundChecker groundChecker;
    private Vector3 verticalVector;
    private float jumpPower;
    private Vector3 move;

    public CharConMove(CharacterController setCharacterController, MoveVectorMaker setMoveVectorMaker,  VerticalMoveMaker setVerticalMoveMaker, GroundChecker setGroundChecker, float setJumpPower)
    {
        moveVectorMaker = setMoveVectorMaker;
        verticalMoveMaker = setVerticalMoveMaker;
        groundChecker = setGroundChecker;
        jumpPower = setJumpPower;
        characterController = setCharacterController;
    }

    public bool IsGround { get => isGround; }

    public void Update(float deltaTime)
    {
        MakeVector(deltaTime);

        characterController.Move(move * deltaTime);
    }

    private void MakeVector(float deltaTime)
    {
        if (verticalMoveMaker.VerticalSpeed <= 0f)
        {
            isGround = groundChecker.IsGround;
        }

        else
        {
            //Debug.Log("ƒvƒŒƒCƒ„[‚ª”ò‚Ñ‚ ‚ª‚Á‚½");
            isGround = false;
        }
        moveVectorMaker.MakeMoveVector();
        verticalMoveMaker.Update(deltaTime);
        verticalVector = verticalMoveMaker.FallVector;
        move = moveVectorMaker.MoveVector + verticalVector;
        if (InputManager.Instance.IsJumpInput && isGround)
        {
            verticalMoveMaker.Jump(jumpPower);
        }

        if (isGround)
        {
            move += groundChecker.GroundOffset;
        }

        if (verticalMoveMaker.VerticalSpeed <= 0f)
        {
            move = Vector3.ProjectOnPlane(move, groundChecker.Normal);
        }
    }
}
