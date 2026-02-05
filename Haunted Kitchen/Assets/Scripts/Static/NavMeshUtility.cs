using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtility
{
    public static bool TryGetRandomPoint(
        Vector3 center,
        float radius,
        out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = center + new Vector3(
                Random.Range(-radius, radius),
                0f,
                Random.Range(-radius, radius));

            if (NavMesh.SamplePosition(
                randomPoint,
                out NavMeshHit hit,
                2f,
                NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
}
