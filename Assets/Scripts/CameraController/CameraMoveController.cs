using UnityEngine;

namespace CameraController
{
    public class CameraMoveController : SingletonBaseClass<CameraMoveController>
    {
        [Header("Position")] 
        public Transform player;
        public float horizontalOffset;

        private void Update()
        {
            Vector3 newPosition = transform.position;
            newPosition.x = player.position.x + horizontalOffset;
            transform.position = newPosition;
        }
    }
}
