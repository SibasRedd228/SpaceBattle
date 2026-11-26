using UnityEngine;

// Границы видимой области ортографической камеры в мировых координатах.
public static class PlayArea
{
    public static bool TryGet(out float minX, out float maxX, out float minY, out float maxY)
    {
        minX = maxX = minY = maxY = 0f;
        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic)
            return false;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector3 center = cam.transform.position;
        minX = center.x - halfWidth;
        maxX = center.x + halfWidth;
        minY = center.y - halfHeight;
        maxY = center.y + halfHeight;
        return true;
    }
}
