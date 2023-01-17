using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ShurikenController _shurikenController;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private EnemyController _enemyController;

    private Dictionary<int,EnemyController> _spawnedEnemies;

    private List<EnemyController> _inactiveEnemies;

    private Queue<ShurikenController> _activeShurikens;

    private List<ShurikenController> _inactiveShurikens;

    private int _spawnedEnemyLimit=200;

    private float _maxEnemySpawnTime = 0.3f;

    private float _currentEnemySpawnTime;

    private float _maxShurikenSpawnTime = 0.4f;

    private float _currentShurikenSpawnTime;

    private float _spawnMinLimit = 3f;

    private float _spawnConstant=5f;

    private int _spawnedEmemyCount = 0;

   
    private System.Random _randomGenerator;

    [SerializeField]
    private LoseMenu loseMenu;

    void Start()
    {
        _currentEnemySpawnTime = _maxEnemySpawnTime;
        _currentShurikenSpawnTime = _maxShurikenSpawnTime;
        
        _randomGenerator = new System.Random();

        _spawnedEnemies = new Dictionary<int,EnemyController>();

        _inactiveEnemies = new List<EnemyController>();

        _activeShurikens = new Queue<ShurikenController>();

        _inactiveShurikens = new List<ShurikenController>();

    }

    // Update is called once per frame
    void Update()
    {
        _currentShurikenSpawnTime -= Time.deltaTime;

        _currentEnemySpawnTime -= Time.deltaTime;

        if (_currentShurikenSpawnTime <= 0)
        {
            float minDistance = 10f;

            EnemyController chosenEnemy = null;

            foreach (EnemyController e in _spawnedEnemies.Values)
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

                _currentShurikenSpawnTime = _maxShurikenSpawnTime;

                if (_inactiveShurikens.Count > 0)
                {
                    toActvivate = _inactiveShurikens[_inactiveShurikens.Count - 1];
                    _inactiveShurikens.RemoveAt(_inactiveShurikens.Count - 1);

                    toActvivate.gameObject.SetActive(true);
                    toActvivate.transform.position = _playerController.transform.position;
                    
                }
                else
                {
                    
                    toActvivate = Instantiate(_shurikenController, _playerController.transform.position, Quaternion.identity);
                    toActvivate.gameManager = this;
                   
                }

                toActvivate.enemyController = chosenEnemy;
                chosenEnemy.spawnLife -= toActvivate.currentDamage;
                _activeShurikens.Enqueue(toActvivate);

            }
        }

        if(_currentEnemySpawnTime <= 0)
        {
            
            if (_spawnedEnemies.Count <= _spawnedEnemyLimit)
            {

                _currentEnemySpawnTime = _maxEnemySpawnTime;
                Vector2 currentSpawnPoint = this.getRandomSpawnPosition();


                if (_inactiveEnemies.Count > 0)
                {
                    EnemyController toActivate = _inactiveEnemies[_inactiveEnemies.Count - 1];
                    _inactiveEnemies.RemoveAt(_inactiveEnemies.Count - 1);
                    toActivate.gameObject.SetActive(true);


                    toActivate.resetValues();
                    toActivate.transform.position = currentSpawnPoint;

                    _spawnedEnemies.Add(toActivate.enemyIndex, toActivate);
                    toActivate.setDistanceFromPlayer();
                    

                }
                else
                {
                    EnemyController instantiatedEnemy = Instantiate(_enemyController, currentSpawnPoint, Quaternion.identity);
                    _spawnedEmemyCount++;

                    instantiatedEnemy.gameManager = this;
                    instantiatedEnemy.player = _playerController;
                    instantiatedEnemy.enemyIndex = _spawnedEmemyCount;
                    _spawnedEnemies.Add(_spawnedEmemyCount, instantiatedEnemy);
                    instantiatedEnemy.setDistanceFromPlayer();
                }
                
            }

        }

    }


    public Vector2 getRandomSpawnPosition()
    {
        float spawnX = (float)(_randomGenerator.NextDouble() - 0.5);
        float spawnY = (float)(_randomGenerator.NextDouble() - 0.5);

        if (spawnX >= 0)
        {
            spawnX = spawnX * _spawnConstant + _spawnMinLimit;
        }
        else
        {
            spawnX = spawnX * _spawnConstant - _spawnMinLimit;
        }


        if (spawnY >= 0)
        {
            spawnY = spawnY * _spawnConstant + _spawnMinLimit;
        }
        else
        {
            spawnY = spawnY * _spawnConstant - _spawnMinLimit;
        }

        return new Vector2(_playerController.transform.position.x + spawnX, _playerController.transform.position.y + spawnY);

    }
    

    public void RemoveEnemy(int index)
    {

        EnemyController toRemove = _spawnedEnemies[index];
        _spawnedEnemies.Remove(index);
        toRemove.gameObject.SetActive(false);
        
        _inactiveEnemies.Add(toRemove);

    }

    public void RemoveShuriken()
    {

        ShurikenController toRemove = this._activeShurikens.Dequeue();
        toRemove.gameObject.SetActive(false);
        _inactiveShurikens.Add(toRemove);

    }

    public void ShowLoseCanvas()
    {
        Time.timeScale = 0;
        loseMenu.gameObject.SetActive(true);

    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
