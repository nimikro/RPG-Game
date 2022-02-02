using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable
{
    PlayerManager playerManager;
    CharacterStats myStats;

    void Start()
    {
        playerManager = PlayerManager.instance;
        myStats = GetComponent<CharacterStats>();
    }

    public override void Interact()
    {
        base.Interact();
        // Attack enemy:
        // Get a reference to the player combat script
        CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        // Attack enemy's stats
        if(playerCombat != null)
        {
            playerCombat.Attack(myStats);
        }

    }
}
