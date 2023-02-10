using Enemy;
using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using Coherence.Connection;
using Coherence.Toolkit;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRef;
    public Animator animatorRef;
    public List<GameObject> bullet1;
    public List<GameObject> bullet2;
    public Transform bulletSpawn1;
    public Transform bulletSpawn2;
    public float movementSpeed;
    public float rotationSpeed;
    public float timeBetweenShots;
    public GameObject gunShotVFX;
    private Vector3 startPlayerPosition;
    private Vector3 startCameraPosition;
    private Rigidbody rb;
    private PlayerStats playerStats;
    float verticalInput;
    float horizontalInput;
    float lastLeftShot;
    float lastRightShot;
	public List<TMPro.TMP_Text> playerNameText;
	public CoherenceMonoBridge MonoBridge;

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

		playerNameText[0].text=LeaderboardHandler.Instance.PlayerData.username;
		playerNameText[1].text=LeaderboardHandler.Instance.PlayerData.username;
    }

    void Update()
    {
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraRay, out cameraRayHit, 1<<7))
        {
            Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
            if(Vector3.Distance(targetPosition, transform.position) > 0.5f)
            transform.LookAt(targetPosition);
        }

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = verticalInput * movementSpeed * verticalVec + horizontalInput * movementSpeed * horizontalVec;
        cameraRef.position = startCameraPosition + transform.position - startPlayerPosition;
        animatorRef.SetFloat("moveSpeedX", horizontalInput);
        animatorRef.SetFloat("moveSpeedZ", verticalInput);


        if (KeyMappings.GetSquareRootFire() && Time.time > lastLeftShot + timeBetweenShots)
        {
            lastLeftShot = Time.time;
            var b = Instantiate(bullet1[Random.Range(0, bullet1.Count)], bulletSpawn1.position, Quaternion.identity);
            b.transform.forward = bulletSpawn1.forward;
            animatorRef.SetBool("isShooting", true);
            var e = Instantiate(gunShotVFX, bulletSpawn1.position, bulletSpawn1.rotation);
            e.transform.SetParent(bulletSpawn1);
        }
        else if (KeyMappings.GetCubeRootFire() && Time.time > lastRightShot + timeBetweenShots)
        {
            lastRightShot = Time.time;
            var b = Instantiate(bullet2[Random.Range(0, bullet1.Count)], bulletSpawn2.position, Quaternion.identity);
            b.transform.forward = bulletSpawn2.forward;
            animatorRef.SetBool("isShooting", true);
            var e = Instantiate(gunShotVFX, bulletSpawn2.position, bulletSpawn2.rotation);
            e.transform.SetParent(bulletSpawn2);
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