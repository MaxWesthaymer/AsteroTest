using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject explosionFx;

    private Action _onDead;
    private float _health = 100f;
    private float _speed;

    public void SetAsteroid(float speed, Action onDead)
    {
        _speed = speed;
        _onDead = onDead;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * (_speed * Time.deltaTime);
        transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
    }

    public void SetDamage(float value)
    {
        _health -= value;

        if (_health <= 0)
        {
            gameObject.SetActive(false);
            var exp = Instantiate(explosionFx, transform.position, Quaternion.identity);
            Destroy(exp, 1.0f);
            _onDead?.Invoke();
        }
    }
}
