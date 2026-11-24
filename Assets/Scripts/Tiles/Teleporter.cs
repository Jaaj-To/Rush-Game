using UnityEngine;

namespace Tiles
{
    public class Teleporter : AltTile
    {
        [SerializeField] private Teleporter linkedTeleporter;

        [SerializeField] private Color pairColor = Color.cyan;

        private void Start()
        {
            CubeColors.Paint(gameObject, pairColor);
        }

        public override void ApplyAction(CubeController pCube)
        {
            if (linkedTeleporter == null)
            {
                Debug.Log("tp" + name+" n'est pas link");
                return;
            }

            pCube.Teleport(linkedTeleporter.GridPos + Vector3Int.up);
        }
    }
}
