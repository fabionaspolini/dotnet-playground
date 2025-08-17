using CommandLine;
using CommandLine.Text;

[Verb("add", HelpText = "Add file contents to the index.")]
class AddOptions
{
    [Option('t', "test", Required = true)]
    public int Teste { get; set; }

    [Option('n', "Nome", Required = true)]
    public string Nome { get; set; }
}
[Verb("commit", HelpText = "Record changes to the repository.")]
class CommitOptions
{
}
[Verb("clone", HelpText = "Clone a repository into a new directory.")]
class CloneOptions
{
}

class Program
{
    static void Main(string[] args)
    {
        // 1 - disable auto generated help
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);

        // 2 - run parser and get result
        var parserResult = parser.ParseArguments<AddOptions, CommitOptions, CloneOptions>(args);

        // 3 - display help from with NotParsed method
        parser.ParseArguments<AddOptions, CommitOptions, CloneOptions>(args)
            .WithNotParsed(errs => DisplayHelp(parserResult, errs))
            .MapResult(
                (AddOptions options) => RunAddAndReturnExitCode(options),
                (CommitOptions options) => RunCommitAndReturnExitCode(options),
                (CloneOptions options) => RunCloneAndReturnExitCode(options),
                errors => 1);

        /*Parser.Default.ParseArguments<AddOptions, CommitOptions, CloneOptions>(args)
            .MapResult(
                (AddOptions options) => RunAddAndReturnExitCode(options),
                (CommitOptions options) => RunCommitAndReturnExitCode(options),
                (CloneOptions options) => RunCloneAndReturnExitCode(options),
                errors => 1);*/
    }

    private static void DisplayHelp(ParserResult<object> parserResult, IEnumerable<Error> errs)
    {
        HelpText helpText;
        if (errs.IsVersion())  //check if error is version request
            helpText = HelpText.AutoBuild(parserResult);
        else
        {
            helpText = HelpText.AutoBuild(parserResult, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "Myapp 2.0.0-beta"; //change header
                h.Copyright = "Copyright (c) 2019 Global.com"; //change copyright text
                return HelpText.DefaultParsingErrorsHandler(parserResult, h);
            });
        }
        Console.WriteLine(helpText);
    }

    private static int RunCloneAndReturnExitCode(CloneOptions options)
    {
        throw new NotImplementedException();
    }

    private static int RunCommitAndReturnExitCode(CommitOptions options)
    {
        throw new NotImplementedException();
    }

    private static int RunAddAndReturnExitCode(AddOptions options)
    {
        throw new NotImplementedException();
    }
}