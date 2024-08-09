using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHitRange : MonoBehaviour
{
    int m_HitNumber = 0;

    void OnTriggerStay(Collider x_Obj)
    {
        if (m_HitNumber != x_Obj.GetComponent<UnitAttackRange>().m_AttackNumber && x_Obj.GetComponent<UnitAttackRange>().m_AttackNumber != 0 && x_Obj.GetComponent<UnitAttackRange>().m_AttackSplash > 0)
        {
            m_HitNumber = x_Obj.GetComponent<UnitAttackRange>().m_AttackNumber;
            x_Obj.GetComponent<UnitAttackRange>().m_AttackSplash--;

            // 히트 처리
            transform.parent.gameObject.GetComponent<UnitStatus>().m_HP -= x_Obj.GetComponent<UnitAttackRange>().m_Power;
        }
    }

}