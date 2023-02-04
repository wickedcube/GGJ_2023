using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Powerups.Grenade;
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

    public class EnemyBehavior : MonoBehaviour, INumberEnemy
    {
        [SerializeField]
        private NavMeshAgent Agent;

        [SerializeField]
        private Transform PlayerTransform;

        [SerializeField]
        private MeshRenderer WalkableArea;


        [SerializeField]
        private float RegularSpeed;

        [SerializeField]
        private float AgitatedSpeedIncrease;

        [SerializeField]
        [Tooltip("Tunable radius. When the player is inside this radius, it'll attack it")]
        private float AttackRadius;

        private bool IsPrime => NumberAlgorithms.IsPrime(this.Value);
        private bool IsPerfectSquare => NumberAlgorithms.IsPerfectSquare(this.Value);
        private bool IsPerfectCube => NumberAlgorithms.IsPerfectCube(this.Value);
        /// <summary>
        /// this is the point that the enemy is usually walking towards.
        /// </summary>
        private Vector3 WalkPoint { set; get; }

        public int Value { private set; get; }


        public void SetValue(int val)
        {
            // TODO : use the enemy creation script to create the number value.
            throw new System.NotImplementedException();
        }


        // Start is called before the first frame update
        void Start()
        {
            WalkPoint = this.transform.position;
            ChronoHelper.OnChronoEffectStarted += OnChronoEffectStarted;
            ChronoHelper.OnChronoEffectEnded += OnChronoEffectEnded;
        }
        private void OnChronoEffectStarted(float slowDownPercentage)
        {
            // TODO : Implement the Chrono slowdown
        }

        private void OnChronoEffectEnded()
        {
            // TODO : Implement the Chrono slowdown.
        }
        

        // Update is called once per frame
        void Update()
        {
            Patrol();
            TryAttackPlayer();
        }
        public bool CanTakeDamage(object enemyObject) // MAKE THIS FULLY PRIVATE BITCH!! 
        {
            if (this.IsPrime) return false; // prime numbers are healths. Can't take damage.

            if (enemyObject is Grenade) return true;
            // TODO : Can Take Damage for bullets.


            return false;
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

        private void TryAttackPlayer()
        {
            var distanceToEnemy = Vector3.Distance(this.transform.position, PlayerTransform.position);
            if(distanceToEnemy < AttackRadius)
            {
                Agent.SetDestination(PlayerTransform.position);
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

        public bool TakeDamage(object obj)
        {
            if (this.CanTakeDamage(obj))
            {
                if(obj is Grenade)
                {
                    // return the enemy to enemy pool.
                    return true;
                }
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(" Entering the fucking enemy!");

            var enteredGameObject = other.gameObject;
            var player = enteredGameObject.GetComponent<PlayerController>();
            if(player != default)
            {
                player.TakeDamage(this);
            }
        }
    }

}
