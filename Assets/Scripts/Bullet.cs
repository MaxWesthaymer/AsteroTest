using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private float speed;
    #endregion
    
    #region PrivateFields
    private float _damage;
    #endregion
    
    #region UnityMethods
    private void Start()
    {
        _damage = GameData.Instance.gameConfig.ShipDamage;
        Destroy(gameObject, 1.0f);
    }
    
    private void Update()
    {
        transform.position += Vector3.up * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Asteroid>()?.SetDamage(_damage);
            Destroy(gameObject);
        }
    }
    #endregion
}
