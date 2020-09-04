using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SettingsData", menuName = "Settings", order = 1)]
public class CreateScriptableObjectMenu : ScriptableObject
{
    public string objectName = "Settings";
    public bool GameLoaded = false;
    public int GameLoadedTimes = 0;

}

[CreateAssetMenu(fileName = "EnemyL1", menuName = "NPCStats", order = 1)]
public class NPCStatsR : ScriptableObject
{
    public string objectName = "Enemyl1";
    public int Money = 0;
    public int Health = 0;
    public int Level = 0;
    public Vector3[] spawnPoints;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "Transformation", menuName = "Abilities", order = 1)]
public class Transformation : ScriptableObject
{
    public SpriteRenderer spriteRenderer;
    public Sprite objectPicked;
    public float timeToChange = 0.1f;
    private float timeSinceChange = 0f;
    public float Form = 1f;






   public byte rrr = 0;
    public byte ggg = 0;
    public byte bbb = 0;
    public byte aaa = 0;
}