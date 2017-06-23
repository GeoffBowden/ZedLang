using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZedLang;
using ZedlangExecutor;
using ZedLangSyntacticElements;

namespace CommandLine
{
    public static class Bertrand
    {
        public static ProgramFile program = new ProgramFile();
        public static void OutputToConsole()
        {
            foreach( string line in program.OutputAsStringList() )
            {
                Console.WriteLine(line);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.Write(">");
            bool commandAllowsContinue =false;
            do
            {
                string commandText = Console.ReadLine().Trim();
                Console.Clear();
                Console.Write(">");
                try
                {
                    commandAllowsContinue = ProcessCommand(commandText);
                }
                catch ( Exception ex )
                {
                    Console.WriteLine(ex.Message);
                }
                WriteHeaderLine();
                Bertrand.OutputToConsole();
                Console.CursorLeft = 1;
                Console.CursorTop = 0;
            } while (commandAllowsContinue);
            System.Threading.Thread.Sleep(500);
        }

        private static void WriteHeaderLine()
        { 
            for (int i = 0; i < Console.WindowWidth; ++i)
            {
                Console.Write("_");
            }
            Console.WriteLine("");
        }

        private static bool ProcessCommand(string commandText)
        {
            Console.WriteLine("");
            Command command = CommandFactory.GetCommand(commandText);
            bool commandAllowsContinue = CommandProcessor.Execute(command);
            return commandAllowsContinue;
        }
    }

    public static class CommandFactory
    {
        public static Command GetCommand( string commandText )
        {
            if ((string.Compare( commandText, "Quit", StringComparison.OrdinalIgnoreCase)==0)||
                (string.Compare(commandText, "Exit", StringComparison.OrdinalIgnoreCase) == 0) ||
                (string.Compare(commandText, "bye", StringComparison.OrdinalIgnoreCase) == 0) )
            {
                return new QuitCommand(commandText);
            }
            else if (string.Compare(commandText, "help", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new HelpCommand(commandText);
            }
            else if (string.Compare(commandText, "new", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new NewProgramCommand(commandText);
            }
            else if (string.Compare(commandText, "clear", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new NewProgramCommand(commandText);
            }
            else if (string.Compare(commandText, "run", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new RunProgramCommand(commandText);
            }
            else if (string.CompareOrdinal(commandText.ToLower(), 0, "load", 0, 4)==0)
            {
                return new LoadFileCommand(commandText);
            }
            else if (string.CompareOrdinal(commandText.ToLower(), 0, "save", 0, 4) == 0)
            {
                return new SaveFileCommand(commandText);
            }
            else
            {
                SyntacticElement se = LineParser.Parse(commandText);
                SyntacticCommand sc = new SyntacticCommand( commandText, se );
                return sc;
            }
        }
    }
    

    public static class CommandProcessor
    {
        public static bool Execute(Command command)
        {
            command.Execute();
            if ( command is QuitCommand )
                return false;
            return true;
        }
    }

    public abstract class Command
    {
        protected string commandText;
        public Command( string commandText )
        {
            this.commandText = commandText;
        }
        public abstract void Execute();
    }
    // loads and runs a file
    public class LoadFileCommand : Command
    {
        private string FileName;
        public List<string> result;
        public LoadFileCommand(string commandText) : base(commandText)
        {
            result = new List<string>();
            FileName = string.Empty;
            List <string> commandWords = new List<string> ( commandText.Split(new char[] { ' ' }) );
            if ( commandWords.Count > 1 )
            {
                foreach ( string word in commandWords.Skip(1) )
                {
                    FileName += FileName == "" ? word : " " + word;
                }
            }
        }

        public override void Execute()
        {
            result.Clear();
            Bertrand.program.Clear();
            try
            {
                //   if file exists
                string[] lines = File.ReadAllLines(FileName);
                foreach (string line in lines)
                {
                    try
                    {
                        Bertrand.program.Add(line);
                    }
                    catch (Exception ex)
                    {
                        //       log invalid syntax
                        result.Add(ex.Message);
                    }
                }
                if ( result.Count == 0 )
                {
                    result = Bertrand.program.run(out ProgramReturnCode code);
                }
            }
            catch
            {
                result.Add("File not found: " + FileName);
            }
        }
    }

    // saves a file
    public class SaveFileCommand : Command
    {
        private string FileName;
        public List<string> result;
        public SaveFileCommand(string commandText) : base(commandText)
        {
            List<string> result = new List<string>();
            FileName = string.Empty;
            List<string> commandWords = new List<string>(commandText.Split(new char[] { ' ' }));
            if (commandWords.Count > 1)
            {
                foreach (string word in commandWords.Skip(1))
                {
                    FileName += FileName == "" ? word : " " + word;
                }
            }
        }
        
        public override void Execute()
        {
            File.WriteAllLines(FileName, Bertrand.program.Listing);
        }
    }

    public class QuitCommand : Command
    {
        public QuitCommand(string commandText) : base(commandText)
        {
        }

        public override void Execute() { Console.WriteLine("Bye bye"); }
    }

    public class HelpCommand : Command
    {
        public HelpCommand(string commandText) : base(commandText)
        {
        }

        public override void Execute()
        {
            Console.WriteLine(
                @"
Keywords:
    predicate       e.g. predicate p = it is raining in oklahoma
    proposition     e.g. proposition p implies q
                    e.g. proposition p ifandonlyif q
    **supposition     e.g. suppose p is true
    therefore       e.g. therefore r
    and             e.g. p and q
    or              e.g. p or q
    xor             e.g. p xor q
    **predicates      e.g. gives a predicate count
    **predicates[]    e.g. lists predicates
    **predicates[1]   e.g. lists a predicate at position 1  

    **propositions    e.g. gives a proposition count
    **propositions[]  e.g. lists propositions
    **propositions[1] e.g. lists proposition at position 1

    run, clear, new
"
                );
        }
    }

    public class NewProgramCommand : Command
    {
        public NewProgramCommand(string commandText) : base(commandText)
        {
        }

        public override void Execute()
        {
            Bertrand.program.Clear();
        }
    }

    public class RunProgramCommand : Command
    {
        public RunProgramCommand(string commandText) : base(commandText)
        {
        }

        public override void Execute()
        {
            List<string> output = Bertrand.program.run(out ProgramReturnCode code);
            foreach (var x in output) { Console.WriteLine(x); }
        }
    }

    // list predicates of show a known predicate
    public class PredicateCommand : Command
    {
        private PredicateSyntacticElement pse;
        public PredicateCommand(string commandText, PredicateSyntacticElement pse) : base(commandText)
        {
            this.pse = pse;
        }
        public override void Execute()
        {
            if (pse.Value == true)
            {
                Console.WriteLine("{0} is known to be {1}", pse.Predicate, pse.Value.ToString());
            }
            else
            {
                Console.WriteLine("{0} is unknown", pse.Predicate );
            }
        }
    }
    // list the propositions or show a known proposition
    public class PropositionsCommand : Command
    {
        public PropositionsCommand(string commandText) : base(commandText) {}
        public override void Execute()
        {
            if ( IsAPropositionListCommand )
            {

            }
            else
            {
            }
        }
        private bool IsAPropositionListCommand
        { get {
                return commandText.Contains( "[" ) && commandText.Contains( "]" );
        } }
    }

    public class SyntacticCommand : Command
    { 
        private SyntacticElement Command;
        public SyntacticCommand(string commandText, SyntacticElement command ) : base(commandText)
        {
            this.Command = command;
        }
        public override void Execute()
        {
            if (Command is PredicateSyntacticElement)
            {
                PredicateSyntacticElement pse = (PredicateSyntacticElement)(Command);
                Bertrand.program.Add(commandText);
                if (pse.State == State.Known)
                {
                    Console.WriteLine("added predicate {0} > '{1}' with a value of {2}", pse.Name, pse.Predicate, pse.Value.ToString());
                }
                else
                {
                    Console.WriteLine("added predicate {0} > '{1}' with an unknown value", pse.Name, pse.Predicate);
                }
                return;
            }
            if (Command is PropositionExpression)
            {
                PropositionExpression pse = (PropositionExpression)(Command);
                Bertrand.program.Add(commandText);
                Console.WriteLine("added proposition {0}' ", pse.Text);
                return;
            }
            if (Command is PropositionSyntacticElement )
            {
                PropositionSyntacticElement pse = (PropositionSyntacticElement)(Command);
                Bertrand.program.Add(commandText);
                Console.WriteLine("added proposition setter {0}' ", pse.Text);
                return;
            }

            if (Command is PropositionNegationSyntacticElement )
            {
                PropositionExpression pse = (PropositionExpression)(Command);
                Bertrand.program.Add(commandText);
                Console.WriteLine("added proposition setter {0}' ", pse.Text);
                return;
            }

            if (Command is ThereforeSyntacticElement)
            {
                ThereforeSyntacticElement pse = (ThereforeSyntacticElement)(Command);
                Bertrand.program.Add(commandText);
                Console.WriteLine("added therefore command {0}' ", pse.Text);
                return;
            }

            if ( Command is InvalidSyntacticElement)
            {
                InvalidSyntacticElement ise = (InvalidSyntacticElement)Command;
                Console.WriteLine("Error> {0}: {1}", ise.Title, ise.Message);
                Console.WriteLine(commandText);
                return;
            }
            throw new NotImplementedException();
        }
    }
}
