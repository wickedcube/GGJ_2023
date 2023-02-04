using System.Collections;
using UnityEngine;
using Enemy;

public class BulletMover : MonoBehaviour
{
    private Rigidbody rb;
    private WaitForSeconds waitForTwoSeconds = new WaitForSeconds(2f);
    public bool isSquareRootBullet;
    public bool isCubeRootBullet;
    // Start is called before the first frame update
    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator Start()
    {
        rb.AddForce(transform.forward*50, ForceMode.Impulse);
        yield return waitForTwoSeconds;
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        var enteredGameObject = other.collider.gameObject;
        var enemy = enteredGameObject.GetComponent<EnemyBehavior>();
        if(enemy != default)
        {
            if ((isCubeRootBullet && enemy.IsPerfectCube)  || (isSquareRootBullet && enemy.IsPerfectSquare))
            { 
                enemy.ReturnToPool();
            }
        }
    }
}
