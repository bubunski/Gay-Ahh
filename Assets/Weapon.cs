using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothness = 4.0f;

    private Vector3 initialPosition;


    public GameObject explosionPrefab;
    public float explosionRadius = 5f;
    public float explosionForce = 100f;
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float moveX = -Input.GetAxis("Mouse X") * swayAmount;
        float moveY = -Input.GetAxis("Mouse Y") * swayAmount;

        moveX = Mathf.Clamp(moveX, -maxSwayAmount, maxSwayAmount);
        moveY = Mathf.Clamp(moveY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * swaySmoothness);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                // Spawn explosion at hit point
                SpawnExplosion(hit.point);

                // Apply explosion force to nearby objects
                ApplyExplosionForce(hit.point);
            }
        }


        void ApplyExplosionForce(Vector3 explosionPosition)
        {
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                }
            }
        }
    }

    private void SpawnExplosion(Vector3 position)
    {
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }
}

