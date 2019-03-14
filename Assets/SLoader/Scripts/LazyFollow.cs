using UnityEngine;

namespace SLoader
{
    public class LazyFollow : MonoBehaviour
    {
        [Tooltip("Coefficient of laziness")]
        [Range(1f, 20f)]
        public float laziness = 10f;

        [Tooltip("Distance from the main camera to the loading panel")]
        [Range(0.3f, 10f)]
        public float panelDistance = 1.6f;

        [Tooltip("Allow panel lazy rotation")]
        public bool lazyRotation = false;

        [Tooltip("Keep the same distance from the camera while rotating")]
        public bool keepDistance = true;

        public void LateUpdate()
        {
            Transform camTransform = Camera.main.transform;
            Vector3 oldPosition = transform.position;

            // get the ray of the camera's forward position
            Ray ray = new Ray(camTransform.position, camTransform.forward);

            // find the target position using ray and distance
            Vector3 targetPosition = ray.GetPoint(panelDistance);

            // Lerp between old and target positions
            Vector3 newPosition = Vector3.Lerp(oldPosition, targetPosition, laziness * Time.deltaTime);

            // recalculate position
            if(keepDistance)
            {
                // get the ray from camera position to newPosition
                ray = new Ray(camTransform.position, newPosition - camTransform.position);
                // find the target position using ray and distance
                newPosition = ray.GetPoint(panelDistance);
            }


            transform.position = newPosition;

            // Calculate rotation
            Vector3 rotationVector = lazyRotation ? oldPosition - camTransform.position : targetPosition - camTransform.position;
            transform.rotation = Quaternion.LookRotation(rotationVector);
        }
    }
}