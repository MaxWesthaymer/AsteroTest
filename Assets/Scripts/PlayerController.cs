using UniRx;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    #endregion

    #region PrivateFields
    private Camera _mainCamera;
    private CapsuleCollider _collider;
    private float _screenWidthInWorldsUnits;
    private float _screenHeightInWorldsUnits;
    private float _playerHeightHalf;
    private float _speed;
    private float _fireRate;
    private float _cooldown;
    private bool _gameIsEnd;
    #endregion
    
    #region Propierties
    public IntReactiveProperty LivesCount {get; private set;}
    public BoolReactiveProperty IsDead {get; private set;}
    #endregion

    #region PublicMethods
    public void SetShipParameters(float speed, float fireRate, int livesCount)
    {
        _speed = speed;
        _fireRate = fireRate;
        LivesCount = new IntReactiveProperty(livesCount);
        IsDead = new BoolReactiveProperty(false);
    }

    public void SetEndGame()
    {
        _gameIsEnd = true;
        joystick.gameObject.SetActive(false);
        _collider.enabled = false;
    }
    #endregion
    
    
    #region UnityMethods
    private void Start()
    {
        _mainCamera = Camera.main;
        _collider = GetComponent<CapsuleCollider>();
        CalculateScreenSize();
    }
    
    private  void Update()
    {
        if (IsDead.Value || _gameIsEnd)
        {
            return;
        }
        PlayerMove();
        PlayerShoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            DecrementLives();
        }
    }
    #endregion
    
    #region PrivateMethods

    private void CalculateScreenSize()
    {
        _screenWidthInWorldsUnits = _mainCamera.aspect * _mainCamera.orthographicSize;
        _screenHeightInWorldsUnits = _mainCamera.orthographicSize;
        _playerHeightHalf = _collider.height / 2;
    }

    private void PlayerMove()
    {
        var input = new Vector3(joystick.Horizontal, joystick.Vertical, 0);
        transform.position += input.normalized * (_speed * Time.deltaTime);
        BoundsControl();
    }
    
    private void BoundsControl()
    {
        var currentPosition = transform.position;
        transform.position = new Vector3(Mathf.Clamp(currentPosition.x, -_screenWidthInWorldsUnits , _screenWidthInWorldsUnits), 
            Mathf.Clamp(currentPosition.y, -_screenHeightInWorldsUnits + _playerHeightHalf, _screenHeightInWorldsUnits - _playerHeightHalf),currentPosition.z);
    }
    
    
    private void PlayerShoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (_cooldown <= 0)
            {
                Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                _cooldown = _fireRate;
            }

            _cooldown -= Time.deltaTime;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            _cooldown = 0;
        }
    }
    
    
    private void DecrementLives()
    {
        LivesCount.Value--;
        if (LivesCount.Value <= 0)
        {
            IsDead.Value = true;
            gameObject.SetActive(false);
        }
    }
    #endregion
    
}
