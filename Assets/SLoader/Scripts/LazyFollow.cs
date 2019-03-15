using UnityEngine;

namespace SLoader
{
    public class LazyFollow : MonoBehaviour
    {
        [Tooltip("Coefficient of laziness")]
        [Range(0.1f, 10f)]
        public float speed = 3f;

        [Tooltip("Distance from the main camera to the loading panel")]
        [Range(0.3f, 10f)]
        public float panelDistance = 1.6f;

        [Tooltip("Allow panel lazy rotation")]
        public bool lazyRotation = false;

        [Tooltip("Keep the same distance from the camera while rotating")]
        public bool keepDistance = true;

        Camera _camera;

        public void Start()
        {
            Reset();
        }

        public void Reset()
        {
            _camera = Camera.main;
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            transform.position = ray.GetPoint(panelDistance);
        }

        public void LateUpdate()
        {
            Vector3 camPosition = _camera.transform.position;
            Vector3 oldPosition = transform.position;

            // get the ray of the camera's forward position
            Ray ray = new Ray(camPosition, _camera.transform.forward);

            // find the target position using ray and distance
            Vector3 targetPosition = ray.GetPoint(panelDistance);

            // Lerp between old and target positions
            Vector3 newPosition = Vector3.Lerp(oldPosition, targetPosition, speed * Time.deltaTime);

            // recalculate position
            if(keepDistance)
            {
                // get the ray from camera position to newPosition
                ray = new Ray(camPosition, newPosition - camPosition);
                // find the target position using ray and distance
                newPosition = ray.GetPoint(panelDistance);
            }


            transform.position = newPosition;

            // Calculate rotation
            Vector3 rotationVector = lazyRotation ? oldPosition - camPosition : targetPosition - camPosition;
            transform.rotation = Quaternion.LookRotation(rotationVector);
        }
    }
}