using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject explosionFx;
    private float health = 100f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    public void SetDamage(float value)
    {
        health -= value;

        if (health <= 0)
        {
            gameObject.SetActive(false);
            var exp = Instantiate(explosionFx, transform.position, Quaternion.identity);
            Destroy(exp, 1.0f);
            LevelManager.Instance.AddScore();
        }
    }
}
