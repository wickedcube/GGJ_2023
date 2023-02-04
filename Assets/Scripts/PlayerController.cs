using System.Collections;
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
    public float movementSpeed;
    public float rotationSpeed;
    public float timeBetweenShots;
    private Vector3 startPlayerPosition;
    private Vector3 startCameraPosition;
    private Rigidbody rb;
    float verticalInput;
    float horizontalInput;
    float lastLeftShot;
    float lastRightShot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPlayerPosition = transform.position;
        startCameraPosition = cameraRef.position;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(0, -Input.mousePosition.x* rotationSpeed, 0);
        // rb.MovePosition(transform.position + verticalInput * Time.deltaTime * movementSpeed * Vector3.forward + horizontalInput * Time.deltaTime * movementSpeed * Vector3.right);
        // transform.Translate(verticalInput * Time.deltaTime * movementSpeed * Vector3.forward + horizontalInput * Time.deltaTime * movementSpeed * Vector3.right);
        rb.velocity = new Vector3(horizontalInput * movementSpeed, 0, verticalInput * movementSpeed);
        // var t = rb.velocity.y;
        //var vec = verticalInput  * movementSpeed * Vector3.forward + horizontalInput * movementSpeed * Vector3.right;
        // vec.y = t;
        // rb.velocity = vec;
        cameraRef.position = startCameraPosition + transform.position - startPlayerPosition;
        animatorRef.SetFloat("moveSpeedX", horizontalInput);
        animatorRef.SetFloat("moveSpeedZ", verticalInput);


        if (KeyMappings.GetSquareRootFire() && Time.time > lastLeftShot + timeBetweenShots)
        {
            lastLeftShot = Time.time;
            Instantiate(bullet1[Random.Range(0, bullet1.Count)], bulletSpawn1.position, bulletSpawn1.rotation);
            animatorRef.SetBool("isShooting", true);
        }
        else if(KeyMappings.GetCubeRootFire() && Time.time > lastRightShot + timeBetweenShots)
        { 
            lastRightShot = Time.time;
            Instantiate(bullet2[Random.Range(0, bullet1.Count)], bulletSpawn2.position, bulletSpawn2.rotation);
            animatorRef.SetBool("isShooting", true);
        }
        else
        { 
            animatorRef.SetBool("isShooting", false);
        }
    }
}