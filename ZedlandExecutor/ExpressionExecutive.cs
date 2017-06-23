using System;
using ZedLangProgramData;
using ZedLangSyntacticElements;

namespace ZedlangExecutor
{
    public static class ExpressionExecutive
    {
        public static PredicateList execute(PropositionSyntacticElement element, PredicateList predicateState)
        {
            // when a proposition element is executed the proposition 
            // must be set to true. This can only be done when:
            //  the proposition is found in the list
            //  the proposition must not be false
            // the element can then be set to true
            PredicateSyntacticElement pse = predicateState.GetPredicate(element.PredicateName);
            if (pse == null)
            {
                element.Value = false;
                throw new ArgumentNullException(element.PredicateName +
                    "could not bo found in state for " + element.Text);
            }
            else
            {
                if (pse.Value == false)
                {
                    element.Value = false;
                }
                else
                {
                    pse.Value = true;
                    element.Value = true;
                }
            }
            return predicateState;
        }
        public static PredicateList execute(PropositionNegationSyntacticElement element, PredicateList predicateState)
        {
            // when a proposition element is executed the proposition 
            // must be set to true. This can only be done when:
            //  the proposition is found in the list
            //  the proposition must not be true
            // the element can then be set to false
            PredicateSyntacticElement pse = predicateState.GetPredicate(element.PredicateName);
            if (pse == null)
            {
                element.Value = false;
                throw new ArgumentNullException("");
            }
            else
            {
                if (pse.Value == true)
                {
                    element.Value = false;
                }
                else
                {
                    pse.Value = false;
                    element.Value = true;
                }
            }
            return predicateState;
        }
        /// <summary>
        /// Implies when run, evaluates left hand side and right hand side then sets itself 
        /// this is the a result, True, the argument is valid and false it is not valid.
        /// Unknown will have to be taken as invalid. If all steps in the argument are valid
        /// then we should have a good argument
        /// implies testing
        /// p --> q
        /// 1  1  1
        /// 1  0  0
        /// 0  1  1
        /// 0  1  0
        /// X  1  1
        /// X  0  X
        /// 1  X  X
        /// 0  X  1
        /// X  X  X
        /// </summary>
        /// <param name="element"></param>
        /// <param name="predicateState"></param>
        /// <returns></returns>
        public static PredicateList execute(ImplicationSyntacticExpression element, PredicateList predicateState)
        {
            bool? lhs = ExpressionExecutive.Evaluate(element.lhs, predicateState);
            bool? rhs = ExpressionExecutive.Evaluate(element.rhs, predicateState);
            element.Value = ImpliesTruthTable(lhs, rhs);
            return predicateState;
        }
        private static bool? ImpliesTruthTable(bool? p, bool? q)
        {
            if ((p == true) & (q == true)) return true;
            if ((p == true) & (q == false)) return false;
            if ((p == false) & (q == true)) return true;
            if ((p == false) & (q == false)) return true;
            if ((p == false) & (q == null)) return true;
            if ((p == true) & (q == null)) return null;
            if ((p == null) & (q == true)) return true;
            if ((p == null) & (q == false)) return null;
            if ((p == null) & (q == null)) return null;
            return null;
        }
        /// <summary>
        /// ifandonlyif when run, evaluates left hand side and right hand side then sets itself 
        /// this is the a result, True, the argument is valid and false it is not
        /// Unknown will have to be taken as invalid. If all steps in the argument are valid
        /// then we should have a good argument
        /// implies testing
        /// p <-> q
        /// 1  1  1
        /// 1  0  0
        /// 0  1  0
        /// 0  0  1
        /// 1  X  X
        /// 0  X  X
        /// X  1  X
        /// X  0  X
        /// X  X  X
        /// </summary>
        /// <param name="element"></param>
        /// <param name="predicateState"></param>
        /// <returns></returns>
        public static PredicateList execute(IfAndOnlyIfSyntacticExpression element, PredicateList predicateState)
        {
            bool? lhs = ExpressionExecutive.Evaluate(element.lhs, predicateState);
            bool? rhs = ExpressionExecutive.Evaluate(element.rhs, predicateState);
            element.Value = IfandOnlyIfTruthTable(lhs, rhs);
            return predicateState;
        }
        private static bool? IfandOnlyIfTruthTable(bool? p, bool? q)
        {
            if ((p == true) & (q == true)) return true;
            if ((p == true) & (q == false)) return false;
            if ((p == false) & (q == true)) return false;
            if ((p == false) & (q == false)) return true;
            if ((p == false) & (q == null)) return null;
            if ((p == true) & (q == null)) return null;
            if ((p == null) & (q == true)) return null;
            if ((p == null) & (q == false)) return null;
            if ((p == null) & (q == null)) return null;
            return null;
        }

        /// <summary>
        /// each of the following expressions all rely on expressions either unary or binary
        /// these are defined in the evaluate function, these can only really be worked out when all predicate suppositions
        /// are made. For propositions like p and not p must be run
        /// </summary>
        /// <param name="element"></param>
        /// <param name="predicateState"></param>
        /// <returns></returns>
        public static PredicateList execute(ThereforeSyntacticElement element, PredicateList predicateState)
        {
            // for of a therefore are 
            element.Value = ExpressionExecutive.Evaluate(element.expression, predicateState);
            return predicateState;
        }

        /// <summary>
        /*
                Expression Syntactic Element(abstract)
                Predicate Expression
                    Not Expression
                    Therefore syntactic Element
                    And Expression
                    Or Expression
                    Xor Expression
                        Implication Expression
                        IfAndOnlyIf Expression
        */
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public class UnknownExpressionException : Exception
        {
            public UnknownExpressionException()
            {
            }

            public UnknownExpressionException(string message)
                : base(message)
            {
            }

            public UnknownExpressionException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
        public static bool? Evaluate(ExpressionSyntacticElement exp, PredicateList state)
        {
            if ( exp.GetType()==typeof(NotExpression) )
            {
                return ExpressionExecutive.Evaluate((NotExpression)exp, state);
            }
            if (exp.GetType() == typeof(AndExpression))
            {
                return ExpressionExecutive.Evaluate((AndExpression)exp, state);
            }
            if (exp.GetType() == typeof(OrExpression))
            {
                return ExpressionExecutive.Evaluate((OrExpression)exp, state);
            }
            if (exp.GetType() == typeof(XorExpression))
            {
                return ExpressionExecutive.Evaluate((XorExpression)exp, state);
            }
            if (exp.GetType() == typeof(PredicateExpression))
            {
                return ExpressionExecutive.Evaluate((PredicateExpression)exp, state);
            }
            if (exp.GetType() == typeof(ImplicationSyntacticExpression))
            {
                return ExpressionExecutive.Evaluate((ImplicationSyntacticExpression)exp, state);
            }
            if (exp.GetType() == typeof(IfAndOnlyIfSyntacticExpression))
            {
                return ExpressionExecutive.Evaluate((IfAndOnlyIfSyntacticExpression)exp, state);
            }
            throw new UnknownExpressionException(); 
        }


        public static bool? Evaluate(NotExpression exp, PredicateList state)
        {
            bool? val = ExpressionExecutive.Evaluate(exp.expression, state);
            if (val == true ) return false;
            if (val == false ) return true;
            if (val == null ) return null;
            throw new NotImplementedException();
        }
        public static bool? Evaluate(AndExpression exp, PredicateList state)
        {
            bool? lhs = ExpressionExecutive.Evaluate(exp.lhs, state);
            bool? rhs = ExpressionExecutive.Evaluate(exp.rhs, state);
            if (lhs == true && rhs == true) return true;
            if (lhs == true && rhs == false) return false;
            if (lhs == true && rhs == null) return null;
            if (lhs == false && rhs == true) return false;
            if (lhs == false && rhs == false) return false;
            if (lhs == false && rhs == null) return null;
            if (lhs == null && rhs == true) return null;
            if (lhs == null && rhs == false) return null;
            if (lhs == null && rhs == null) return null;
            throw new NotImplementedException();
        }
        public static bool? Evaluate(OrExpression exp, PredicateList state)
        {
            bool? lhs = ExpressionExecutive.Evaluate(exp.lhs, state);
            bool? rhs = ExpressionExecutive.Evaluate(exp.rhs, state);
            if (lhs == true && rhs == true) return true;
            if (lhs == true && rhs == false) return true;
            if (lhs == true && rhs == null) return true;
            if (lhs == false && rhs == true) return true;
            if (lhs == false && rhs == false) return false;
            if (lhs == false && rhs == null) return null;
            if (lhs == null && rhs == true) return true;
            if (lhs == null && rhs == false) return null;
            if (lhs == null && rhs == null) return null;
            throw new NotImplementedException();
        }
        public static bool? Evaluate(XorExpression exp, PredicateList state)
        {
            bool? lhs = ExpressionExecutive.Evaluate(exp.lhs, state);
            bool? rhs = ExpressionExecutive.Evaluate(exp.rhs, state);
            if (lhs == true && rhs == true) return false;
            if (lhs == true && rhs == false) return true;
            if (lhs == true && rhs == null) return null;
            if (lhs == false && rhs == true) return true;
            if (lhs == false && rhs == false) return false;
            if (lhs == false && rhs == null) return null;
            if (lhs == null && rhs == true) return null;
            if (lhs == null && rhs == false) return null;
            if (lhs == null && rhs == null) return null;
            throw new NotImplementedException();
        }
        public static bool? Evaluate(ImplicationSyntacticExpression exp, PredicateList state)
        {
            bool? lhs = ExpressionExecutive.Evaluate(exp.lhs, state);
            bool? rhs = ExpressionExecutive.Evaluate(exp.rhs, state);
            if (lhs == true && rhs == true) return true;
            if (lhs == true && rhs == false) return false;
            if (lhs == true && rhs == null) return null;
            if (lhs == false && rhs == true) return true;
            if (lhs == false && rhs == false) return true;
            if (lhs == false && rhs == null) return true;
            if (lhs == null && rhs == true) return null;
            if (lhs == null && rhs == false) return null;
            if (lhs == null && rhs == null) return null;
            throw new NotImplementedException();
        }
        public static bool? Evaluate(IfAndOnlyIfSyntacticExpression exp, PredicateList state)
        {
            bool? lhs = ExpressionExecutive.Evaluate(exp.lhs, state);
            bool? rhs = ExpressionExecutive.Evaluate(exp.rhs, state);
            if (lhs == true && rhs == true) return true;
            if (lhs == true && rhs == false) return false;
            if (lhs == true && rhs == null) return null;
            if (lhs == false && rhs == true) return false;
            if (lhs == false && rhs == false) return true;
            if (lhs == false && rhs == null) return null;
            if (lhs == null && rhs == true) return null;
            if (lhs == null && rhs == false) return null;
            if (lhs == null && rhs == null) return null;
            throw new NotImplementedException();
        }
        public static bool? Evaluate(PredicateExpression exp, PredicateList state)
        {
            if( state.IsPredicate( exp.PredicateName ) )
            {
                PredicateSyntacticElement p = state.GetPredicate(exp.PredicateName);
                return p.Value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("predicate not found in state");
            }
        }
    }
}