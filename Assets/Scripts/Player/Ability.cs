using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public float abilityLifetime = 5f;
    public int healthModifier = 1;
    public float speedModifier = 1f;
    public float sizeModifier = 1f;
}
