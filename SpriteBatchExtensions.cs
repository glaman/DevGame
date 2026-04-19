// SpriteBatchExtensions.cs - Helper for drawing lines in MapScreen
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class SpriteBatchExtensions
{
    /// <summary>
    /// Draws a line between two points using a 1x1 white pixel texture.
    /// </summary>
    public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, 
        Vector2 start, Vector2 end, Color color, int thickness = 2)
    {
        Vector2 direction = end - start;
        float length = direction.Length();

        if (length == 0) return;

        float angle = (float)Math.Atan2(direction.Y, direction.X);

        spriteBatch.Draw(texture,
            new Rectangle((int)start.X, (int)start.Y, (int)length, thickness),
            null,
            color,
            angle,
            new Vector2(0, thickness / 2f),
            SpriteEffects.None,
            0);
    }
}
