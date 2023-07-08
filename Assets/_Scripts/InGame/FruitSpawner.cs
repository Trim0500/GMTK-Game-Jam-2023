using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public float spawnerTimer = 2.0f;
    public GameObject itemToCreate;
    public int maxSpawnCount = 5;
    public float spawnPointOffset = 10.0f;
    public GameObject spawnSoundEffect;
    public GameManager _gameManager;
    public GameObject leftLimit;
    public GameObject rightLimit;
    public GameObject topLimit;
    public GameObject bottomLimit;
    public float cooldownTimer = 10.0f;

    private int spawnCount = 0;
    private bool cooldown = false;

    private IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTimer);

        cooldown = false;

        spawnCount = 0;
    }
    
    public void SpawnItems()
    {
        Debug.Log("Checking if there are too many objects");

        var canSpawn = _gameManager.CheckForMaxPieces();

        Debug.Log("Value of canSPawn is: " + canSpawn);
        if(!cooldown && canSpawn && spawnCount < maxSpawnCount)
        {
            var randInt = Random.Range(0.0f, 5.0f);
            var remainderValue = (int)randInt % 2;
            if(remainderValue == 0)
            {
                randInt = randInt * -1;
            }

            var spawnerPosition = this.gameObject.transform.position;

            var newOffset = spawnPointOffset + randInt;
            var newPosition = new Vector2(spawnerPosition.x + newOffset, spawnerPosition.y + newOffset);

            if(newPosition.x < leftLimit.transform.position.x || newPosition.y > rightLimit.transform.position.x)
            {
                newPosition.x = this.gameObject.transform.position.x;
            }

            if(newPosition.y < leftLimit.transform.position.y || newPosition.y > rightLimit.transform.position.y)
            {
                newPosition.y = this.gameObject.transform.position.y;
            }

            var objectToInstantiateIn = GameObject.FindGameObjectWithTag("Fruit_Object_Group");

            Instantiate(itemToCreate, newPosition, itemToCreate.transform.rotation, objectToInstantiateIn.transform);

            ++spawnCount;

            _gameManager.IncreasesCurrentItemCount();
        }
        else if(!cooldown && spawnCount == maxSpawnCount)
        {
            cooldown = true;

            StartCoroutine("CooldownTimer");
        }
    }

    private void Start()
    {
        InvokeRepeating("SpawnItems", 2.0f, spawnerTimer);
    }
}
