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
            Instantiate(bullet1[Random.Range(0, bullet1.Count)], bulletSpawn1.position, bulletSpawn1.rotation);
        }

        if (Input.GetMouseButton(1))
        {
            Instantiate(bullet2[Random.Range(0, bullet1.Count)], bulletSpawn2.position, bulletSpawn2.rotation);
        }
    }
}