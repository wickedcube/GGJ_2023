using Enemy;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRef;
    public Animator animatorRef;
    public List<GameObject> bullet1;
    public List<GameObject> bullet2;
    public Transform bulletSpawn1;
    public Transform bulletSpawn2;
    public Transform indicatorTransform;
    public float movementSpeed;
    public float rotationSpeed;
    public float timeBetweenShots;
    private Vector3 startPlayerPosition;
    private Vector3 startCameraPosition;
    private Rigidbody rb;
    private PlayerStats playerStats;
    float verticalInput;
    float horizontalInput;
    float lastLeftShot;
    float lastRightShot;

    Ray cameraRay;                // The ray that is cast from the camera to the mouse position
    RaycastHit cameraRayHit;

    private Vector3 verticalVec;
    private Vector3 horizontalVec;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        startPlayerPosition = transform.position;
        startCameraPosition = cameraRef.position;
        verticalVec = -(Vector3.right + Vector3.back).normalized;
        horizontalVec = (Vector3.forward + Vector3.right).normalized;
    }

    void Update()
    {
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraRay, out cameraRayHit, 1<<7))
        {
            Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
            transform.LookAt(targetPosition);
        }

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = verticalInput * movementSpeed * verticalVec + horizontalInput * movementSpeed * horizontalVec;
        cameraRef.position = startCameraPosition + transform.position - startPlayerPosition;
        // animatorRef.SetFloat("moveSpeedX", horizontalInput);
        animatorRef.SetFloat("moveSpeedZ", verticalInput);


        if (KeyMappings.GetSquareRootFire() && Time.time > lastLeftShot + timeBetweenShots)
        {
            lastLeftShot = Time.time;
            var b = Instantiate(bullet1[Random.Range(0, bullet1.Count)], indicatorTransform.position, Quaternion.identity);
            b.transform.forward = indicatorTransform.forward;
            animatorRef.SetBool("isShooting", true);
        }
        else if (KeyMappings.GetCubeRootFire() && Time.time > lastRightShot + timeBetweenShots)
        {
            lastRightShot = Time.time;
            var b = Instantiate(bullet2[Random.Range(0, bullet1.Count)], indicatorTransform.position, Quaternion.identity);
            b.transform.forward = indicatorTransform.forward;
            animatorRef.SetBool("isShooting", true);
        }
        else
        {
            animatorRef.SetBool("isShooting", false);
        }
    }

    public void TakeDamage(INumberEnemy enemy)
    {

        if (enemy is EnemyBehavior eb)
        {
            Debug.Log($" Damage Taken !! {eb.gameObject.name}");

        }
    }
}