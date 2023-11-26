namespace ParrotIncSquawk.Constants.Errors;

public readonly record struct SquawkErr
{
	public SquawkErr()
	{
	}

	public readonly string DuplicatedError = "The text cannot be duplicated";

	public readonly string LessThenOrEqual400 = "'{PropertyName}' should have only 400 max characters.";

	public readonly string AtLeastOne = "'{PropertyName}' must be at least 1 character.";

	public readonly string BlackList = "'{PropertyName}' contains a word that is not allowed.";
}
