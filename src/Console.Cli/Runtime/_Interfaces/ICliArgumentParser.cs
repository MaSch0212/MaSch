namespace MaSch.Console.Cli.Runtime
{
    public interface ICliArgumentParser
    {
        void AddValidator(ICliValidator<object> validator);
        CliArgumentParserResult Parse(ICliApplicationBase application, string[] args);
    }
}
