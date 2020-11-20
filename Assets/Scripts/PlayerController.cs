using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private float _screenWidthInWorldsUnits;
    private float _screenHeightInWorldsUnits;
    private float _playerHeightHalf;
    
    private float _speed;
    private float _fireRate;
    private float _cooldown;
    public IntReactiveProperty LivesCount {get; private set;}
    public BoolReactiveProperty IsDead {get; private set;}

    public void SetShipParameters(float speed, float fireRate, int livesCount)
    {
        _speed = speed;
        _fireRate = fireRate;
        LivesCount = new IntReactiveProperty(livesCount);
        IsDead = new BoolReactiveProperty(false);
    }

    void Start()
    {
        var mainCamera = Camera.main;
        _screenWidthInWorldsUnits = mainCamera.aspect * mainCamera.orthographicSize;
        _screenHeightInWorldsUnits = mainCamera.orthographicSize;
        _playerHeightHalf = GetComponent<CapsuleCollider>().height / 2;
    }
    
    void Update()
    {
        if (IsDead.Value)
        {
            return;
        }
        var input = new Vector3(joystick.Horizontal, joystick.Vertical, 0);
        transform.position += input.normalized * _speed * Time.deltaTime;
        var currentPosition = transform.position;
        transform.position = new Vector3(Mathf.Clamp(currentPosition.x, -_screenWidthInWorldsUnits , _screenWidthInWorldsUnits), 
            Mathf.Clamp(currentPosition.y, -_screenHeightInWorldsUnits + _playerHeightHalf, _screenHeightInWorldsUnits - _playerHeightHalf),currentPosition.z);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            LivesCount.Value--;

            if (LivesCount.Value <= 0)
            {
                IsDead.Value = true;
                gameObject.SetActive(false);
            }
        }
    }
}
