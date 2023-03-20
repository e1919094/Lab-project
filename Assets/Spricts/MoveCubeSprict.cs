using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeSprict : MonoBehaviour
{ 
    [SerializeField]
    [Tooltip("巡回する地点の配列")]
    public Transform[] waypoints;
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;
    public float moveSpeed = 3f;
    int rnd;
    public GuideScript guideScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rnd = Random.Range(0 ,waypoints.Length);
        guideScript.SetDestination(waypoints[rnd]);
    }

    void Update()
    {
        Move();

        if(transform.localPosition.y < -10f)
        {
            Reset();
        }

    }
    void Move()
    {
        //キーボード数値取得。プレイヤーの方向として扱う
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        
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

    private void Reset()
    {
        transform.localPosition = new Vector3(0f,0f,0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flag")
        {
            Debug.Log("Flag");
            if(waypoints[rnd] == other.gameObject.transform)
            {
                rnd = Random.Range(0 ,waypoints.Length);
                guideScript.NestDestination(waypoints[rnd]);
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
   
}