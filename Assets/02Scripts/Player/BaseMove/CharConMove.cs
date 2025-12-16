using UnityEngine;

public class CharConMove
{
    private Transform playerTrans;
    private CharacterController characterController;
    private MoveVectorMaker moveVectorMaker;
    private VerticalMoveMaker verticalMoveMaker;
    private bool isGround;
    private GroundChecker groundChecker;
    private Vector3 verticalVector;
    private float jumpPower;
    private Vector3 move;
    private bool isMovable;

    public CharConMove(Transform setTrans, CharacterController setCharacterController, MoveVectorMaker setMoveVectorMaker,  VerticalMoveMaker setVerticalMoveMaker, GroundChecker setGroundChecker, float setJumpPower)
    {
        playerTrans = setTrans;
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

    public void SetIfMovable(bool value)
    {
        isMovable = value;
    }

    private void MakeVector(float deltaTime)
    {
        //if (verticalMoveMaker.VerticalSpeed <= 0f)
        //{
        //    isGround = groundChecker.IsGround;
        //}

        //else
        //{
        //    //Debug.Log("プレイヤーが飛びあがった");
        //    isGround = false;
        //}

        isGround = groundChecker.IsGround;
        if (isMovable)
        {
            moveVectorMaker.MakeMoveVector();
        }
        verticalMoveMaker.Update(deltaTime);
        verticalVector = verticalMoveMaker.FallVector;
        move = isMovable ? moveVectorMaker.MoveVector : Vector3.zero;

        float diff = Vector3.Angle(playerTrans.up, groundChecker.Normal);
        Debug.Log("プレイヤーと地面の垂線との角度 : " + diff);
        if (diff > groundChecker.GroundSlopeLimit && verticalVector.y < 0)
        {
            Debug.Log("滑り落ちる角度です");
            //坂に向かって移動しようとしているか判定(0未満→坂道の垂線とは逆向きなら坂道を登ろうとしている
            float upDot = Vector3.Dot(move, groundChecker.Normal);

            if(upDot < 0)
            {
                Vector3 normal_XZ = new Vector3(groundChecker.Normal.x, 0f, groundChecker.Normal.z).normalized;

                float ascend = Vector3.Dot(move, normal_XZ);

                Vector3 cancelVec = normal_XZ * Mathf.Abs(ascend);

                move = move + cancelVec;
            }
            verticalVector = Vector3.ProjectOnPlane(verticalVector, groundChecker.Normal);
        }

        move += verticalVector;
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
