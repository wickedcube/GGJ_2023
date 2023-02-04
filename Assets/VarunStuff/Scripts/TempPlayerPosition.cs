using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerPosition : MonoBehaviour
{

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask ArenaMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            var results = Physics.RaycastAll(ray, float.MaxValue, ArenaMask);
            foreach(var result in results)
            {
                this.transform.position = result.point + new Vector3(0 , this.transform.localScale.y /2 , 0);

            }
        }
    }


}
