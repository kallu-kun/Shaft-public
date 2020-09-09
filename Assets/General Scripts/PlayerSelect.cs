using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSelect
{
    public static bool onePlayerMode;

    public static void SetOnePlayerMode()
    {
        onePlayerMode = true;
    }

    public static void SetTwoPlayerMode()
    {
        onePlayerMode = false;
    }


}
