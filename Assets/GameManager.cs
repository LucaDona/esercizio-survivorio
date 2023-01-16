using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ShurikenController shurikenController;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private EnemyController enemyController;

    private Dictionary<int,EnemyController> spawnedEnemies;

    private List<EnemyController> inactiveEnemies;

    private Queue<ShurikenController> activeShurikens;

    private List<ShurikenController> inactiveShurikens;

    private int spawnedEnemyLimit=20;

    private float maxEnemySpawnTime = 1.5f;

    private float currentEnemySpawnTime;

    private float maxShurikenSpawnTime = 0.4f;

    private float currentShurikenSpawnTime;

    private float spawnMinLimit = 3f;

    private float spawnConstant=5f;

    private int sawnedEmemyCount = 0;

    private System.Random randomGenerator;

    [SerializeField]
    private LoseMenu loseMenu;

    void Start()
    {
        currentEnemySpawnTime = maxEnemySpawnTime;
        currentShurikenSpawnTime = maxShurikenSpawnTime;

        randomGenerator = new System.Random();

        spawnedEnemies = new Dictionary<int,EnemyController>();

        inactiveEnemies = new List<EnemyController>();

        activeShurikens = new Queue<ShurikenController>();

        inactiveShurikens = new List<ShurikenController>();

    }

    // Update is called once per frame
    void Update()
    {
        currentShurikenSpawnTime -= Time.deltaTime;

        currentEnemySpawnTime -= Time.deltaTime;

        if (currentShurikenSpawnTime <= 0)
        {
            float minDistance = 10f;

            EnemyController chosenEnemy = null;

            foreach(EnemyController e in spawnedEnemies.Values)
            {
                if(e.distanceFromPlayer <= minDistance && e.spawnLife>0)
                {
                    chosenEnemy = e;
                    minDistance = e.distanceFromPlayer;
                }
            }

            if (chosenEnemy != null)
            {
                ShurikenController toActvivate;

                currentShurikenSpawnTime = maxShurikenSpawnTime;

                if (inactiveShurikens.Count > 0)
                {
                    toActvivate = inactiveShurikens[inactiveShurikens.Count - 1];
                    inactiveShurikens.RemoveAt(inactiveShurikens.Count - 1);

                    toActvivate.gameObject.SetActive(true);
                    toActvivate.transform.position = playerController.transform.position;
                    


                }
                else
                {
                    
                    toActvivate = Instantiate(shurikenController, playerController.transform.position, Quaternion.identity);
                    toActvivate.gameManager = this;
                   
                }

                toActvivate.enemyController = chosenEnemy;
                chosenEnemy.spawnLife -= toActvivate.currentDamage;
                activeShurikens.Enqueue(toActvivate);

            }
        }

        if(currentEnemySpawnTime <= 0)
        {
            
            if (spawnedEnemies.Count <= spawnedEnemyLimit)
            {

                currentEnemySpawnTime = maxEnemySpawnTime;
                Vector2 currentSpawnPoint = this.getRandomSpawnPosition();


                if (inactiveEnemies.Count > 0)
                {
                    EnemyController toActivate = inactiveEnemies[inactiveEnemies.Count - 1];
                    inactiveEnemies.RemoveAt(inactiveEnemies.Count - 1);
                    toActivate.gameObject.SetActive(true);


                    toActivate.resetValues();
                    toActivate.transform.position = currentSpawnPoint;

                    spawnedEnemies.Add(toActivate.enemyIndex, toActivate);
                    toActivate.setDistanceFromPlayer();

                }
                else
                {
                    EnemyController instantiatedEnemy = Instantiate(enemyController, currentSpawnPoint, Quaternion.identity);
                    sawnedEmemyCount++;

                    instantiatedEnemy.gameManager = this;
                    instantiatedEnemy.player = playerController;
                    instantiatedEnemy.enemyIndex = sawnedEmemyCount;
                    spawnedEnemies.Add(sawnedEmemyCount, instantiatedEnemy);
                    instantiatedEnemy.setDistanceFromPlayer();
                }
                
            }

        }

    }


    public Vector2 getRandomSpawnPosition()
    {
        float spawnX = (float)(randomGenerator.NextDouble() - 0.5);
        float spawnY = (float)(randomGenerator.NextDouble() - 0.5);

        if (spawnX >= 0)
        {
            spawnX = spawnX * spawnConstant + spawnMinLimit;
        }
        else
        {
            spawnX = spawnX * spawnConstant - spawnMinLimit;
        }


        if (spawnY >= 0)
        {
            spawnY = spawnY * spawnConstant + spawnMinLimit;
        }
        else
        {
            spawnY = spawnY * spawnConstant - spawnMinLimit;
        }

        return new Vector2(playerController.transform.position.x + spawnX, playerController.transform.position.y + spawnY);

    }
    

    public void removeEnemy(int index)
    {

        EnemyController toRemove = spawnedEnemies[index];
        spawnedEnemies.Remove(index);
        toRemove.gameObject.SetActive(false);
        inactiveEnemies.Add(toRemove);

    }

    public void removeShuriken()
    {

        ShurikenController toRemove = this.activeShurikens.Dequeue();
        toRemove.gameObject.SetActive(false);
        inactiveShurikens.Add(toRemove);

    }

    public void showLoseCanvas()
    {
        Time.timeScale = 0;
        loseMenu.gameObject.SetActive(true);

    }


    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
