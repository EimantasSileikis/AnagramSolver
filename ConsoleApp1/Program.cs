using GenericTask;

var result = GenericTasks.MapValueToEnum<Weekday, string>("1");
Console.WriteLine(result);