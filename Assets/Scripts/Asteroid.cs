using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject explosionFx;
    #endregion

    #region PrivateFields
    private Action _onDead;
    private float _health = 100f;
    private float _speed;
    #endregion

    #region PublicMethods
    public void SetAsteroid(float speed, Action onDead)
    {
        _speed = speed;
        _onDead = onDead;
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
    #endregion
    
    #region UnityMethods
    void Update()
    {
        Move();
    }
    #endregion

    #region PrivateMethods
    private void Move()
    {
        transform.position += Vector3.down * (_speed * Time.deltaTime);
        transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
    }
    #endregion
}
