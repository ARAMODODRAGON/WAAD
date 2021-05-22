using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    //Referenced by the AI to know the limits of a room
    public GameObject min;
    public GameObject max;
    //Typically the center of the room
    public GameObject cameraPosition;
}
