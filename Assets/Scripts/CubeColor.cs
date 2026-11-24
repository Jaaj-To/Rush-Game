using UnityEngine;

public enum CubeColor { Red, Blue, Green, Yellow, Orange, Purple }

public static class CubeColors
{
    private static readonly int BASE_COLOR_ID = Shader.PropertyToID("_BaseColor");
    private static readonly Color ORANGE = new Color(1f, 0.5f, 0f);
    private static readonly Color PURPLE = new Color(0.6f, 0.2f, 0.9f);

    public static Color ToColor(CubeColor pColor)
    {
        switch (pColor)
        {
            case CubeColor.Blue:
                return Color.blue;
            case CubeColor.Green:
                return Color.green;
            case CubeColor.Yellow:
                return Color.yellow;
            case CubeColor.Orange:
                return ORANGE;
            case CubeColor.Purple:
                return PURPLE;
            default:
                return Color.red;
        }
    }

    public static void Paint(GameObject pTarget, Color pColor)
    {
        MaterialPropertyBlock lBlock = new MaterialPropertyBlock();

        foreach (Renderer lRenderer in pTarget.GetComponentsInChildren<Renderer>()){
            lRenderer.GetPropertyBlock(lBlock);
            lBlock.SetColor(BASE_COLOR_ID, pColor);
            lRenderer.SetPropertyBlock(lBlock);
        }
    }
}
