using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptables/Dialogue", order = 1)]
public class DialogueScriptable : ScriptableObject
{
    [SerializeField] private Character.CharacterName characterName;
}
