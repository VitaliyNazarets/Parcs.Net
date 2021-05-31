using Parcs.Module.CommandLine;
using CommandLine;


namespace Lab
{

    public class CLOptions : BaseModuleOptions
    {
        [Option("hand", Required = true, HelpText = "Add the poker hand to parameters.")]
        public string Hand { get; set; }

        [Option("numbers", Required = true, HelpText = "Add number of the different hands.")]
        public int NumberOfHands { get; set; }

        [Option("points", Required = true)]
        public int NumberOfPoints { get; set; }
    }
}
