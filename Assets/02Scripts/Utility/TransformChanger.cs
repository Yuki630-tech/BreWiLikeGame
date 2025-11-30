using Cysharp.Threading.Tasks;
using UnityEngine;

public static class TransformChanger
{
    public static async UniTask ChangePos(Transform transform, Vector3 pos, float speed)
    {
        while(transform.position != pos)
        {
            transform.position = Vector3.Lerp(transform.position, pos, speed);
            await UniTask.Yield();
        }

        transform.position = pos;
    }

    public static async UniTask ChangeRot(Transform transform, Quaternion rot, float speed)
    {
        while(transform.rotation != rot)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, speed);
            await UniTask.Yield();
        }

        transform.rotation = rot;
    }
}
