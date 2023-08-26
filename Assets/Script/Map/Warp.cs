using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public int next_zone_num;
    public Vector3 next_player_pos;
    public bool fix_x;
    public bool fix_y;
    public bool cameraMove_x = false;
    public bool cameraMove_y = false;

    public bool reverse_direction_x;
    public bool reverse_direction_y;
}
