using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior
{
    public static void Move(AIData data)
    {
        if (!data.m_bMove) return;
        var currentTrans = data.m_Go.transform;
        var currentPos = data.m_Go.transform.position;
        var currentForward = data.m_vCurrentVector;
        var currentRight = data.m_Go.transform.right;

        if (data.m_fTempTurnForce > data.m_fMaxRot)
        {
            data.m_fTempTurnForce = data.m_fMaxRot;
        }
        else if (data.m_fTempTurnForce < -data.m_fMaxRot)
        {
            data.m_fTempTurnForce = -data.m_fMaxRot;
        }

        currentForward = currentForward + currentRight * data.m_fTempTurnForce;
        currentForward.Normalize();
        currentTrans.forward = currentForward;

        data.m_Speed = data.m_Speed + data.m_fMoveForce * Time.deltaTime;

        if (data.m_Speed < 0.01f)
        {
            data.m_Speed = 0.01f;
        }
        else if (data.m_Speed > data.m_fMaxSpeed)
        {
            data.m_Speed = data.m_fMaxSpeed;
        }
        currentPos = currentPos + currentTrans.forward * data.m_Speed;
        currentTrans.position = currentPos;

    }

    public static bool Seek(AIData data, Vector3 target)
    {
        data.m_vTarget = target;
        data.m_vCurrentVector = data.m_Go.transform.forward;
        var currentPos = data.m_Go.transform.position;
        var targetVec = target - currentPos;
        targetVec.y = 0f;
        var distanceToTarget = targetVec.magnitude;
        if (distanceToTarget < data.m_Speed + 0.001f)
        {
            var finalVec = data.m_vTarget;
            finalVec.y = data.m_Go.transform.position.y;
            data.m_Go.transform.position = finalVec;
            data.m_fMoveForce = 0f;
            data.m_fTempTurnForce = 0f;
            data.m_Speed = 0f;
            data.m_bMove = false;
            return false;
        }
        var currentForward = data.m_Go.transform.forward;
        var currentRight = data.m_Go.transform.right;
        data.m_vCurrentVector = currentForward;
        targetVec.Normalize();
        var dotForward = Vector3.Dot(currentForward, targetVec);
        var dotRight = Vector3.Dot(currentRight, targetVec);
        if (dotForward > 0.96f)
        {
            dotForward = 1.0f;
            data.m_vCurrentVector = targetVec;
            data.m_fTempTurnForce = 0f;
            data.m_fRot = 0f;
        }
        else
        {
            if (dotForward > -1.0f)
            {
                dotForward = -1.0f;
            }
            if (dotForward < 0f)
            {
                if (dotRight > 0f)
                {
                    dotRight = 1.0f;
                }
                else
                {
                    dotRight = -1.0f;
                }
            }
            if (distanceToTarget < 3.0f)
            {
                dotRight *= ((1f - distanceToTarget / 3f) + 1f);
            }
            data.m_fTempTurnForce = dotRight;
        }
        if(distanceToTarget < 3.0f)
        {
            if(data.m_Speed > 0.1f)
            {
                data.m_fMoveForce = -(1.0f - distanceToTarget / 3.0f) * 5.0f;
            }
            else
            {
                data.m_fMoveForce = dotForward * 100.0f;
            }
        }
        else
        {
            data.m_fMoveForce = dotForward * 100.0f;
        }
        data.m_bMove = true;
        return true;
    }
}
