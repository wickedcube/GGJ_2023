using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public static class IntExtensions
    {
        public static int SquareRoot(this int value) => (int)Mathf.Sqrt(value);
        public static int CubeRoot(this int value) => (int)Mathf.Pow(value, (1 / 3f));
    }

    

    public enum PlaceHolderAttackType
    {
        SqrtBullet = 0,
        CubeRootBullet = 1,
        ReducerBomb = 2
    }

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
        private float RegularSpeed;

        [SerializeField]
        private float AgitatedSpeedIncrease;

        [SerializeField]
        [Tooltip("Tunable radius. When the player is inside this radius, it'll attack it")]
        private float AttackRadius;

        // this number is what the number of the object is.
        public int Number { set; get; } = 1;


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



        private void Patrol()
        {
            var distanceToWalkPoint = Vector3.Distance(WalkPoint, this.transform.position);
            if (!Agent.hasPath || distanceToWalkPoint < 2f)
            {
                WalkPoint = GetNewPatrollingWalkPoint();
                Agent.ResetPath();
                Agent.SetDestination(WalkPoint);
            }


            if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
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

        }

        private bool IsPerfectSquare()
        {
            if(Number > 0)
            {
                int sqrt = Number.SquareRoot();
                return sqrt * sqrt == Number;
            }
            return false;
        }

        private bool IsPerfectCube()
        {
            if(Number > 0)
            {
                int cbrt = Number.CubeRoot();

                return cbrt * cbrt * cbrt == Number;
            }

            return false;
        }

        private bool IsPrime()
        {
            if(Number == 0 || Number == 1 || Number == 2)
            {
                return true; //this should only happen on 2.. 
            }


            for (int i = 2; i <= Number / 2 + 1; ++i)
            {

                if (Number % i == 0)
                {

                    return false;
                }
            }

            return true;
        }


        public bool IsAttackValid(PlaceHolderAttackType bulletType)
        {

            switch (bulletType)
            {
                case PlaceHolderAttackType.SqrtBullet: return IsPerfectSquare();
                case PlaceHolderAttackType.CubeRootBullet: return IsPerfectCube();
                case PlaceHolderAttackType.ReducerBomb: return !IsPrime();
            }
           
            return false;
        }

        public int GetCubeRoot() => this.Number.CubeRoot();
        public int GetSquareRoot() => this.Number.SquareRoot();
    }

}
