using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Utils
{
    public static Vector3 FloatToVector(float[] floats) {
        return new Vector3(floats[0], floats[1], floats[2]);
    }
}
