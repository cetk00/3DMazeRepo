using UnityEngine;

public class MoveForward : MonoBehaviour
{

    public float speed = 40.0f;
    private Vector3 _direction;
    

    void Start()
    {
        if (_direction == Vector3.zero)
            _direction = transform.forward;
    }

    public void setDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0, vertical);

        transform.Translate(_direction * Time.deltaTime * speed, Space.World);
    }
}
