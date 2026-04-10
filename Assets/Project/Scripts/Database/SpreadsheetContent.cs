using System;
using System.Collections.Generic;
using NorskaLib.Spreadsheets;
using Project.Scripts.Database.Data;

namespace Project.Scripts.Database
{
    [Serializable]
    public class SpreadsheetContent
    {
        [SpreadsheetPage("Abilities")] public List<AbilityData> AbilitiesData;
        [SpreadsheetPage("Modifications")] public List<ModificationData> ModificationsData;
        [SpreadsheetPage("Characters")] public List<CharacterData> CharactersData;
    }
}