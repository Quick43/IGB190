using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
public class LevelUpScreen : MonoBehaviour
{
    public Player player;
    // Update is called once per frame
    public void dmgButtonPressed()
    {
        player.attackDamage += 10;
        player.gameUI.enabled = true;
        player.levelUI.enabled = false;
    }

    public void hpButtonPressed()
    {
        player.maxHealth += 10;
        player.gameUI.enabled = true;
        player.levelUI.enabled = false;
    }

    public void atkspdButtonPressed()
    {
        player.attacksPerSecond += 0.1f;
        player.gameUI.enabled = true;
        player.levelUI.enabled = false;
    }
}
