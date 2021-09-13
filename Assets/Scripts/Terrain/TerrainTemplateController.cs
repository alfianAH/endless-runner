using UnityEngine;

namespace Terrain
{
    public class TerrainTemplateController : MonoBehaviour
    {
        private const float DEBUG_LINE_HEIGHT = 10.0f;

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Debug.DrawLine(position + Vector3.up*DEBUG_LINE_HEIGHT/2,
                position + Vector3.down*DEBUG_LINE_HEIGHT/2, 
                Color.green);
        }
    }
}
