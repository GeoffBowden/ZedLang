using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedLangProgramData;
using ZedLangSyntacticElements;

namespace ZedlangExecutor
{
    public enum ProgramReturnCode {
        No_errors
     ,  No_predicates
     ,  No_propositions
     ,  No_therefore
     ,  No_predicate_for_setter
     ,  Setter_has_invalid_value
     ,  Predicate_evaluates_to_false
     ,  Predicate_evaluates_to_unknown
    }
    public static class ZedLangProgramExecutor
    {
        //what does this do?
        // if the conclusion is known, it works out what the conclusion should be and
        // returns the actual conclusion and the given one.
        // p->q conclusion is q predicate is p after execution p is known or not depending on value of p
        // if both are set then? is the truth table checked? do interested in state before and after
        // so how does an implies statement work?
        //  p --> q   ==== !p or q
        //  0  1  0
        //  0  1  1
        //  1  0  0
        //  1  1  1
        /// <summary>
        /// Execute with a full trace of what happened
        /// </summary>
        /// <param name="program"></param>
        public static List<string> Execute(LogicArgument program, out ProgramReturnCode errorCode)
        {
            // we want a value for the whole program and
            // we want a value for each line in the program 
            // from the start to the end

            // executing must suggest a value for a predicate,
            // if this value cannot be set
            //    1 can be set to 1
            //    0 can be set to 0
            //    ? can be set to anything  1, 0 or ?

            // The execution must be such that it runs through setting predicates

            // run predicates -- if fails produce result
            // run not expressions -- if fails produce result
            // run propositions in order
            // produce result
            List<string> programOutput = new List<string>();
            // if no errors
            errorCode = Validate(program, ref programOutput);
            if (errorCode == ProgramReturnCode.No_errors)
            {
                errorCode = RunPredicateSetters(program, ref programOutput);
                if (errorCode == ProgramReturnCode.No_errors)
                {
                    // run propositions
                    errorCode = RunPropositions(program, ref programOutput);
                    if (errorCode == ProgramReturnCode.No_errors)
                    {
                        // run therefore statement
                        ExpressionExecutive.execute(program.thereforeSyntacticElement, program.predicateList);
                        program.Conclusion = program.thereforeSyntacticElement.Value;
                        if ( program.Conclusion == true )
                        {
                            programOutput.Add("Your arguments conclusion was true.");
                        }
                        else
                        {
                            programOutput.Add("Your arguments conclusion was false.");
                        }
                    }
                }
            }
            return programOutput;
        }
        private static ProgramReturnCode Validate(LogicArgument program, ref List<string> errorList)
        {
            ProgramReturnCode code = ProgramReturnCode.No_errors;
            if (program.thereforeSyntacticElement == null)
            {
                errorList.Add("An argument without a conclusion is merely a fight. Please add a therefore expression to your logic.");
                code = ProgramReturnCode.No_therefore;
            }
            if (program.predicateList.PredicateCount() == 0)
            {
                errorList.Add("No suppositions, what are you trying to prove here? Please add a predicate.");
                code = ProgramReturnCode.No_predicates;
            }
            if (program.propositionList.PropositionCount() == 0)
            {
                errorList.Add("No proof just suppositions? I'm guessing one of your predicates contains the word \"God\". Even so your work has only just begun, please add a proposition to make an argument.");
                code = ProgramReturnCode.No_propositions;
            }
            return code;
        }
        private static ProgramReturnCode RunPredicateSetters(LogicArgument program, ref List<string> programOutput)
        {
            ProgramReturnCode errorCode = ProgramReturnCode.No_errors;
            for( int i = 0; i< program.predicateSetterList.PredicateSetterCount(); i++)
            {
                PredicateSetterSyntacticElement pse = program.predicateSetterList.GetPredicateSetterAt(i);
                try
                {
                    pse = RunPredicateSetter(pse, program);
                    if ( pse.Value == false )
                    {
                        string error = pse.Text + " : contained an invalid value for predicate";
                        programOutput.Add(error);
                        errorCode = ProgramReturnCode.Setter_has_invalid_value;
                    }
                }
                catch( ArgumentNullException ex)
                {
                    string error = pse.Text + " : " + ex.Message;
                    programOutput.Add(error);
                    errorCode = ProgramReturnCode.No_predicate_for_setter;
                }
                catch (Exception ex)
                {
                    programOutput.Add(ex.Message);
                }
            }
            return errorCode;
        }

        private static PredicateSetterSyntacticElement RunPredicateSetter(PredicateSetterSyntacticElement pse, LogicArgument program)
        {
            if (pse is PropositionSyntacticElement)
            {
                program.predicateList = ExpressionExecutive.execute((PropositionSyntacticElement)pse, program.predicateList);
            }
            else if (pse is PropositionNegationSyntacticElement)
            {
                program.predicateList = ExpressionExecutive.execute((PropositionNegationSyntacticElement)pse, program.predicateList);
            }
            else
            {   // case cannot currently exist 
                throw new NotImplementedException();
            }
            return pse;
        }

        private static ProgramReturnCode RunPropositions(LogicArgument program, ref List<string> programOutput)
        {
            ProgramReturnCode errorCode = ProgramReturnCode.No_errors;
            for (int i = 0; i < program.propositionList.PropositionCount(); i++)
            {
                PropositionExpression pse = program.propositionList.GetPropositionAt(i);
                try
                {
                    pse = RunProposition(pse, program );
                    if (pse.Value == false)
                    {
                        string error = pse.Text + " : Evaluated to false";
                        programOutput.Add(error);
                        errorCode = ProgramReturnCode.Predicate_evaluates_to_false;
                    }
                    if (pse.Value == null )
                    {
                        string error = pse.Text + " : Evaluated to null";
                        programOutput.Add(error);
                        errorCode = ProgramReturnCode.Predicate_evaluates_to_unknown;
                    }
                }
                catch (Exception ex)
                {
                    programOutput.Add(ex.Message);
                }
            }
            return errorCode;
        }

        private static PropositionExpression RunProposition(PropositionExpression pse, LogicArgument program)
        {
            if( pse is ImplicationSyntacticExpression)
            {
                ExpressionExecutive.execute((ImplicationSyntacticExpression)pse, program.predicateList);
            }
            else if( pse is IfAndOnlyIfSyntacticExpression)
            {
                ExpressionExecutive.execute((IfAndOnlyIfSyntacticExpression)pse, program.predicateList);
            }
            else
            {
                throw new NotImplementedException("this bit cannot be reached and records a design error");
            }
            return pse;
        }
    }
}
