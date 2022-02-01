using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Utility methods for collision
    /// </summary>
    public static class CollisionExtensions {
        /**
         * Take a look at https://en.wikipedia.org/wiki/Coefficient_of_restitution to learn more about collision force detection
         * -------------------------------------------------------------------
         * How to calculate the force of a collision
         * F = m • a - or - F = m • ∆v / t
         * impulse = F • t = m • ∆v
         * F (collision force) = m • ∆v / t = impulse / t
         */
        private static float GetImpactForce (this Collision collision) {
           var force = collision.impulse.magnitude / Time.fixedDeltaTime;
           GameManager.CheckForRecordOf(force);
           return force;
        }

        /// <summary>
        /// Calculate the force in a collision
        /// </summary>
        /// <param name="collision">The collision for force calculation</param>
        /// <param name="targetPosition">The position of the collision target</param>
        /// <param name="collisionPointIndex">Specific collision point to check, default is 0</param>
        /// <returns>A Vector3, result from the collision direction and force applied</returns>
        public static Vector3 GetForce (this Collision collision, Vector3 targetPosition, int collisionPointIndex = 0)
        {
            var force = Mathf.Clamp(0, 50, collision.GetImpactForce());                         // clamp for reasonable values and to prevent collision tunneling
            Vector3 dir = collision.contacts[collisionPointIndex].point - targetPosition;       // Calculate Angle Between the collision point and the player
            dir = -dir.normalized;                                                              // We then get the opposite (-Vector3) and normalize it
            return dir * force;
        }
    }
}