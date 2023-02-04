using System.Collections;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private Rigidbody rb;
    private WaitForSeconds waitForTwoSeconds = new WaitForSeconds(2f);
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
}
