using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilityDatabase abilityDatabase;
    [SerializeField] private PlayerAbilityInventory playerInventory;
    [SerializeField] private UIManager uiManager;
    
    private void ShowAvailableAbilitiesForSelection(int currentLevel)
    {
        var availableAbilities = new List<SpecialAbility>();

        foreach (var ability in abilityDatabase.AllAbilities)
        {
            if (playerInventory.HasAbility(ability) && !playerInventory.IsMaxLevel(ability))
            {
                var nextLevel = playerInventory.GetCurrentLevel(ability.CurrentSpecialAbilityType) + 1;
                var nextAbility = abilityDatabase.GetAbility(ability.CurrentSpecialAbilityType, nextLevel);
                availableAbilities.Add(nextAbility);
            }
            else if (!playerInventory.HasAbility(ability) && ability.CurrentSpecialAbilityLevel == SpecialAbilityLevel.Level01)
            {
                availableAbilities.Add(ability); 
            }
        }

        var selectedAbilities = availableAbilities.OrderBy(x => Random.value).Take(2).ToList();
        uiManager.ShowAvailableAbilitiesForSelection(selectedAbilities);
    }

    public void SelectAbility(SpecialAbility selectedAbility)
    {
        playerInventory.AddOrUpgradeAbility(selectedAbility);
    }

    private void OnEnable()
    {
        PlayerStats.OnLevelChanged += ShowAvailableAbilitiesForSelection;
    }

    private void OnDisable()
    {
        PlayerStats.OnLevelChanged -= ShowAvailableAbilitiesForSelection;
    }
}
