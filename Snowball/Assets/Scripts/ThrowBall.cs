using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public float minAngle = 90.0f;
    public float maxAngle = 55.0f;
    public float chargeTime = 3.0f;
    public float maxStrength = 10.0f;
    public GameObject throwable;

    private Transform _camera;
    private Transform _handSocket;
    private bool _throwing = false;
    private float _tCharge = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = transform.parent;
        _handSocket = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_throwing)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _tCharge += Time.deltaTime / chargeTime;
                float armAngle = Mathf.Lerp(minAngle, maxAngle, _tCharge);
                transform.localEulerAngles = new Vector3(armAngle, 0, 0);
            }
            else if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                _throwing = true;
                _tCharge = 0.0f;
                DoThrow(transform.localEulerAngles.x);
            }
        }
        else
        {
            _tCharge += Time.deltaTime / 2 * chargeTime;
            float armAngle = Mathf.Lerp(transform.localEulerAngles.x, minAngle, _tCharge);
            transform.localEulerAngles = new Vector3(armAngle, 0, 0);
            if (armAngle > minAngle - 1)
            {
                _throwing = false;
                armAngle = minAngle;
                transform.localEulerAngles = new Vector3(minAngle, 0, 0);
                _tCharge = 0.0f;
            }
        }
    }
    

    private void DoThrow(float armAngle)
    {
        float normStrength = (armAngle - minAngle) / (maxAngle - minAngle);
        
        GameObject ball = Instantiate(throwable, _handSocket.position, Quaternion.Euler(0,0,0));

        Vector3 throwForce = _camera.forward * normStrength * maxStrength;
        ball.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.Impulse);

        Debug.Log(string.Format("minangle: {0} | maxangle {1} | armangle {2} | strength: {3}", minAngle, maxAngle, armAngle, normStrength));
    }
}
