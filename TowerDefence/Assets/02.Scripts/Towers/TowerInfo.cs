using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower/Create New Tower")]
public class TowerInfo : ScriptableObject
{
    public TowerType type;
    public int level;
    public int Price;
}
public enum TowerType
{
    Turret,
    Missile,
    Laser
}
