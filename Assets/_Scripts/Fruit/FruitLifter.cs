using UnityEngine;
using UnityEngine.Serialization;

public class FruitLifter : MonoBehaviour
{
    [SerializeField] private float lockoutTime = 0.50f;
    [SerializeField] private Camera mainCamera;
    private InputManager _inputManager;

    public Rigidbody2D liftedBody;
    public FruitHalf liftedFruit;

    private Vector3 _offset;
    private Vector3 _mousePosition;

    private float _lockoutTimer;

    public static FruitLifter instance;
    LayerMask _mask;
    
    private void Start()
    {
        _mask = ~LayerMask.GetMask("IgnoreHalfFruit");
        
        _inputManager = InputManager.instance;
        instance = this;
    }

    void Update()
    {
        _mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(
            _inputManager.horizontalLookAxis,
            _inputManager.verticalLookAxis)
        );

        // holding with no fruit & not locked out 
        if (_inputManager.selectHeld && !liftedBody && _lockoutTimer < lockoutTime)
        {
            _lockoutTimer += Time.deltaTime;

            Collider2D targetObject = Physics2D.OverlapPoint(_mousePosition, _mask);
            if (!targetObject) return;

            liftedBody = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();

            liftedFruit = targetObject.transform.gameObject.GetComponent<FruitHalf>();

            if (liftedFruit != null)
            {
                liftedFruit.isLifted = true;
                FruitHalf.seekingType = liftedFruit.ThisFruitSeeks();
            }

            _offset = liftedBody.transform.position - _mousePosition;
        }
        else if (!_inputManager.selectHeld)
        {
            _lockoutTimer = 0.0f;

            FruitHalf.seekingType = FruitHalf.FruitType.None;

            //TODO: possibly optimize
            if (liftedFruit != null)
                liftedFruit.isLifted = false;

            liftedBody = null;
            liftedFruit = null;
        }
    }

    void FixedUpdate()
    {
        if (liftedBody)
        {
            var nextPos = _mousePosition + _offset;

            //Cancel next move if it leaves screen
            if (Mathf.Abs(nextPos.x) > 10 || Mathf.Abs(nextPos.y) > 5.5f)
                return;

            liftedBody.MovePosition(nextPos);
        }
    }
}