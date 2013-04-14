using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLTool;

namespace UnitTestProject
{
    [TestClass]
    public class ApplicationNameTextBoxUnitTest
    {
        /// <summary>
        /// Checks that the TextBox and its associated value in App are empty and that the text box is enabled
        /// </summary>
        [TestMethod]
        public void initialisationTest()
        {
            MainForm testSubject = new MainForm();
            string expected = "My Application";
            Assert.IsTrue(testSubject.applicationNameTextBox.Enabled);
            Assert.AreEqual(testSubject.applicationNameTextBox.Text, expected);
            Assert.AreEqual(testSubject.mApp.mName, expected);
        }

        /// <summary>
        /// Checks that the appropriate value is passed to the app from the text box
        /// </summary>
        [TestMethod]
        public void modificationTest()
        {
            MainForm testSubject = new MainForm();
            string expected = "Modified Application Name";
            testSubject.applicationNameTextBox.Text = expected;
            Assert.AreEqual(testSubject.mApp.mName, expected);
        }
    }
}
