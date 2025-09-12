using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleLayerMask = -1; //when layers block line of sight
    private NavMeshAgent enemyMeshAgent;

    Camera playerCam;
    MeshRenderer tempRend;
    Plane[] cameraFrustum;
    Collider frustumCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyMeshAgent = GetComponent<NavMeshAgent>();

        playerCam = Camera.main;
        tempRend = GetComponent<MeshRenderer>();
        frustumCollider = transform.Find("EnemyBody").GetComponent<Collider>();
    }

    // Update is called once per frame

    //Please find a better way to do make the update loop that involves setting the speed to zero or using isStopped on the enemyMeshAgent.
    void Update()
    {
        if (player != null)
        {
            if (CanSeePlayer() && PlayerLooking())
            {
                enemyMeshAgent.SetDestination(transform.position);
            }
            else
            {
                enemyMeshAgent.SetDestination(player.position);
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //cast ray from enemy to player
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hitInfo, distanceToPlayer, obstacleLayerMask))
        {
            //return truth of if the hit is the player
            return hitInfo.transform == player;
        }

        // if no obstacles hit within player distance, the player can be seen
        return true;
    }

    bool PlayerLooking()
    {
        var bounds = frustumCollider.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(playerCam);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
