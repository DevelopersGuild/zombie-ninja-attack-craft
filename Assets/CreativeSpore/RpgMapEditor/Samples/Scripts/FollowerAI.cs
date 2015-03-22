using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

namespace CreativeSpore
{
    [RequireComponent(typeof(MovingBehaviour))]
    [RequireComponent(typeof(PhysicCharBehaviour))]
    [RequireComponent(typeof(MapPathFindingBehaviour))]
	public class FollowerAI : MonoBehaviour {

		GameObject m_player;
		MovingBehaviour m_moving;
		PhysicCharBehaviour m_phyChar;
        MapPathFindingBehaviour m_pathFindingBehaviour;

		public float AngRandRadious = 0.16f;
		public float AngRandOff = 15f;
		float m_fAngOff;

		// Use this for initialization
		void Start () 
		{
			m_fAngOff = 360f * Random.value;
			m_player = GameObject.FindGameObjectWithTag("Player");
			m_moving = GetComponent<MovingBehaviour>();
			m_phyChar = GetComponent<PhysicCharBehaviour>();
            m_pathFindingBehaviour = GetComponent<MapPathFindingBehaviour>();
            m_pathFindingBehaviour.Target = m_player;
		}
		
		// Update is called once per frame
		void Update() 
		{
            Vector3 vTarget = m_player.transform.position; vTarget.z = transform.position.z;

            m_pathFindingBehaviour.enabled = (vTarget - transform.position).sqrMagnitude > 0.32*0.32;
            if (!m_pathFindingBehaviour.enabled)
            {
                m_fAngOff += Random.Range(-AngRandOff, AngRandOff);
                Vector3 vOffset = Quaternion.AngleAxis(m_fAngOff, Vector3.forward) * (AngRandRadious * Vector3.right);
                vTarget += vOffset;
                Debug.DrawLine(transform.position, m_player.transform.position, Color.blue);
                Debug.DrawRay(m_player.transform.position, vOffset, Color.blue);

                m_moving.Arrive(vTarget);
            }

            //+++avoid obstacles
            Vector3 vTurnVel = Vector3.zero;
			if ( 0 != (m_phyChar.CollFlags & PhysicCharBehaviour.eCollFlags.RIGHT))
			{
                vTurnVel.x = -m_moving.MaxSpeed;
			}
			else if ( 0 != (m_phyChar.CollFlags & PhysicCharBehaviour.eCollFlags.LEFT))
			{
                vTurnVel.x = m_moving.MaxSpeed;
			}
			if ( 0 != (m_phyChar.CollFlags & PhysicCharBehaviour.eCollFlags.DOWN))
			{
                vTurnVel.y = m_moving.MaxSpeed;
			}
			else if ( 0 != (m_phyChar.CollFlags & PhysicCharBehaviour.eCollFlags.UP))
			{
                vTurnVel.y = -m_moving.MaxSpeed;
			}
            if( vTurnVel != Vector3.zero )
            {
                m_moving.ApplyForce(vTurnVel - m_moving.Veloc);
            }
            //---
		}
	}
}