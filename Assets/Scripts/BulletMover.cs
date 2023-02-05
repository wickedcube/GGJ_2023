using System.Collections;
using UnityEngine;
using Enemy;
using Unity.Mathematics;

public class BulletMover : MonoBehaviour
{

    [SerializeField] private GameObject correctBulletHit;
    [SerializeField] private GameObject wrongBulletHit;
    [SerializeField] private GameObject regularBulletHit;
    
    private Rigidbody rb;
    private WaitForSeconds waitForTwoSeconds = new WaitForSeconds(2f);
    public bool isSquareRootBullet;
    public bool isCubeRootBullet;
    private PlayerStats stats;
    private WaveSpawner waveSpawner;
    // Start is called before the first frame update
    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        stats = FindObjectOfType<PlayerStats>();
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    IEnumerator Start()
    {
        rb.AddForce(transform.forward*50, ForceMode.Impulse);
        yield return waitForTwoSeconds;
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.GetComponent<PlayerController>() != null)
            return;
        
        if (other.collider.gameObject.GetComponent<BulletMover>() != null)
            return;
        
        var enteredGameObject = other.collider.gameObject;
        var enemy = enteredGameObject.GetComponent<EnemyBehavior>();
        if(enemy != default)
        {
            if ((isCubeRootBullet && enemy.IsPerfectCube)  || (isSquareRootBullet && enemy.IsPerfectSquare))
            {
                // enemy.HandleEnemyDeath();
                enemy.TakeDamage(this);
                Instantiate(correctBulletHit, transform.position, quaternion.identity);
            }
            else
            {
                stats.ResetKillCounter();
                Instantiate(wrongBulletHit, transform.position, quaternion.identity);
            }
           
        }
        else
        {
            Instantiate(regularBulletHit, transform.position, quaternion.identity);
        }
        
        Destroy(this.gameObject);
    }
}
