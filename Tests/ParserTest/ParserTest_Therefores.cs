using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLang;
using ZedLangSyntacticElements;

namespace ParserTest
{
    [TestClass]
    public class ParserTest_Therefores
    {
        [TestMethod]
        public void LineParser_ValidThereforeDefinition_ReturnsTherefore()
        {
            string line;
            line = "\ttherefore r ";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(ThereforeSyntacticElement));
            Assert.AreEqual("therefore r", ((ThereforeSyntacticElement)parseResult).Text);
        }
        [TestMethod]
        public void LineParser_TooManyImplies_ReturnsInvalidSyntax()
        {
            string line = "\ttherefore p implies z";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }

    }
}
