using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtility
{

    static float minDistance = 2f;
    static int attemptCount = 30;
    public static bool TryGetCirclePosOnNavMesh(Vector3 center, float radius, out Vector3 pos)
    {
        for(int i = 0; i < attemptCount; i++)
        {
            NavMeshHit hit;

            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(0f, radius);

            Vector3 randomPos = center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;

            if (NavMesh.SamplePosition(randomPos, out hit, minDistance, NavMesh.AllAreas))
            {
                pos = hit.position;
                return true;
            }
        }

        pos = Vector3.zero;
        return false;
    }
}
