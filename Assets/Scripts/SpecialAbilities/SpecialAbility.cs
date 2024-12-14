using UnityEngine;

[CreateAssetMenu(fileName = "SpecialAbility", menuName = "Extra/SpecialAbility")]
public class SpecialAbility : ScriptableObject
{
    [SerializeField] private SpecialAbilityType specialAbilityType;
    [SerializeField] private SpecialAbilityLevel specialAbilityLevel;
    
    [SerializeField] private string abilityName;
    
    [TextArea (5, 15)]
    [SerializeField] private string abilityDescription;
    [SerializeField] private Sprite abilityIcon;
    
    
    public string AbilityName { get => abilityName; set => abilityName = value; }
    public string AbilityDescription { get => abilityDescription; set => abilityDescription = value; }
    public Sprite AbilityIcon { get => abilityIcon; set => abilityIcon = value; }
    public SpecialAbilityType CurrentSpecialAbilityType { get => specialAbilityType; set => specialAbilityType = value; }
    public SpecialAbilityLevel CurrentSpecialAbilityLevel { get => specialAbilityLevel; set => specialAbilityLevel = value; }
}

public enum SpecialAbilityType
{
    BlastShot,
    MagneticField,
    PierceShot,
    FreezeShot,
    Satellite
}

public enum SpecialAbilityLevel
{
    Level01,
    Level02,
    Level03,
}
