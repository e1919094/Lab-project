using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Game;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using UnityEngine;

public class AutoMoveAgent : Agent
{
    [Tooltip("The character that should start and stop the jump ability.")]
    [SerializeField] protected GameObject m_Character;
    [SerializeField] private float RotationSpeed;
    private UltimateCharacterLocomotion m_CharacterLocomotion;
    private float m_HorizontalMovement;
    private float m_ForwardMovement; 
    private float Rotation;
    private Vector3 WayPoint;
    public WayPointPatrolAgent waypointPatrol;
    public override void Initialize()
    {
        m_CharacterLocomotion = m_Character.GetComponent<UltimateCharacterLocomotion>(); 
        WayPoint = waypointPatrol.waypoints[waypointPatrol.currentWaypointIndex].transform.localPosition;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
         sensor.AddObservation(WayPoint);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Rotation = actionBuffers.ContinuousActions[0] * RotationSpeed;
        m_HorizontalMovement = actionBuffers.ContinuousActions[0];
        m_ForwardMovement = actionBuffers.ContinuousActions[1];
        KinematicObjectManager.SetCharacterMovementInput(m_CharacterLocomotion.KinematicObjectIndex, m_HorizontalMovement, m_ForwardMovement);
        KinematicObjectManager.SetCharacterDeltaYawRotation( m_CharacterLocomotion . KinematicObjectIndex ,Rotation);         
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Building")
        {
            AddReward(-0.5f);
        }

        if (other.gameObject.tag == "parson")
        {
            AddReward(-0.05f);
        }
        
    }


    public void ReachingWayPoints()
    {
        AddReward(2.0f);
        WayPoint = waypointPatrol.waypoints[waypointPatrol.currentWaypointIndex].transform.localPosition;
        Debug.Log(WayPoint);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

}