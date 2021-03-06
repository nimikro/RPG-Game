using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controls enemy AI
public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;      // Detection range for player

    Transform target;       //Reference to the player
    NavMeshAgent agent;     // Reference to the NavMesh
    CharacterCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance)
            {
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                // Attack target
                if(targetStats != null)
                {
                    combat.Attack(targetStats);
                }
                // Face target
                FaceTarget();
            }
        }
    }

    // Turn to face target
    void FaceTarget()
    {
        // Get a direction towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        // Get a rotation pointing towards the target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        // Update rotation to point in the above direction
        // transform.rotation = lookRotation; <- basic method without smoothing
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
