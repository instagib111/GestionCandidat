using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestionCandidat.Tests
{
    [TestClass]
    public class Recherche
    {
        [TestMethod]
        public void formatageRecherche_JAVA()
        {
            Assert.AreEqual("java", formatageRecherche__java__JAVA("JAVA"));
        }
        [TestMethod]
        public void formatageRecherche_JAVA_CDieze()
        {
            Assert.AreEqual("java,c#", formatageRecherche_JAVA_java("JAVA C#"));
        }
        [TestMethod]
        public void formatageRecherche_JAVA_plusCDieze()
        {
            Assert.AreEqual("java,%2bc%23", formatageRecherche_JAVA_java("JAVA + C#"));
        }
        [TestMethod]
        public void formatageRecherche_JAVA_CDIEZE_PLUSSQL()
        {
            Assert.AreEqual("java,c%23,%2bsql", formatageRecherche_JAVA_java("jaVA C# + sql"));
        }
        [TestMethod]
        public void formatageRecherche_JAVA_CDIEZE_PLUSSQL_scala()
        {
            Assert.AreEqual("java,c%23,%2bsql,%2bscala", formatageRecherche_JAVA_java("jAVA C# + SQL Scala"));
        }
        [TestMethod]
        public void formatageRecherche_JAVA_CDIEZE_MOINSSQL_SCALA()
        {
            Assert.AreEqual("java,c%23,-sql,-scala", formatageRecherche_JAVA_java("JAVA C# - SQL SCALA "));
        }
        [TestMethod]
        public void formatageRecherche_JAVA_CDIEZE_plusSQL_moinsSCALA()
        {
            Assert.AreEqual("java,c%23,%2bsql,-scala", formatageRecherche_JAVA_java("JAVA    C# + SQL -SCALA "));
        }
        [TestMethod]
        public void formatageRecherche_JAVA_CDIEZE_MOINSSQL_plusSCALA()
        {
            Assert.AreEqual("java,c%23,-sql,%2badelia", formatageRecherche_JAVA_java("    JAVA C# -SQL +Adélia "));
        }
        [TestMethod]
        public void formatageRecherche_moinsJAVA_CDIEZE_MOINSSQL_plusSCALA()
        {
            Assert.AreEqual("java,c%23,-sql,%2badelia", formatageRecherche_JAVA_java("- JAVA C# -SQL +Adélia "));
        }
    }
}
