using UnityEngine;
using System;

public class Grid {
    public static Vector3[] Generate(int count, float xOff, float yOff, float z = 0)
    {
        float halfX = xOff / 2;
        float halfY = yOff / 2;

        if (count < 1 || count > 9)
            throw new NotImplementedException();
        
        switch (count)
        {
            case 1:
                return new Vector3[] { new Vector3(0, 0, z) };
            case 2:
                return new Vector3[] { new Vector3(-halfX, 0, z), new Vector3(halfX, 0, z) };
            case 3:
                return new Vector3[] { new Vector3(-xOff, 0, z), new Vector3(0, 0, z), new Vector3(xOff, 0, z) };
            case 4:
                return new Vector3[] {
                    new Vector3(-halfX, halfY, z), new Vector3(halfX, halfY, z),
                    new Vector3(-halfX, -halfY, z), new Vector3(halfX, -halfY, z)
                };
            case 5:
                return new Vector3[] {
                    new Vector3(0, yOff, z),
                    new Vector3(-xOff, 0, z), new Vector3(0, 0, z), new Vector3(xOff, 0, z),
                    new Vector3(0, -yOff, z)
                };
            case 6:
                return new Vector3[] {
                    new Vector3(-halfX, yOff, z), new Vector3(halfX, yOff, z),
                    new Vector3(-halfX, 0, z), new Vector3(halfX, 0, z),
                    new Vector3(-halfX, -yOff, z), new Vector3(halfX, -yOff, z)
                };
            case 7:
                return new Vector3[] {
                    new Vector3(-halfX, yOff, z), new Vector3(halfX, yOff, z),
                    new Vector3(-xOff, 0, z), new Vector3(0, 0, z), new Vector3(xOff, 0, z),
                    new Vector3(-halfX, -yOff, z), new Vector3(halfX, -yOff, z)
                };
            case 8:
                return new Vector3[] {
                    new Vector3(-xOff, yOff, z), new Vector3(0, yOff, z), new Vector3(xOff, yOff, z),
                    new Vector3(-halfX, 0, z), new Vector3(halfX, 0, z),
                    new Vector3(-xOff, -yOff, z), new Vector3(0, -yOff, z), new Vector3(xOff, -yOff, z)
                };
            case 9:
                return new Vector3[] {
                    new Vector3(-xOff, yOff, z), new Vector3(0, yOff, z), new Vector3(xOff, yOff, z),
                    new Vector3(-xOff, 0, z), new Vector3(0, 0, z), new Vector3(xOff, 0, z),
                    new Vector3(-xOff, -yOff, z), new Vector3(0, -yOff, z), new Vector3(xOff, -yOff, z),
                };
        }

        return null;
    }
}
