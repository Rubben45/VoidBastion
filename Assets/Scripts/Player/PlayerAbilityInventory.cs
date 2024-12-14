using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAbilityInventory : MonoBehaviour
{
    private Dictionary<SpecialAbilityType, SpecialAbilityLevel> playerAbilities = new();
    
    public static event Action OnSatelliteUnlocked;
    public static event Action<SpecialAbilityType> OnSpecialAbilityChanged;

    public bool HasAbility(SpecialAbility ability)
    {
        return playerAbilities.ContainsKey(ability.CurrentSpecialAbilityType);
    }

    public bool IsMaxLevel(SpecialAbility ability)
    {
        return playerAbilities.ContainsKey(ability.CurrentSpecialAbilityType) && 
               playerAbilities[ability.CurrentSpecialAbilityType] == SpecialAbilityLevel.Level03;
    }

    public SpecialAbilityLevel GetCurrentLevel(SpecialAbilityType abilityType)
    {
        return playerAbilities.GetValueOrDefault(abilityType, SpecialAbilityLevel.Level01);
    }

    public void AddOrUpgradeAbility(SpecialAbility ability)
    {
        if (HasAbility(ability))
        {
            playerAbilities[ability.CurrentSpecialAbilityType]++;
        }
        else
        {
            playerAbilities[ability.CurrentSpecialAbilityType] = ability.CurrentSpecialAbilityLevel;
            
            if (ability.CurrentSpecialAbilityType == SpecialAbilityType.Satellite)
            {
                OnSatelliteUnlocked?.Invoke(); 
            }
        }
        
        OnSpecialAbilityChanged?.Invoke(ability.CurrentSpecialAbilityType);
    }

    public bool HasBlastShot()
    {
        return playerAbilities.ContainsKey(SpecialAbilityType.BlastShot);
    }

    public bool HasMagneticField()
    {
        return playerAbilities.ContainsKey(SpecialAbilityType.MagneticField);
    }

    public bool HasPierceShot()
    {
        return playerAbilities.ContainsKey(SpecialAbilityType.PierceShot);
    }

    public bool HasFreezeShot()
    {
        return playerAbilities.ContainsKey(SpecialAbilityType.FreezeShot);
    }

    public bool HasSatellite()
    {
        return playerAbilities.ContainsKey(SpecialAbilityType.Satellite);
    }
}
