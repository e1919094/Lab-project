using UnityEngine;
using UnityEngine.AI;

// NavMeshAgentコンポーネントがアタッチされていない場合アタッチ
[RequireComponent(typeof(NavMeshAgent))]
public class WayPointPatrol : MonoBehaviour
{
    [SerializeField]
    [Tooltip("巡回する地点の配列")]
    public Transform[] waypoints;

    // NavMeshAgentコンポーネントを入れる変数
    private NavMeshAgent navMeshAgent;
    // 現在の目的地
    [System.NonSerialized]
    public int currentWaypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        // navMeshAgent変数にNavMeshAgentコンポーネントを入れる
        navMeshAgent = GetComponent<NavMeshAgent>();
        // 最初の目的地を入れる
        int value = Random.Range(0, waypoints.Length + 1);
        currentWaypointIndex = (currentWaypointIndex + value) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        // 目的地点までの距離(remainingDistance)が目的地の手前までの距離(stoppingDistance)以下になったら
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            int value = Random.Range(0, waypoints.Length + 1);
            // 目的地の番号を更新（右辺を剰余演算子にすることで目的地をループさせれる）
            currentWaypointIndex = (currentWaypointIndex + value) % waypoints.Length;
            // 目的地を次の場所に設定
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
