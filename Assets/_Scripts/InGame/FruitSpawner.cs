using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public float spawnerTimer = 2.0f;
    public GameObject leftItemToCreate;
    public GameObject rightItemToCreate;
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
            var newLeftItemPosition = new Vector2(spawnerPositions[0].position.x + newOffset, spawnerPositions[0].position.y + newOffset);
            var newRightItemPosition = new Vector2(spawnerPositions[1].position.x + newOffset, spawnerPositions[1].position.y + newOffset);

            if(newLeftItemPosition.x < leftLimit.transform.position.x + 0.5f || newLeftItemPosition.x > rightLimit.transform.position.x - 0.5f)
            {
                newLeftItemPosition.x = spawnerPositions[0].position.x;
            }

            if(newLeftItemPosition.y < topLimit.transform.position.y - 0.5f || newLeftItemPosition.y > bottomLimit.transform.position.y + 0.5f)
            {
                newLeftItemPosition.y = spawnerPositions[0].position.y;
            }

            if (newRightItemPosition.x < leftLimit.transform.position.x + 0.5f || newRightItemPosition.x > rightLimit.transform.position.x - 0.5f)
            {
                newRightItemPosition.x = spawnerPositions[1].position.x;
            }

            if (newRightItemPosition.y < topLimit.transform.position.y - 0.5f || newRightItemPosition.y > bottomLimit.transform.position.y + 0.5f)
            {
                newRightItemPosition.y = spawnerPositions[1].position.y;
            }

            var objectToInstantiateIn = GameObject.FindGameObjectWithTag("Fruit_Object_Group");

            Debug.Log("Attempting to make sound effect object: " + soundEffectPrefab);

            Instantiate(leftItemToCreate, newLeftItemPosition, leftItemToCreate.transform.rotation, objectToInstantiateIn.transform);
            Instantiate(rightItemToCreate, newRightItemPosition, rightItemToCreate.transform.rotation, objectToInstantiateIn.transform);
            Instantiate(soundEffectPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

            ++spawnCount;

            _gameManager.IncreasesCurrentItemCount(2);
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
