using System;
using System.Collections.Generic;
using Godot;

public class CompositionParsingService
{
	public CompositionParsingService() { }
	public CompositionParsingService(string fileName)
	{
		Reader = GetCharacterArray($"res://CombatScenes/DjBattle/Compositions/{fileName}");
		Offset = 0;
	}

	private char[] Reader { get; set; }
	private int Offset { get; set; }

	public List<char> GetNextToken()
	{
		var result = new List<char>();
		bool isBeatToken = false;
		bool hasArrowToken = false;
		while (Offset != Reader.Length)
		{
			char c = Reader[Offset];
			if (hasArrowToken)
			{
				if (IsBeatToken(c))
				{
					// if the result already contains arrows return arrows
					// and do not move offset
					isBeatToken = true;
					return result;
				}
				else if (IsArrowToken(c))
				{
					hasArrowToken = true;
					result.Add(c);
				}
			}
			else
			{
				if (IsBeatToken(c))
				{
					isBeatToken = true;
					result.Add(c);
				}
				else if (IsArrowToken(c))
				{
					hasArrowToken = true;
					result.Add(c);
				}
			}

			Offset += 1;

			if (isBeatToken) return result;
		}

		return result;
	}

	private bool IsBeatToken(char c)
	{
		switch (c)
		{
			case CompositionTokens.Beat:
			case CompositionTokens.TwoBeats:
			case CompositionTokens.FourBeats:
			case CompositionTokens.EOC:
				return true;
			default:
				return false;
		}
	}

	private bool IsArrowToken(char c)
	{
		switch (c)
		{
			case CompositionTokens.U:
			case CompositionTokens.R:
			case CompositionTokens.D:
			case CompositionTokens.L:
				return true;
			default:
				return false;
		}
	}

	private char[] GetCharacterArray(string filePath)
	{
		try
		{
			using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			return content.ToCharArray();
		}
		catch (Exception ex)
		{
			GD.Print($"An error occurred: {ex.Message}");
			return null;
		}
		return new char[0];
	}
}
