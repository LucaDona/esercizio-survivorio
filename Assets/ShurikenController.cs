using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
 
    [SerializeField]
    private float speed;

    public EnemyController enemyController; 

    private float raycastDistance = 0.02f;

    public int currentDamage = 4;

    public GameManager gameManager;


    void Update()
    {
      
        float distanceX = transform.position.x - enemyController.transform.position.x;
        float distanceY = transform.position.y - enemyController.transform.position.y;

        float ipotenusa = Mathf.Sqrt((distanceX * distanceX + distanceY * distanceY));

        float directionX = distanceX / ipotenusa * speed * Time.deltaTime * -1;
        float directionY = distanceY / ipotenusa * speed * Time.deltaTime * -1;

        transform.position = new Vector2(transform.position.x + directionX, transform.position.y + directionY);

        Vector2 raycastDirection = new Vector3(directionX, directionY);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastDistance);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.transform.position.x== enemyController.transform.position.x && hit.collider.gameObject.transform.position.y == enemyController.transform.position.y)
            {

                enemyController.currentLife -= currentDamage;
                
                //this.gameObject.SetActive(false);
                if (enemyController.currentLife<=0)
                {
                   
                    gameManager.removeEnemy(enemyController.enemyIndex);
                   
                }

                //Destroy(this.gameObject);
                gameManager.removeShuriken();

            }

        }


    }
}
