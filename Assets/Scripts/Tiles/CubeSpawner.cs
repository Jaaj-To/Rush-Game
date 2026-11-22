using UnityEngine;

namespace Tiles
{
    public class CubeSpawner : MonoBehaviour
    {
        [SerializeField] private CubeController cubePrefab;

        [SerializeField] private CubeColor color = CubeColor.Red;

        [Range(1, 20)] [SerializeField] private int cubeCount = 3;

        [Range(1, 10)] [SerializeField] private int ticksBetweenSpawns = 2;

        private int tickCounter = 0;
        private int spawnedCount = 0;

        public int CubeCount => cubeCount;

        private void Start()
        {
            CubeColors.Paint(gameObject, CubeColors.ToColor(color));
        }

        public CubeController TrySpawn(Level pLevel)
        {
            CubeController lNewCube = null;
            if (spawnedCount < cubeCount && tickCounter == 0)
            {
                lNewCube = Spawn(pLevel);
            }

            tickCounter++;
            if (tickCounter >= ticksBetweenSpawns)
            {
                tickCounter = 0;
            }

            return lNewCube;
        }

        private CubeController Spawn(Level pLevel)
        {
            Vector3Int lGridPos = pLevel.WorldToGrid(transform.position);
            Vector3Int lDirection = Vector3Int.RoundToInt(transform.forward);

            CubeController lNewCube = Instantiate(cubePrefab, pLevel.GridToWorld(lGridPos), Quaternion.identity);
            spawnedCount++;
            lNewCube.Init(pLevel.GridOrigin, lGridPos, lDirection, color);
            return lNewCube;
        }
    }
}
