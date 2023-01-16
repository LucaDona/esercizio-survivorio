using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameManager gameManager;

    public PlayerController player;

    [SerializeField]
    private float enemySpeed=1f;

    [SerializeField]
    private GameObject enemySprite;

    public float distanceFromPlayer;

    public int currentLife = 20;

    private int maxLife = 20;

    public int enemyIndex;

    public int spawnLife = 20;

    private float respawnDistance = 35f;

    private float colliderHalfHeight = 0.5f;

    private float colliderHalfWidth = 0.3f;

    private float raycastDistance = 0.1f;

    private float pushSpeed = 0.1f;

    private void Update()
    {

        if (distanceFromPlayer >= respawnDistance)
        {
            this.transform.position = gameManager.getRandomSpawnPosition();
            this.currentLife = maxLife;
            this.spawnLife = maxLife;
        }

        float distanceX = transform.position.x - player.transform.position.x;
        float distanceY = transform.position.y - player.transform.position.y;

        distanceFromPlayer = Mathf.Sqrt((distanceX * distanceX + distanceY * distanceY));

        float directionX = distanceX / distanceFromPlayer * enemySpeed * Time.deltaTime * -1;
        float directionY = distanceY / distanceFromPlayer * enemySpeed * Time.deltaTime *-1;


        //  raycast part

        RaycastHit2D hitHorizontal;
        RaycastHit2D hitVertical;


        if (directionX > 0)
        {
            Vector2 startPosition1 = new Vector2(transform.position.x + colliderHalfWidth+0.01f,transform.position.y+colliderHalfHeight);
            Vector2 startPosition2 = new Vector2(transform.position.x + colliderHalfWidth + 0.01f, transform.position.y - colliderHalfHeight);
            RaycastHit2D hit1 = Physics2D.Raycast(startPosition1, Vector2.right, raycastDistance);
            RaycastHit2D hit2 = Physics2D.Raycast(startPosition2, Vector2.right, raycastDistance);

            
                if((hit1.collider != null && hit1.collider.gameObject.tag== "Enemy") || (hit2.collider != null && hit2.collider.gameObject.tag == "Enemy"))
                {
                    directionX = - pushSpeed*Time.deltaTime;
                }
               
            

        }
        else
        {
            Vector2 startPosition1 = new Vector2(transform.position.x - colliderHalfWidth - 0.01f, transform.position.y + colliderHalfHeight);
            Vector2 startPosition2 = new Vector2(transform.position.x - colliderHalfWidth - 0.01f, transform.position.y - colliderHalfHeight);
            RaycastHit2D hit1 = Physics2D.Raycast(startPosition1, Vector2.left, raycastDistance);
            RaycastHit2D hit2 = Physics2D.Raycast(startPosition2, Vector2.left, raycastDistance);

            if ((hit1.collider != null && hit1.collider.gameObject.tag == "Enemy") || (hit2.collider != null && hit2.collider.gameObject.tag == "Enemy"))
            {
                directionX = pushSpeed * Time.deltaTime;
            }
        }

        if(directionY > 0)
        {
            Vector2 startPosition1 = new Vector2(transform.position.x+colliderHalfWidth, transform.position.y+colliderHalfHeight+0.01f);
            Vector2 startPosition2 = new Vector2(transform.position.x-colliderHalfWidth, transform.position.y + colliderHalfHeight + 0.01f);
            RaycastHit2D hit1 = Physics2D.Raycast(startPosition1, Vector2.up, raycastDistance);
            RaycastHit2D hit2 = Physics2D.Raycast(startPosition2, Vector2.up, raycastDistance);

            if ((hit1.collider != null && hit1.collider.gameObject.tag == "Enemy") || (hit2.collider != null && hit2.collider.gameObject.tag == "Enemy"))
            {
                directionY = -pushSpeed * Time.deltaTime;
            }
        }
        else
        {
            Vector2 startPosition1 = new Vector2(transform.position.x + colliderHalfWidth, transform.position.y - colliderHalfHeight - 0.01f);
            Vector2 startPosition2 = new Vector2(transform.position.x - colliderHalfWidth, transform.position.y - colliderHalfHeight - 0.01f);

            RaycastHit2D hit1 = Physics2D.Raycast(startPosition1, Vector2.down, raycastDistance);
            RaycastHit2D hit2 = Physics2D.Raycast(startPosition2, Vector2.down, raycastDistance);

            if ((hit1.collider != null && hit1.collider.gameObject.tag == "Enemy") || (hit2.collider != null && hit2.collider.gameObject.tag == "Enemy"))
            {
                directionY = pushSpeed * Time.deltaTime;
            }
        }


        if (enemySprite.transform.localScale.x > 0 && distanceX < 0)
        {
            enemySprite.transform.localScale = new Vector3(enemySprite.transform.localScale.x * -1, enemySprite.transform.localScale.y, enemySprite.transform.localScale.z);
        }
        else if (enemySprite.transform.localScale.x < 0 && distanceX > 0)
        {
            enemySprite.transform.localScale = new Vector3(enemySprite.transform.localScale.x * -1, enemySprite.transform.localScale.y, enemySprite.transform.localScale.z);
        }


        transform.position = new Vector2(transform.position.x+ directionX, transform.position.y+ directionY);

    }

    public void setDistanceFromPlayer()
    {
        float distanceX = transform.position.x - player.transform.position.x;
        float distanceY = transform.position.y - player.transform.position.y;

        distanceFromPlayer = Mathf.Sqrt((distanceX * distanceX + distanceY * distanceY));
    }

    public void resetValues()
    {
        this.currentLife = this.maxLife;
        this.spawnLife = this.maxLife;
    }

}
