using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMines.Models
{
    public enum SafetyState
    {
        Success = 0,
        MineHit = 1,
        StillInDanger = 2
    }
}
