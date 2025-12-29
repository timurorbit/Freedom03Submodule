using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Game.Scripts.Behaviours
{
    /// <summary>
    /// Plays MMF Feedback when a collision occurs at the collision position.
    /// 
    /// Usage:
    /// 1. Attach this component to a GameObject with a Collider
    /// 2. Assign an MMF_Player prefab to the collisionFeedback field
    /// 3. (Optional) Set filterTag to only respond to specific tagged objects
    /// 4. (Optional) Adjust cleanupDelay based on your feedback duration
    /// 
    /// The feedback will be instantiated at the exact collision contact point.
    /// </summary>
    public class CollisionFeedback : MonoBehaviour
    {
        [Header("Feedback Settings")]
        [SerializeField] private MMF_Player collisionFeedback;

        [SerializeField] private MMF_Player soundFeedback;
        
        
        [Header("Collision settings")]

        [SerializeField] private string filterTag = "";


        [SerializeField] 
        [Tooltip("Time in seconds before cleaning up the instantiated feedback")]
        private float cleanupDelay = 3f;

        [SerializeField]
        private Vector3 offset = Vector3.zero;

        [SerializeField] private float delay = 0f;


        [Header("Collision Filtering (Fixes Low/High Speed Issues)")]
        [SerializeField] 
        [Tooltip("Minimum relative velocity magnitude to trigger feedback (ignores sliding/low-speed noise)")]
        [Range(0.01f, 5f)] 
        private float minRelativeVelocity = 0.1f;

        [SerializeField] 
        [Tooltip("Minimum impulse magnitude (sqr) for the best contact (ignores weak/jittery hits)")]
        [Range(0.0001f, 10f)] 
        private float minImpulseThreshold = 0.01f;

        /// <summary>
        /// Called when a collision occurs
        /// </summary>
        /// <param name="collision">Collision data</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (!string.IsNullOrEmpty(filterTag) && !collision.gameObject.CompareTag(filterTag))
            {
                return;
            }
            
            if (collisionFeedback != null && collision.contactCount > 0)
            {
                PlaySoundFeedback(collision);
                soundFeedback?.PlayFeedbacks();
                StartCoroutine(Play(collision));
            }
        }

        private void PlaySoundFeedback(Collision collision)
        {
            float rv = collision.relativeVelocity.magnitude;
            float minThreshold = minRelativeVelocity * 0.1f;
            float volume;
            if (rv >= minRelativeVelocity)
            {
                volume = 1f;
            }
            else if (rv < minThreshold)
            {
                volume = 0f;
            }
            else
            {
                // Linear interpolation between 0.15 and 1.0 for velocities between 10% and 100% of minRelativeVelocity
                float t = (rv - minThreshold) / (minRelativeVelocity - minThreshold);
                volume = 0.15f + t * (1f - 0.15f);
            }

            StartCoroutine(PlaySound(volume));
        }

        private IEnumerator PlaySound(float volume)
        {
            yield return new WaitForSeconds(delay);
            MMF_Player feedbackInstance = Instantiate(soundFeedback, 
                transform);
        
            // Destroy the temporary holder after the specified cleanup delay
            Destroy(feedbackInstance, cleanupDelay);
            MMF_MMSoundManagerSound sound = soundFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>();
            // if (sound != null)
            // {
            //     sound.MinVolume = volume;
            //     sound.MaxVolume = volume;
            // }
            feedbackInstance.PlayFeedbacks();
        }

        private IEnumerator Play(Collision collision)
        {
            // Filter out low-speed noise
            if (collision.relativeVelocity.magnitude < minRelativeVelocity)
            {
                yield break;
            }
            // Find BEST contact: highest impulse (ignores jitter/low-speed multiples)
            ContactPoint bestContact = default;
            float maxImpulseMag = 0f;
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint cp = collision.GetContact(i);
                float impulseMag = cp.impulse.sqrMagnitude;  // Use sqr for perf
                if (impulseMag > maxImpulseMag)
                {
                    maxImpulseMag = impulseMag;
                    bestContact = cp;
                }
            }

            // Filter weak hits
            if (maxImpulseMag < minImpulseThreshold)
            {
                yield break;
            }

            yield return new WaitForSeconds(delay);

            // Use precise best contact point
            Vector3 collisionPosition = bestContact.point;
    
            // Optional: Nudge outward slightly to avoid clipping into surface (uncomment & tweak distance)
             collisionPosition += bestContact.normal * 0.01f;

            // Create a temporary GameObject at the collision position to hold the feedback
            GameObject tempFeedbackHolder = new GameObject($"CollisionFeedback_{GetInstanceID()}");
            tempFeedbackHolder.transform.position = collisionPosition;
    
            // Optional: Align feedback rotation to surface normal (uncomment & adjust)
             tempFeedbackHolder.transform.rotation = Quaternion.LookRotation(-bestContact.normal, Vector3.up);

            // Instantiate the feedback (local to holder for easy cleanup)
            MMF_Player feedbackInstance = Instantiate(collisionFeedback, 
                tempFeedbackHolder.transform.position + offset,
                tempFeedbackHolder.transform.rotation,
                tempFeedbackHolder.transform);
            feedbackInstance.PlayFeedbacks();
        
            // Destroy the temporary holder after the specified cleanup delay
            Destroy(tempFeedbackHolder, cleanupDelay);
        }
    }
}