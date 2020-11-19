using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private float speed;
    [SerializeField] private GameObject bulletPrefab;
    private float screenWidthInWorldsUnits;
    private float screenHeightInWorldsUnits;
    private float playerHeightHalf;

    private float timeShoot = 0.1f;
    private float cooldown;
    public IntReactiveProperty LivesCount {get; private set;}
    public BoolReactiveProperty IsDead {get; private set;}
    void Start()
    {
        LivesCount = new IntReactiveProperty(3);
        IsDead = new BoolReactiveProperty(false);
        var mainCamera = Camera.main;
        screenWidthInWorldsUnits = mainCamera.aspect * mainCamera.orthographicSize;
        screenHeightInWorldsUnits = mainCamera.orthographicSize;
        playerHeightHalf = GetComponent<CapsuleCollider>().height / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead.Value)
        {
            return;
        }
        var input = new Vector3(joystick.Horizontal, joystick.Vertical, 0);
        transform.position += input.normalized * speed * Time.deltaTime;
        var currentPosition = transform.position;
        transform.position = new Vector3(Mathf.Clamp(currentPosition.x, -screenWidthInWorldsUnits , screenWidthInWorldsUnits), 
            Mathf.Clamp(currentPosition.y, -screenHeightInWorldsUnits + playerHeightHalf, screenHeightInWorldsUnits - playerHeightHalf),currentPosition.z);

        if (Input.GetMouseButton(0))
        {
            if (cooldown <= 0)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                cooldown = timeShoot;
            }

            cooldown -= Time.deltaTime;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            cooldown = 0;
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
