using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell 
{
    public bool isLand;
    public bool isWater;
    public bool isSand;
    public bool isRock;
    public bool isSnow;
    public Cell(bool _isLand,bool _isWater,bool _isSand,bool _isRock, bool _isSnow) {
        isLand = _isLand;
        isWater = _isWater;
        isSand = _isSand;
        isRock = _isRock;
        isSnow = _isSnow;
    }

}
