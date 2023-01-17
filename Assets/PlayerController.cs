using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _directionX;

    private float _directionY;

    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private GameObject _spriteObject;

    [SerializeField]
    private float _speed;

    private float _normalizeConstant = 0.707106781f;

    public GameManager GameManager;

   
    void Update()
    {
        bool left=Input.GetKey(KeyCode.A);
        bool right= Input.GetKey(KeyCode.D);
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);


        if(left || right)
        {
            if(left && right)
            {
                _directionX = 0;
            }else if (left)
            {
                _directionX = -1;
            }
            else
            {
                _directionX = 1;
            }
        }
        else
        {
            _directionX = 0;
        }



        if(up || down)
        {
            if(up && down)
            {
                _directionY = 0;
            }else if (up)
            {
                _directionY = 1;
            }
            else
            {
                _directionY = -1;
            }
        }
        else
        {
            _directionY = 0;
        }

        if ((_spriteObject.transform.localScale.x > 0 && _directionX>0) || (_spriteObject.transform.localScale.x < 0 && _directionX < 0))
        {
            _spriteObject.transform.localScale = new Vector3(_spriteObject.transform.localScale.x*-1, _spriteObject.transform.localScale.y, _spriteObject.transform.localScale.z);
        }

    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_directionX) > 0.5f && Mathf.Abs(_directionY) > 0.5f)
        {
            _directionX *= _normalizeConstant;
            _directionY *= _normalizeConstant;
        }

        _rigidbody.velocity = new Vector2(_directionX, _directionY) * _speed * Time.fixedDeltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.ShowLoseCanvas();
        }
    }
}
