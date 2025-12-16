using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<bool> GameOver;
    public static Action<int> UpdateScore;
    public static Action CheckIfShapeCanBePlaced;

    public static Action MoveShapeToStartPos;

    public static Action RequestNewShapes;

    public static Action CheckPlayerLost;

    public static Action SetShapeInactive;

    public static Action<int,int> UpdateBestScoreDisplay;

    public static Action<Config.SquareColor> UpdateSquareColor;

    public static Action<Config.SquareColor> ShowBonusImage;

    public static Action ShowCongratulationText;


}
