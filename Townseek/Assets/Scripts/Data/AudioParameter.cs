using UnityEngine;

namespace WhalesAndGames.MapGame.Data
{
    /// <summary>
    /// Defines an audio parameter that can be assigned to multiple objects. The string here corresponds to the FMod Event.
    /// </summary>
    [CreateAssetMenu(fileName = "New Audio Parameter", menuName = "Map Game/Audio Parameter", order = 100)]
    public class AudioParameter : ScriptableObject
    {
        public string parameterName;
    }
}