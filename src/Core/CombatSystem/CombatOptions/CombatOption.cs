using System;
using Godot;
using Newtonsoft.Json;

public class CombatOption
{
	public static readonly StringName _ASK = new StringName("res://Core/CombatSystem/CombatOptions/Chat/Ask.json");
	public static readonly StringName _CHARM = new StringName("res://Core/CombatSystem/CombatOptions/Chat/Charm.json");
	public static readonly StringName _BITE = new StringName("res://Core/CombatSystem/CombatOptions/Fight/Bite.json");
	public static readonly StringName _SCRATCH = new StringName("res://Core/CombatSystem/CombatOptions/Fight/Scratch.json");

	public CombatOption() { }
	public CombatOption(int id)
	{
		try
		{
			string filePath = null;
			switch (id)
			{
				case (int)Enumerations.Combat.CombatOptions.Ask:
					filePath = _ASK;
					break;
				case (int)Enumerations.Combat.CombatOptions.Charm:
					filePath = _CHARM;
					break;
				case (int)Enumerations.Combat.CombatOptions.Bite:
					filePath = _BITE;
					break;
				case (int)Enumerations.Combat.CombatOptions.Scratch:
					filePath = _SCRATCH;
					break;
				case -1:
					filePath = string.Empty;
					break;
				default:
					////GD.Print("CombatOptions could not map to Enumerations.Combat.CombatOptions");
					filePath = string.Empty;
					break;
			}

			if (string.IsNullOrEmpty(filePath))
			{
				Id = -1;
				Type = -1;
				Name = string.Empty;
				Effect = string.Empty;
				Description = string.Empty;
				return;
			}

			using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			////GD.Print($"content {content}");
			var obj = JsonConvert.DeserializeObject<CombatOption>(content);
			Id = obj.Id;
			Type = obj.Type;
			Name = obj.Name;
			Effect = obj.Effect;
			Description = obj.Description;
		}
		catch (Exception exception)
		{
			////GD.Print($"Error: CombatOption not mapped on id {id}");
			////GD.PrintErr(exception.Message);
		}
	}

	[JsonProperty("Id")]
	public int Id { get; set; }

	[JsonProperty("Type")]
	public int Type { get; set; }

	[JsonProperty("Name")]
	public string Name { get; set; }

	[JsonProperty("Effect")]
	public string Effect { get; set; }

	[JsonProperty("Description")]
	public string Description { get; set; }
}
