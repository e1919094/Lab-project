using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshAgentコンポーネントがアタッチされていない場合アタッチ
[RequireComponent(typeof(NavMeshAgent))]
public class GuideScript : MonoBehaviour
{ 
    private NavMeshAgent navMeshAgent;
    
   void Start()
    {
        // navMeshAgent変数にNavMeshAgentコンポーネントを入れる
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDestination(Transform waypoints)
    {
        //最初の目的地
        navMeshAgent.SetDestination(waypoints.position);
    }
    public void NestDestination(Transform waypoints)
    {
        // 目的地を次の場所に設定
        navMeshAgent.SetDestination(waypoints.position);
    }
    
}