using UnityEngine;
using System.Collections;

public class ThrowSimulation : MonoBehaviour
{
    [HideInInspector] 
    public Transform target;
    [HideInInspector] 
    public Transform originalTarget;

    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    public bool destructible = true;
    private bool returningToOriginal = false;

    public void Simulate()
    {
        StartCoroutine(SimulateProjectile());
    }

    public void ResendToOriginalTarget()
    {
        if (returningToOriginal) {
            return;
        }

        returningToOriginal = true;

        StopCoroutine("SimulateProjectile");

        target = originalTarget;

        StartCoroutine(SimulateProjectile());
    }
    
    IEnumerator SimulateProjectile()
    {
        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
        float elapse_time = 0;

        int direction = transform.position.x > target.position.x ? -1 : 1;

        while (elapse_time < flightDuration)
        {
            transform.Translate(
                Vx * direction * Time.deltaTime,
                (Vy - (gravity * elapse_time)) * Time.deltaTime * Time.timeScale,
                0
            );

            elapse_time += Time.deltaTime;

            yield return new WaitForSeconds(1); // slow time
        }
    }
}