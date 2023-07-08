using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitHalf : MonoBehaviour
{
    [SerializeField] private FruitType thisFruit;
    [SerializeField] private GameObject fullFruit;

    [SerializeField] private CircleCollider2D _collider2D;
    private Rigidbody2D _rb2d;
    
    public bool isLifted;

    public enum FruitType
    {
        None,
        AppleLeft,
        AppleRight,
        OrangeLeft,
        OrangeRight,
        CoconutLeft,
        CoconutRight,
        DragonfruitLeft,
        DragonfruitRight,
    }
    public FruitType ThisFruitSeeks()
    {
        return thisFruit switch
        {
            FruitType.AppleLeft => FruitType.AppleRight,
            FruitType.AppleRight => FruitType.AppleLeft,
            FruitType.OrangeLeft => FruitType.OrangeRight,
            FruitType.OrangeRight => FruitType.OrangeLeft,
            FruitType.CoconutLeft => FruitType.CoconutRight, 
            FruitType.CoconutRight => FruitType.CoconutLeft, 
            FruitType.DragonfruitLeft => FruitType.DragonfruitRight, 
            FruitType.DragonfruitRight => FruitType.DragonfruitLeft, 
            FruitType.None => FruitType.None,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static FruitType seekingType = FruitType.None;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        // _collider2D = GetComponentInChildren<CircleCollider2D>();
        
        _rb2d.velocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
    }

    private void Update()
    {
        if (!isLifted && thisFruit == seekingType)
        {
            _collider2D.enabled = true;
        }
        else
        {
            _collider2D.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Make sure it's the one being held
        if (!isLifted) return;
        
        Destroy(other.gameObject.transform.parent.gameObject);

        //Instantiate new fruit and put it into lifter
        fullFruit = Instantiate(fullFruit, transform.position, Quaternion.identity);
        FruitLifter.instance.liftedBody = fullFruit.GetComponent<Rigidbody2D>();
        FruitLifter.instance.liftedFruit = null;
        
        Destroy(this.gameObject);
    }
}