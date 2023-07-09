using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public float spawnerTimer = 2.0f;
    public GameObject itemToCreate;
    public int maxSpawnCount = 5;
    public float spawnPointOffset = 10.0f;
    public GameObject soundEffectPrefab;
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

            var spawnerPositions = this.GetComponentsInChildren<Transform>();

            var newOffset = spawnPointOffset + randInt;
            var newItemPosition = new Vector2(spawnerPositions[0].position.x + newOffset, spawnerPositions[0].position.y + newOffset);

            if(newItemPosition.x < leftLimit.transform.position.x + 0.5f || newItemPosition.x > rightLimit.transform.position.x - 0.5f)
            {
                newItemPosition.x = spawnerPositions[0].position.x;
            }

            if(newItemPosition.y < topLimit.transform.position.y - 0.5f || newItemPosition.y > bottomLimit.transform.position.y + 0.5f)
            {
                newItemPosition.y = spawnerPositions[0].position.y;
            }

            var objectToInstantiateIn = GameObject.FindGameObjectWithTag("Fruit_Object_Group");

            Debug.Log("Attempting to make sound effect object: " + soundEffectPrefab);

            Instantiate(itemToCreate, newItemPosition, itemToCreate.transform.rotation, objectToInstantiateIn.transform);
            Instantiate(soundEffectPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

            ++spawnCount;

            _gameManager.IncreasesCurrentItemCount(1);
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
