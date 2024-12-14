using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AbilityDatabase", menuName = "Extra/AbilityDatabase")]
public class AbilityDatabase : ScriptableObject
{
    [SerializeField] private List<SpecialAbility> allAbilities;

    public List<SpecialAbility> AllAbilities => allAbilities;

    public SpecialAbility GetAbility(SpecialAbilityType abilityType, SpecialAbilityLevel level)
    {
        return allAbilities.Find(ability => 
            ability.CurrentSpecialAbilityType == abilityType && ability.CurrentSpecialAbilityLevel == level);
    }

    public List<SpecialAbility> GetAbilitiesOfType(SpecialAbilityType abilityType)
    {
        return allAbilities.FindAll(ability => ability.CurrentSpecialAbilityType == abilityType);
    }
}
