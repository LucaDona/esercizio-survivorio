using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private float directionX=0;

    private float directionY = 0;

    [SerializeField]
    private Rigidbody2D rigidbody;

    [SerializeField]
    private GameObject spriteObject;

    [SerializeField]
    private float speed;

    private float normalizeConstant = 0.707106781f;

    public GameManager gameManager;

    
    
    void Start()
    {
        
    }


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
                directionX = 0;
            }else if (left)
            {
                directionX = -1;
            }
            else
            {
                directionX = 1;
            }
        }
        else
        {
            directionX = 0;
        }



        if(up || down)
        {
            if(up && down)
            {
                directionY = 0;
            }else if (up)
            {
                directionY = 1;
            }
            else
            {
                directionY = -1;
            }
        }
        else
        {
            directionY = 0;
        }

        if (spriteObject.transform.localScale.x > 0 && directionX>0)
        {
            spriteObject.transform.localScale = new Vector3(spriteObject.transform.localScale.x*-1, spriteObject.transform.localScale.y, spriteObject.transform.localScale.z);
        }else if(spriteObject.transform.localScale.x <0 && directionX < 0)
        {
            spriteObject.transform.localScale = new Vector3(spriteObject.transform.localScale.x * -1, spriteObject.transform.localScale.y, spriteObject.transform.localScale.z);
        }

      
        if (Mathf.Abs(directionX)>0.5f && Mathf.Abs(directionY) > 0.5f)
        {
            
            directionX *= normalizeConstant;
            directionY *= normalizeConstant;

        }

    }

    private void FixedUpdate()
    {

        
        rigidbody.velocity = new Vector2(directionX, directionY) * speed * Time.fixedDeltaTime;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
           
            gameManager.showLoseCanvas();


        }
    }
}
