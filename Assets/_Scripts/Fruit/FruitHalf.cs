using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitHalf : MonoBehaviour
{
    [SerializeField] private FruitType thisFruit;
    [SerializeField] private GameObject fullFruit;
    [SerializeField] private GameObject soundEffectPrefab;
    [SerializeField] private GameManager gameManager;

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

        gameManager = GameManager.instance;
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

        gameManager.DecreaseCurrentItemCount(1);

        //Instantiate new fruit and put it into lifter
        fullFruit = Instantiate(fullFruit, transform.position, Quaternion.identity);
        FruitLifter.instance.liftedBody = fullFruit.GetComponent<Rigidbody2D>();
        FruitLifter.instance.liftedFruit = null;
        
        var objectToInstantiateIn = GameObject.FindGameObjectWithTag("Sound_Effect_Group");
        Instantiate(soundEffectPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, objectToInstantiateIn.transform);

        Destroy(this.gameObject);
    }
}