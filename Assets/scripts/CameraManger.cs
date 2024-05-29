using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManger : MonoBehaviour
{
    public Vector3 startPosition = new Vector3(58f, 5f, 3f);
    Camera _camera;
    public float speed = 10f;
    public static bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        _camera = transform.GetComponent<Camera>();
        transform.position = startPosition;
        _camera.fieldOfView = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(start == true)
        {
            Move();
            ScaleView();
        }
    }
    private void Move()
    {
        if (Input.GetMouseButton(0))
        {
            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");
            if (x != 0 || y != 0)
            {
                Vector3 target = this.transform.position + new Vector3(y, 0, x) * speed;
                this.transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            }
        }
    }
    private void ScaleView()
    {
        float mouseScro = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScro != 0)
        {
            _camera.fieldOfView += mouseScro * speed;
            if (_camera.fieldOfView <= 38) { _camera.fieldOfView = 38; }
            else if(_camera.fieldOfView >=95 ) { _camera.fieldOfView =95; }
        }
    }
}
