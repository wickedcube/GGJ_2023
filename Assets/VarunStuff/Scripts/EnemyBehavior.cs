using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Powerups.Grenade;
using ObjectPooling;
using System.Linq;

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

        [SerializeField]
        private GameObject DestroyFX;

        [SerializeField]
        private BoxCollider BoxCollider;

        private bool IsPrime => NumberAlgorithms.IsPrime(this.Value);
        public bool IsPerfectSquare => NumberAlgorithms.IsPerfectSquare(this.Value);
        public bool IsPerfectCube => NumberAlgorithms.IsPerfectCube(this.Value);

        List<IndependentNumber> NumberComponents = new List<IndependentNumber>();
        // private PlayerStats playerStats;

        private EnemySpawner meraBagwhan;
        
        public float AgenSpeed;

        /// <summary>
        /// this is the point that the enemy is usually walking towards.
        /// </summary>
        private Vector3 WalkPoint { set; get; }

        public int Value { private set; get; }

        [SerializeField] bool parametersSet = false;

        public void SetParameters(MeshRenderer meshRenderer)
        {
            this.WalkableArea = meshRenderer;
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            var dist = Mathf.Infinity;
            Transform closestPlayer = null;
            foreach(var p in players)
            {
                float calcDist = Vector3.Distance(p.transform.position, this.transform.position);
                if(calcDist < dist)
                {
                    dist = calcDist;
                    closestPlayer = p.transform;
                }
            }
            this.PlayerTransform = closestPlayer;
        }


        private void RecalculateBounds()
        {
            Bounds b = new Bounds();
            foreach(var number in this.NumberComponents)
            {
                var meshRenderBounds = number.GetMeshRendererBounds;
                b.Encapsulate(new Bounds(meshRenderBounds.center, meshRenderBounds.size));
            }
            b.center = Vector3.zero + new Vector3(0, this.NumberComponents.First().HeightOffset, 0);
            BoxCollider.center = b.center;
            BoxCollider.size = b.size;
            if (BoxCollider.size.x < BoxCollider.size.z) this.Agent.radius = BoxCollider.size.z/2;
            else this.Agent.radius = BoxCollider.size.x/2;
        }

        public void SetValue(int val, EnemyNumberCreator creator)
        {
            this.Value = val;
            NumberComponents = creator.CreateNumber(this.Value, NumberHolder);
            RecalculateBounds();
            
            parametersSet = true;
        }

        
        protected override void OnReturnedToPool()
        {
            ChronoHelper.OnChronoEffectEnded -= OnChronoEffectEnded;
            ChronoHelper.OnChronoEffectStarted -= OnChronoEffectStarted;
            
            NumberHolder.localPosition = Vector3.zero;
            NumberHolder.localEulerAngles = Vector3.zero;
            NumberHolder.localScale = Vector3.one;

            foreach (var number in NumberComponents)
            {
                number.ReturnToPool();
                Instantiate(DestroyFX, transform.position, Quaternion.identity);
            }
            NumberComponents.Clear();
            parametersSet = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            AgenSpeed = RegularSpeed;
            meraBagwhan = FindObjectOfType<EnemySpawner>();
            WalkPoint = this.transform.position;
            ChronoHelper.OnChronoEffectStarted += OnChronoEffectStarted;
            ChronoHelper.OnChronoEffectEnded += OnChronoEffectEnded;
            // if(playerStats != null)
            //     playerStats.HealthDepleted += DelayAndDie;
        }

        private void DelayAndDie(){
            PlayerHealthUI.Instance.StartCoroutine(WaitAndDie());
        }

        IEnumerator WaitAndDie()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 15f));
            ReturnToPool();
        }

        private void OnChronoEffectStarted(float slowDownPercentage)
        {
            AgenSpeed = RegularSpeed * slowDownPercentage;
        }

        private void OnChronoEffectEnded()
        {
            AgenSpeed = RegularSpeed;
        }
        
        private void Look()
        {
            var t = (Camera.main.transform.position - transform.position);
            transform.LookAt(Vector3.ProjectOnPlane(t,Vector3.up ) * t.magnitude,Vector3.up);
        }
        void Update()
        {
            if (Agent != null)
                Agent.speed = AgenSpeed;
            if(!parametersSet) return;

            if (this.WalkableArea == default) return;
            
            Patrol();
            TryAttackPlayer();
            Look();
        }
        public bool CanTakeDamage(object enemyObject) // MAKE THIS FULLY PRIVATE BITCH!! 
        {
            if (this.IsPrime) return false; // prime numbers are healths. Can't take damage.

            if (enemyObject is Grenade || enemyObject is BulletMover) return true;
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
            if (PlayerTransform == null) return;
            var temp = FindObjectsOfType<PlayerTag>();
            if (temp.Length == 0)
            {
                return;
            }

            var enemyToAttack = temp[Random.Range(0, temp.Length)];
            var distanceToEnemy = Vector3.Distance(this.transform.position, enemyToAttack.transform.position);
            if(distanceToEnemy < AttackRadius)
            {
                Agent.SetDestination(enemyToAttack.transform.position);
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
                if(obj is Grenade || obj is BulletMover)
                {
                    // return the enemy to enemy pool.
                    ReturnToPool();
                    FindObjectOfType<PlayerStats>().IncrementKillValue();
                    return true;
                }
            }

            return false;
        }

        public void HandleEnemyDeath()
        {
            if (Value == 1 || IsPrime)
            {
                FindObjectOfType<WaveSpawner>().EnemyDied();
                ReturnToPool();
                return;
            }
            
            if (IsPerfectSquare)
            {
                var sqrt = Value.SquareRoot();
                if (NumberAlgorithms.IsPrime(sqrt))
                { 
                    //Effect / Drop
                    FindObjectOfType<WaveSpawner>().EnemyDied();
                    ReturnToPool();
                    // Instantiate(Resources.Load(sqrt+""), transform.position, transform.rotation);
                    return;
                }

                meraBagwhan.CreateEnemyAt(transform.position,sqrt);
                meraBagwhan.CreateEnemyAt(transform.position,sqrt);
            }
            else if (IsPerfectCube)
            {
                var cbrt = Value.CubeRoot();

                if (NumberAlgorithms.IsPrime(cbrt))
                { 
                    //Effect / Drop
                    FindObjectOfType<WaveSpawner>().EnemyDied();
                    ReturnToPool();
                    // Instantiate(Resources.Load(cbrt+""), transform.position, transform.rotation);
                    return;
                }

                meraBagwhan.CreateEnemyAt(transform.position,cbrt);
                meraBagwhan.CreateEnemyAt(transform.position,cbrt);
                meraBagwhan.CreateEnemyAt(transform.position,cbrt);
            }
            
            FindObjectOfType<WaveSpawner>().EnemyDied();
            ReturnToPool();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            var enteredGameObject = other.collider.gameObject;
            var player = enteredGameObject.GetComponentInParent<PlayerController>();
            if(player != default)
            {
                enteredGameObject.GetComponentInParent<PlayerStats>().TakeDamage(Value);
                // playerStats.IncrementKillValue();
                FindObjectOfType<WaveSpawner>().EnemyDied();
                this.ReturnToPool();
            }
        }

    }

    
}
