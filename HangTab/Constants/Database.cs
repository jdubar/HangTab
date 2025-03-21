namespace HangTab.Constants;

public static class Database
{
    public static string FileName => "Bowling.db3";
	public static SQLite.SQLiteOpenFlags OpenFlags =>
		SQLite.SQLiteOpenFlags.ReadWrite |
		SQLite.SQLiteOpenFlags.Create |
		SQLite.SQLiteOpenFlags.SharedCache;
}
