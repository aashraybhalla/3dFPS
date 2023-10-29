using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Wave Data", order = 1)]
public class WaveData : ScriptableObject
{
    public EnemyData[] zombiesInWave;
    public int numberOfZombies = 10;
}
