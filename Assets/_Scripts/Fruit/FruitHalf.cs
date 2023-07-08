using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitHalf : MonoBehaviour
{
    [SerializeField] private FruitType thisFruit;

    private Rigidbody2D _rb2d;
    private CircleCollider2D _collider2D;
    
    public enum FruitType
    {
        None,
        AppleLeft,
        AppleRight,
        OrangeLeft,
        OrangeRight,
    }
    public FruitType ThisFruitSeeks()
    {
        return thisFruit switch
        {
            FruitType.AppleLeft => FruitType.AppleRight,
            FruitType.AppleRight => FruitType.AppleLeft,
            FruitType.OrangeLeft => FruitType.OrangeRight,
            FruitType.OrangeRight => FruitType.OrangeLeft,
            FruitType.None => FruitType.None,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static FruitType seekingType = FruitType.None;
    
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CircleCollider2D>();
        
        _rb2d.velocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
    }

    private void Update()
    {
        if (thisFruit == seekingType)
        {
            _collider2D.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        
        Debug.Log("I'm full now!");
    }
}