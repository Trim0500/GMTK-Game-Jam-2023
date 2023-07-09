using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullFruit : MonoBehaviour
{
    public int pointValue = 100;
    public int recoveryValue = 5;
    public GameObject soundEffectPrefab;
    
    private GameManager gameManager;
    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;

    private readonly int _conveyorLayer = 6; //layer index of conveyor

    private Boolean isOnConveyor = false;

    private void Awake()
    {
        gameManager = GameManager.instance;
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!isOnConveyor) return;

        if (transform.position.y > 10)
        {
            Destroy(gameObject); //TODO POOLING?

            gameManager.DecreaseCurrentItemCount(2);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != _conveyorLayer) return;

        if (FruitLifter.instance.liftedBody == _rigidbody2D)
            FruitLifter.instance.liftedBody = null;

        Debug.Log("Applying score and recovery value of: " + pointValue + " and " + recoveryValue);

        gameManager.AddScore(pointValue);
        gameManager.RecoverMeter(recoveryValue);

        isOnConveyor = true;

        var objectToInstantiateIn = GameObject.FindGameObjectWithTag("Sound_Effect_Group");
        Instantiate(soundEffectPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, objectToInstantiateIn.transform);

        _collider2D.enabled = false;

        _rigidbody2D.position = new Vector2(8.5f, transform.position.y);
        _rigidbody2D.velocity = Vector2.up;
    }
}