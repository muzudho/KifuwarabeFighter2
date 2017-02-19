using NUnit.Framework;
using System.Collections.Generic;

namespace DojinCircleGrayscale.StellaQL
{
    public class TestFullpathParser
    {
        #region N70 Syntax Parser
        // 構文パーサー
        [Test]
        public void N80_Syntax_Fixed_LayerName()
        {
            string query = "Base Layer.Alpaca.Bear.Cat";
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();

            bool successful = FullpathSyntaxP.Fixed_LayerName(query, ref caret, ref ft);

            Assert.IsTrue(successful);
            Assert.AreEqual(11, caret);
            Assert.AreEqual("Base Layer", ft.LayerNameEndsWithoutDot);
        }

        [Test]
        public void N80_Syntax_Fixed_LayerName_And_StatemachineNames()
        {
            string query = "Base Layer.Alpaca.Bear.Cat";
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();

            bool successful = FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(query, ref caret, ref ft);

            Assert.IsTrue(successful);
            Assert.AreEqual(23, caret);
            Assert.AreEqual("Base Layer", ft.LayerNameEndsWithoutDot);
            Assert.AreEqual(2, ft.StatemachineNamesEndsWithoutDot.Count);
            Assert.AreEqual("Alpaca", ft.StatemachineNamesEndsWithoutDot[0]);
            Assert.AreEqual("Bear", ft.StatemachineNamesEndsWithoutDot[1]);
        }

        [Test]
        public void N80_Syntax_Fixed_LayerName_And_StatemachineNames_And_StateName()
        {
            string query = "Base Layer.Alpaca.Bear.Cat";
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();

            bool successful = FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames_And_StateName(query, ref caret, ref ft);

            Assert.IsTrue(successful);
            Assert.AreEqual(26, caret);
            Assert.AreEqual("Base Layer", ft.LayerNameEndsWithoutDot);
            Assert.AreEqual(2, ft.StatemachineNamesEndsWithoutDot.Count);
            Assert.AreEqual("Alpaca", ft.StatemachineNamesEndsWithoutDot[0]);
            Assert.AreEqual("Bear", ft.StatemachineNamesEndsWithoutDot[1]);
            Assert.AreEqual("Cat", ft.StateName);
        }

        [Test]
        public void N80_Syntax_Continued_Fixed_LayerName_And_StatemachineNames_And_StateName()
        {
            string query = "Base Layer.Alpaca.Bear.Cat";
            int caret = 0;
            FullpathTokens ft = new FullpathTokens();

            // "~ Bear." まで読む
            bool successful1 = FullpathSyntaxP.Fixed_LayerName_And_StatemachineNames(query, ref caret, ref ft);
            Assert.IsTrue(successful1);

            // 続きから "Cat" まで読む
            bool successful2 = FullpathSyntaxP.Continued_Fixed_StateName(query, ref caret, ft, ref ft);

            Assert.IsTrue(successful2);
            Assert.AreEqual(26, caret);
            Assert.AreEqual("Base Layer", ft.LayerNameEndsWithoutDot);
            Assert.AreEqual(2, ft.StatemachineNamesEndsWithoutDot.Count);
            Assert.AreEqual("Alpaca", ft.StatemachineNamesEndsWithoutDot[0]);
            Assert.AreEqual("Bear", ft.StatemachineNamesEndsWithoutDot[1]);
            Assert.AreEqual("Cat", ft.StateName);
        }
        #endregion



        #region N80 Lexical Parser
        // 字句解析パーサー
        [Test]
        public void N80_Lexical_VarLayerName()
        {
            string query = "Base Layer.Alpaca.Bear.Cat";
            int caret = 0;
            string layerNameEndsWithoutDot;

            bool successful = FullpathLexcalP.VarLayerName(query, ref caret, out layerNameEndsWithoutDot);

            Assert.IsTrue(successful);
            Assert.AreEqual(11, caret);
            Assert.AreEqual("Base Layer", layerNameEndsWithoutDot);
        }

        [Test]
        public void N80_Lexical_VarStatemachineNames()
        {
            string query = "Base Layer.Alpaca.Bear.Cat";
            // "Base Layer." まで走査した続きから。
            int caret = "Base Layer.".Length;
            List<string> statemachinesNameEndsWithoutDot;

            bool successful = FullpathLexcalP.VarStatemachineNames(query, ref caret, out statemachinesNameEndsWithoutDot);

            Assert.IsTrue(successful);
            // "～Bear." の次。
            Assert.AreEqual(23, caret);
            Assert.AreEqual(2, statemachinesNameEndsWithoutDot.Count);
            Assert.AreEqual("Alpaca", statemachinesNameEndsWithoutDot[0]);
            Assert.AreEqual("Bear", statemachinesNameEndsWithoutDot[1]);
        }

        [Test]
        public void N80_Lexical_VarStateName()
        {
            string query = "Base Layer.Alpaca.Bear.Cat";
            // "Base Layer.Alpaca.Bear." まで走査した続きから。
            int caret = "Base Layer.Alpaca.Bear.".Length;
            string stateName;

            bool successful = FullpathLexcalP.VarStateName(query, ref caret, out stateName);

            Assert.IsTrue(successful);
            // "Cat" の次。
            Assert.AreEqual(26, caret);
            Assert.AreEqual("Cat", stateName);
        }
        #endregion
    }
}
