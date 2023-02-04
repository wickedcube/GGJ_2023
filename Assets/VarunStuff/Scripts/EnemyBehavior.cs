using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Powerups.Grenade;
using ObjectPooling;

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

    public class EnemyBehavior : Poolable, INumberEnemy
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


        [SerializeField]
        private Transform NumberHolder;

        private bool IsPrime => NumberAlgorithms.IsPrime(this.Value);
        public bool IsPerfectSquare => NumberAlgorithms.IsPerfectSquare(this.Value);
        public bool IsPerfectCube => NumberAlgorithms.IsPerfectCube(this.Value);

        List<IndependentNumber> NumberComponents = new List<IndependentNumber>();


        /// <summary>
        /// this is the point that the enemy is usually walking towards.
        /// </summary>
        private Vector3 WalkPoint { set; get; }

        public int Value { private set; get; }

        bool parametersSet { set; get; } = false;

        public void SetParameters(MeshRenderer meshRenderer,
                                  Transform playerTransform)
        {
            this.WalkableArea = meshRenderer;
            this.PlayerTransform = playerTransform;
            parametersSet = true;
        }


        public void SetValue(int val, EnemyNumberCreator creator)
        {
            this.Value = val;
            NumberComponents = creator.CreateNumber(this.Value, NumberHolder);
        }



        protected override void OnReturnedToPool()
        {
            NumberHolder.localPosition = Vector3.zero;
            NumberHolder.localEulerAngles = Vector3.zero;
            NumberHolder.localScale = Vector3.one;

            foreach (var number in NumberComponents)
            {
                number.ReturnToPool();
            }
            NumberComponents.Clear();
            parametersSet = false;
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
            if(!parametersSet) return;
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

        private void OnCollisionEnter(Collision other)
        {

            var enteredGameObject = other.collider.gameObject;
            var player = enteredGameObject.GetComponent<PlayerController>();
            if(player != default)
            {
                player.TakeDamage(this);

                this.ReturnToPool();
            }
        }

    }

}
