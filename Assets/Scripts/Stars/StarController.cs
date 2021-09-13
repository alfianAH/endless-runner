using Character;
using UnityEngine;

namespace Stars
{
    public class StarController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<CharacterMoveController>()) return;

            CharacterMoveController chara = other.GetComponent<CharacterMoveController>();
            chara.AddScore(10);
            
            gameObject.SetActive(false);
        }
    }
}
