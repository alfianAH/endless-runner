using System.Collections.Generic;
using UnityEngine;

namespace Stars
{
    public class StarGenerator : MonoBehaviour
    {
        [Header("Generator Area")] 
        public Camera gameCamera;
        public float areaStartOffset = -10,
            areaEndOffset = 5;
        public float terrainTemplateWidth = 8.89f;

        [SerializeField] private StarController starPrefab;
        
        private readonly List<StarController> starsPool = new List<StarController>();
        
        private float lastGeneratedPositionX;
        private float lastRemovedPositionX;
        
        private float GetHorizontalPositionStart()
        {
            return gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f)).x + areaStartOffset;
        }
    
        private float GetHorizontalPositionEnd()
        {
            return gameCamera.ViewportToWorldPoint(new Vector3(1f, 0f)).x + areaEndOffset;
        }

        private void Start()
        {
            lastGeneratedPositionX = GetHorizontalPositionStart();
            lastRemovedPositionX = lastGeneratedPositionX - terrainTemplateWidth;
        }
        
        private void Update()
        {
            while (lastGeneratedPositionX < GetHorizontalPositionEnd())
            {
                GenerateStar(lastGeneratedPositionX);
                lastGeneratedPositionX += terrainTemplateWidth;
            }

            while (lastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
            {
                lastRemovedPositionX += terrainTemplateWidth;
                RemoveStar(lastRemovedPositionX);
            }
        }
        
        /// <summary>
        /// Generate star if randomNumber is even
        /// </summary>
        /// <param name="posX"></param>
        private void GenerateStar(float posX)
        {
            int randomNumber = Random.Range(0, 30);
            
            if(randomNumber % 2 == 0)
            {
                StarController star = GetOrCreateStar();
                star.transform.position = new Vector3(posX, 0);
                star.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Deactivate star if out of screen
        /// </summary>
        /// <param name="posX"></param>
        private void RemoveStar(float posX)
        {
            GameObject starToRemove = null;
            
            // Check all star in pool
            foreach (StarController star in starsPool)
            {
                if (star.gameObject.activeInHierarchy && star.transform.position.x <= posX)
                {
                    starToRemove = star.gameObject;
                }
            }

            if (starToRemove != null)
            {
                starToRemove.SetActive(false);
            }
        }
        
        /// <summary>
        /// Get or create star
        /// </summary>
        /// <returns></returns>
        private StarController GetOrCreateStar()
        {
            StarController star = starsPool.Find(s => !s.gameObject.activeSelf);

            if (star == null)
            {
                star = Instantiate(starPrefab.gameObject, transform).GetComponent<StarController>();
                
                starsPool.Add(star);
            }

            return star;
        }
    }
}
