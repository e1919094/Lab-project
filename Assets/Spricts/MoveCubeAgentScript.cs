using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using UnityEngine;


public class MoveCubeAgentScript : Agent
{
    [SerializeField]
    [Tooltip("巡回する地点の配列")]
    public Transform[] waypoints;
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;
    public float moveSpeed = 3f;
    private int rnd;
    public GuideScript GuideScript;
    // Start is called before the first frame update
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rnd = Random.Range(0 ,waypoints.Length);
        GuideScript.SetDestination(waypoints[rnd]);
        Debug.Log(rnd);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //キーボード数値取得。プレイヤーの方向として扱う
        inputHorizontal = actionBuffers.ContinuousActions[0];
        inputVertical = actionBuffers.ContinuousActions[1];
        
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
 
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
 
        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flag")
        {
            Debug.Log("Flag");
            if(waypoints[rnd] == other.gameObject.transform)
            {
                rnd = Random.Range(0 ,waypoints.Length);
                GuideScript.NestDestination(waypoints[rnd]);
                Debug.Log("Goal. Next " + rnd);
            }
            
        }
    }

    void OnCollisionEnter(Collision other)
    {
         if(other.gameObject.tag == "Building")
        {
           Debug.Log("当たっとるけど");
        }
    }

     public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }


}
