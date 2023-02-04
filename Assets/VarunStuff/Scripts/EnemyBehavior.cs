using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Enemy
{
    public class EnemyBehavior : MonoBehaviour
    {

        [SerializeField]
        private NavMeshAgent Agent;

        [SerializeField]
        private Transform PlayerTransform;

        [SerializeField]
        private MeshRenderer WalkableArea;

        [SerializeField]
        private float DetectPlayerRadius;

        [SerializeField]
        private float RoamingSpeed;

        [SerializeField]
        private float AttackSpeed;

        [SerializeField]
        private float AttackRadius;

        [SerializeField]


        //[SerializeField]
        // For testing purposes!!.
        //[SerializeField]
        //private Camera Camera;

        //[SerializeField]
        //private Transform ClickLocationIndicator;


        /// <summary>
        /// this is the point that the enemy is usually walking towards.
        /// </summary>
        private Vector3 WalkPoint { set; get; }


        // Start is called before the first frame update
        void Start()
        {
            WalkPoint = this.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Patrol();
        }

        private List<float> accumulatedError = new List<float>();


        /*
        private void SetRayCastPosition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.ScreenPointToRay(Input.mousePosition);
                var rayCastHits = Physics.RaycastAll(ray, float.MaxValue, groundMask);
                foreach(var hit in rayCastHits)
                {

                    Agent.SetDestination(hit.point);
                    ClickLocationIndicator.position = hit.point + new Vector3(0 , hit.transform.localScale.y / 2, 0);

                    return;
                }
            }
        }
        */
        


        private void Patrol()
        {
            var distanceToWalkPoint = Vector3.Distance(WalkPoint, this.transform.position);
            if (!Agent.hasPath || distanceToWalkPoint < 2f )
            {
                var newPoint = GetNewPatrollingWalkPoint();
                var error = Vector3.Distance(newPoint, WalkPoint);
                accumulatedError.Add(error);
                WalkPoint = newPoint;
                Agent.ResetPath();
                Agent.SetDestination(WalkPoint);
            }
            

            if(Agent.pathStatus== NavMeshPathStatus.PathInvalid)
            {
                Debug.Log($" Path is invalid for {this.gameObject.name}");
                Agent.ResetPath();
            }
            
        }

        private Vector3 GetNewPatrollingWalkPoint()
        {
            var bounds = WalkableArea.bounds;
            var center = bounds.center;
            var extents = bounds.extents;


            var randomX = Random.Range(-1f, 1f);
            var randomZ = Random.Range(-1f, 1f);


            var position = center + new Vector3(randomX * extents.x + center.x, this.transform.position.y, center.z + randomZ * extents.z);

            return position;
        }


        private void OnDestroy()
        {
            string s = "";
            foreach(var error in accumulatedError)
            {
                s += $"{error} \t";
            }
            Debug.Log($"{this.gameObject.name} , has errors {s}");
        }
    }

}
