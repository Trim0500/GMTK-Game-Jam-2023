using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float detonationTime = 4.0f;
    [SerializeField] private float blastRadius = 3f;
    [SerializeField] private float blastModif = 1f;
    [SerializeField] private GameObject fuseSoundEffectPrefab;
    [SerializeField] private GameObject explosionSoundEffectPrefab;

    private Animator _animator;
    private Rigidbody2D _rigidbody2d;
    private CircleCollider2D _circleCollider;
    private GameManager _gameManager;

    private float _timer;
    private bool _exploding;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _gameManager = GameManager.instance;

        var objectToInstantiateIn = GameObject.FindGameObjectWithTag("Sound_Effect_Group");
        Instantiate(fuseSoundEffectPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, objectToInstantiateIn.transform);
    }

    void Update()
    {
        if (_exploding) return;

        if (_timer > detonationTime)
        {
            _animator.Play("bomb_explode");
            StartCoroutine(Explode());
        }
        else if (_timer > detonationTime / 2)
        {
            _animator.Play("bomb_fast");
        }

        _timer += Time.deltaTime;
    }

    IEnumerator Explode()
    {
        _exploding = true;
        yield return new WaitForSeconds(1.0f);

        Destroy(fuseSoundEffectPrefab);

        var objectToInstantiateIn = GameObject.FindGameObjectWithTag("Sound_Effect_Group");
        Instantiate(explosionSoundEffectPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, objectToInstantiateIn.transform);

        if (FruitLifter.instance.liftedBody == _rigidbody2d)
            FruitLifter.instance.liftedBody = null;

        _circleCollider.enabled = false;
        
        var pos = transform.position;
        var hitObjs = Physics2D.OverlapCircleAll(pos, blastRadius);

        foreach (Collider2D obj in hitObjs)
        {
            Debug.LogWarning((obj.transform.position - pos).magnitude + " / " + obj.transform.position + " - " + pos);
            obj.GetComponent<Rigidbody2D>().AddForce((obj.transform.position - pos) * blastModif, ForceMode2D.Impulse);
        }
        
        yield return new WaitForSeconds(1.7f);
        Destroy(gameObject);

        _gameManager.DecreaseCurrentItemCount(1);
    }
}