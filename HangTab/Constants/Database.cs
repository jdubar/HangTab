namespace HangTab.Constants;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test constants. There's no logic to test.")]
public static class Database
{
    public static string FileName => "Bowling.db3";
	public static SQLite.SQLiteOpenFlags OpenFlags =>
		SQLite.SQLiteOpenFlags.ReadWrite |
		SQLite.SQLiteOpenFlags.Create |
		SQLite.SQLiteOpenFlags.SharedCache;
}
