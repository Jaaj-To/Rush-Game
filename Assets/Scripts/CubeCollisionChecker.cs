using System.Collections.Generic;
using UnityEngine;

public class CubeCollisionChecker
{
    public List<CubeController> FindCollidingCubes(List<CubeController> pCubes)
    {
        List<CubeController> lCollidingCubes = new List<CubeController>();
        for (int i = 0; i < pCubes.Count; i++)
        {
            CubeController lCubeA = pCubes[i];
            for (int j = i + 1; j < pCubes.Count; j++)
            {
                CubeController lCubeB = pCubes[j];
                if (!AreColliding(lCubeA, lCubeB))
                {
                    continue;
                }
                AddOnce(lCollidingCubes, lCubeA);
                AddOnce(lCollidingCubes, lCubeB);
            }
        }
        return lCollidingCubes;
    }

    private bool AreColliding(CubeController pCubeA, CubeController pCubeB)
    {
        if (pCubeA.IsArrived || pCubeB.IsArrived)
        {
            return false;
        }

        bool lIsSameCell = pCubeA.NextPosition == pCubeB.NextPosition;
        bool lIsCrossing = pCubeA.NextPosition == pCubeB.GridPosition && pCubeB.NextPosition == pCubeA.GridPosition;
        return lIsSameCell || lIsCrossing;
    }

    private void AddOnce(List<CubeController> pCubes, CubeController pCube)
    {
        if (!pCubes.Contains(pCube))
        {
            pCubes.Add(pCube);
        }
    }
}
