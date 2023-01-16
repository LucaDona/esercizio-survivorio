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


        if (enemySprite.transform.localScale.x > 0 && directionX > 0)
        {
            enemySprite.transform.localScale = new Vector3(enemySprite.transform.localScale.x * -1, enemySprite.transform.localScale.y, enemySprite.transform.localScale.z);
        }
        else if (enemySprite.transform.localScale.x < 0 && directionX < 0)
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
