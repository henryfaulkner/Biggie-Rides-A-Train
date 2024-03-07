using System;
using Godot;
using Newtonsoft.Json;

public class CombatOption
{
    public static readonly string _ASK = "res://Core/CombatSystem/CombatOptions/Chat/Ask";
    public static readonly string _CHARM = "res://Core/CombatSystem/CombatOptions/Chat/Charm";
    public static readonly string _BITE = "res://Core/CombatSystem/CombatOptions/Fight/Bite";
    public static readonly string _SCRATCH = "res://Core/CombatSystem/CombatOptions/Fight/Scratch";

    public CombatOption(Enumerations.CombatOptions id)
    {
        switch (id)
        {
            case Enumerations.CombatOptions.Ask:
                var objAsk = JsonConvert.DeserializeObject<CombatOption>(_ASK);
                Id = objAsk.Id;
                Type = objAsk.Type;
                Name = objAsk.Name;
                Subname = objAsk.Subname;
                Description = objAsk.Description;
                break;
            case Enumerations.CombatOptions.Charm:
                var objCharm = JsonConvert.DeserializeObject<CombatOption>(_CHARM);
                Id = objCharm.Id;
                Type = objCharm.Type;
                Name = objCharm.Name;
                Subname = objCharm.Subname;
                Description = objCharm.Description;
                break;
            case Enumerations.CombatOptions.Bite:
                var objBite = JsonConvert.DeserializeObject<CombatOption>(_BITE);
                Id = objBite.Id;
                Type = objBite.Type;
                Name = objBite.Name;
                Subname = objBite.Subname;
                Description = objBite.Description;
                break;
            case Enumerations.CombatOptions.Scratch:
                var objScratch = JsonConvert.DeserializeObject<CombatOption>(_SCRATCH);
                Id = objScratch.Id;
                Type = objScratch.Type;
                Name = objScratch.Name;
                Subname = objScratch.Subname;
                Description = objScratch.Description;
                break;
            default:
                GD.Print("CombatOptions could not map to Enumerations.CombatOptions");
                break;
        }
    }

    public int Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; }
    public string Subname { get; set; }
    public string Description { get; set; }
}