using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [Tooltip("Prefab to shoot")]
    [SerializeField] private Projectile projectilePrefab;

    [Tooltip("Projectile force")]
    [SerializeField] private float muzzleVelocity = 700f;

    [Tooltip("End point of gun where shotrs appear")]
    [SerializeField] private Transform muzzlePosition;

    [Tooltip("Time between shots / smaller = higher rate of fire")]
    [SerializeField] private float cooldownWindow = 0.1f;

    private IObjectPool<Projectile> objectPool;

    // throw an exception if we try to return an existing item, already in the pool
    [SerializeField] private bool collectionCheck = true;

    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private float nextTimeToShoot;

    private void Awake()
    {
        objectPool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);

    }
    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && Time.time > nextTimeToShoot && objectPool != null)
        {
            Projectile bulletObject = objectPool.Get();

            if (bulletObject == null)
                return;

            bulletObject.transform.SetPositionAndRotation(muzzlePosition.position, muzzlePosition.rotation);

            bulletObject.GetComponent<Rigidbody>().AddForce(bulletObject.transform.forward * muzzleVelocity, ForceMode.Acceleration);

            bulletObject.Deactivate();

            nextTimeToShoot = Time.time + cooldownWindow;
        }
    }

    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.ObjectPool = objectPool;
        return projectileInstance;
    }

    private void OnGetFromPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Projectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
