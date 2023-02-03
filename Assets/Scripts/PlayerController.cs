using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRef;
    public Animator animatorRef;
    public List<GameObject> bullet;
    public Transform bulletSpawn;
    private Vector3 startPlayerPosition;
    private Vector3 startCameraPosition;
    float verticalInput;
    float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        startPlayerPosition = transform.position;
        startCameraPosition = cameraRef.position;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(0, -Input.mousePosition.x, 0);
        transform.Translate(verticalInput * Time.deltaTime * 20f * Vector3.forward + horizontalInput * Time.deltaTime* 20f * Vector3.right);
        cameraRef.position = startCameraPosition + transform.position - startPlayerPosition;
        animatorRef.SetFloat("moveSpeedX", horizontalInput);
        animatorRef.SetFloat("moveSpeedZ", verticalInput);


        if (Input.GetMouseButton(0))
        {
            Instantiate(bullet[Random.Range(0, 3)], bulletSpawn.position, bulletSpawn.rotation);
        }
    }
}