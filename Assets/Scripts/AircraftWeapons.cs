using UnityEngine;

/// <summary>Самолётдан олдинга ўқ чиқаради. Бу ўйин механикаси, ҳақиқий қурол симуляцияси эмас.</summary>
public class AircraftWeapons : MonoBehaviour
{
    public Transform muzzle;
    public float projectileSpeed = 240f;
    public float fireInterval = 0.18f;
    private float nextShot;

    public void Fire()
    {
        if (Time.time < nextShot || muzzle == null) return;
        nextShot = Time.time + fireInterval;
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.name = "Training projectile";
        projectile.transform.position = muzzle.position;
        projectile.transform.localScale = Vector3.one * 0.18f;
        projectile.GetComponent<Renderer>().material.color = new Color(1f, 0.65f, 0.1f);
        Rigidbody rb = projectile.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = muzzle.forward * projectileSpeed;
        projectile.AddComponent<TrainingProjectile>();
        Destroy(projectile, 4f);
    }
}

public class TrainingProjectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        TargetDummy target = collision.collider.GetComponentInParent<TargetDummy>();
        if (target != null) target.Hit();
        Destroy(gameObject);
    }
}
