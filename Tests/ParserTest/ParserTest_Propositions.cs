using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZedLang;
using ZedLangSyntacticElements;

namespace ParserTest
{
    [TestClass]
    public class ParserTest_Propositions
    {
        [TestMethod]
        public void LineParser_ValidImplicationDefinitionPandNotQ_ReturnsPropsition()
        {
            string line;
            line = "\tproposition ( p and not q ) implies r ";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(ImplicationSyntacticExpression), parseResult.GetType());
            Assert.AreEqual("If (p and (not q)) Then r", ((ImplicationSyntacticExpression)parseResult).Text);
        }
        [TestMethod]
        public void LineParser_ValidImplicationDefinition_ReturnsPropsition()
        {
            string line;
            line = "\tproposition p implies q ";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(ImplicationSyntacticExpression), parseResult.GetType());
            Assert.AreEqual("If p Then q", ((ImplicationSyntacticExpression)parseResult).Text);
        }
        [TestMethod]
        public void LineParser_ValidIfAndOnlyIfDefinition_ReturnsPropsition()
        {
            string line;
            line = "\tproposition p IfAndonlyIf q ";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(IfAndOnlyIfSyntacticExpression), parseResult.GetType());
            Assert.AreEqual("If and only if p Then q", ((IfAndOnlyIfSyntacticExpression)parseResult).Text);
        }
        [TestMethod]
        public void LineParser_TooManyImplies_ReturnsInvalidSyntax()
        {
            string line = "\tproposition p implies q implies z";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser_TooManyIfAndOnlyIfs_ReturnsInvalidSyntax()
        {
            string line = "\tproposition p implies q implies z";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser_NoLHS_ReturnsInvalidSyntax()
        {
            string line = "\tproposition ifandonlyif z";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser_InvalidLHS_ReturnsInvalidSyntax()
        {
            string line = "\tproposition ( p and q r ) ifandonlyif z";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser_NoRHS_ReturnsInvalidSyntax()
        {
            string line = "\tproposition p ifandonlyif ";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser_InvalidRHS_ReturnsInvalidSyntax()
        {
            string line = "\tproposition ( p and q ) ifandonlyif z r";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser_TooManyPropositionQualifiers_ReturnsInvalidSyntax()
        {
            string line = "\tproposition p ifandonlyif q implies z";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser__NoPropositionKeyword_ReturnsInvalidSyntax()
        {
            string line = "p implies q";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser__NoPropositionSymbol_ReturnsInvalidSyntax()
        {
            string line = "\tproposition p q ";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void LineParser__NoPropositionSymbol_onepredicate_ReturnsPropositionExpression()
        {
            string line = "\tproposition p ";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(PropositionSyntacticElement), parseResult.GetType());
            Assert.AreEqual("p", ((PropositionSyntacticElement)parseResult).Text);
        }
        [TestMethod]
        public void LineParser__NoPropositionSymbol_onepredicateNegated_ReturnsNotExpression()
        {
            string line = "\tproposition not(p) ";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(PropositionNegationSyntacticElement), parseResult.GetType());
            Assert.AreEqual("not (p)", ((PropositionNegationSyntacticElement)parseResult).Text);
        }
    }
}
