using System;
using NameSort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace NameSortTests
{
    [TestClass]
    public class UnitTests
    {

        //This tests one list against the sort method with different permutations of first/last difference
        [TestMethod]
        public void SortListTest()
        {
            //setup dummy data for testing
            List<String[]> input = new List<String[]>();
            List<String[]> expectedOutput = new List<String[]>();
            String[] row1 = new String[] { "SMITH", "AARON" };
            input.Add(row1);
            String[] row2 = new String[] { "ANDERSON", "THOMAS" };
            input.Add(row2);
            String[] row3 = new String[] { "JAMIESON", "JAMES" };
            input.Add(row3);
            String[] row4 = new String[] { "HASTINGS", "STEVE" };
            input.Add(row4);
            String[] row5 = new String[] { "SMITH", "JOHN" };
            input.Add(row5);
            String[] row6 = new String[] { "KENT", "JOHN" };
            input.Add(row6);
            String[] row7 = new String[] { "SMITH", "AVERY" };
            input.Add(row7);
            expectedOutput.Add(row2);
            expectedOutput.Add(row4);
            expectedOutput.Add(row3);
            expectedOutput.Add(row6);
            expectedOutput.Add(row1);
            expectedOutput.Add(row7);
            expectedOutput.Add(row5);
            Assert.AreEqual(true, expectedOutput.SequenceEqual(NameSort.Program.SortList(input)));
        }

        [TestMethod]
        public void WriteListTest()
        {
            //setup dummy data for testing
            List<String[]> input = new List<String[]>();
            String[] row1 = new String[] { "SMITH", "AARON" };
            String[] row2 = new String[] { "ANDERSON", "THOMAS" };
            String[] row3 = new String[] { "JAMIESON", "JAMES" };
            String[] row4 = new String[] { "HASTINGS", "STEVE" };
            String[] row5 = new String[] { "SMITH", "JOHN" };
            String[] row6 = new String[] { "KENT", "JOHN" };
            String[] row7 = new String[] { "SMITH", "AVERY" };
            input.Add(row2);
            input.Add(row4);
            input.Add(row3);
            input.Add(row6);
            input.Add(row1);
            input.Add(row7);
            input.Add(row5);
            String expectedString = "ANDERSON, THOMAS" + '\r' + '\n' +
                                        "HASTINGS, STEVE" + '\r' + '\n' +
                                        "JAMIESON, JAMES" + '\r' + '\n' +
                                        "KENT, JOHN" + '\r' + '\n' +
                                        "SMITH, AARON" + '\r' + '\n' +
                                        "SMITH, AVERY" + '\r' + '\n' +
                                        "SMITH, JOHN";
            String[] expectedOutput = new String[] {"Success","D:\\unittest-sorted.txt",expectedString};
            Assert.AreEqual(true, expectedOutput.SequenceEqual(NameSort.Program.WriteList(input, "D:\\unittest.txt")));
            String fileText;
            try
            {
                using (var streamReader = new StreamReader(@"D:\\unittest-sorted.txt", Encoding.UTF8))
                {
                    fileText = streamReader.ReadToEnd();
                    if (!fileText.Equals(expectedString))
                    {
                        Assert.Fail("File contents not correct");
                    }
                }
            }
            catch
            {
                Assert.Fail("File read failed. Could mean file does not exist.");
            }
        }

        //Test the read file method
        [TestMethod]
        public void ReadFileTest()
        {
            //write a dummy file for testing
            string filePath = @"D:\\fileread-unittest.txt";
            String fileContents = "ANDERSON, THOMAS" + '\r' + '\n' +
                                        "HASTINGS, STEVE" + '\r' + '\n' +
                                        "JAMIESON, JAMES" + '\r' + '\n' +
                                        "KENT, JOHN" + '\r' + '\n' +
                                        "SMITH, AARON" + '\r' + '\n' +
                                        "SMITH, AVERY" + '\r' + '\n' +
                                        "SMITH, JOHN";
            List<String[]> expectedOutput = new List<String[]>();
            String[] row1 = new String[] { "SMITH", "AARON" };
            String[] row2 = new String[] { "ANDERSON", "THOMAS" };
            String[] row3 = new String[] { "JAMIESON", "JAMES" };
            String[] row4 = new String[] { "HASTINGS", "STEVE" };
            String[] row5 = new String[] { "SMITH", "JOHN" };
            String[] row6 = new String[] { "KENT", "JOHN" };
            String[] row7 = new String[] { "SMITH", "AVERY" };
            expectedOutput.Add(row2);
            expectedOutput.Add(row4);
            expectedOutput.Add(row3);
            expectedOutput.Add(row6);
            expectedOutput.Add(row1);
            expectedOutput.Add(row7);
            expectedOutput.Add(row5);
            File.WriteAllText(filePath, fileContents);
            List<String[]> result = NameSort.Program.ReadFile(filePath);

            //because the arrays reference different objects, we need to perform a big loop through them all
            if (result.Count == expectedOutput.Count)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].Length.Equals(expectedOutput[i].Length))
                    {
                        for (int j = 0; j < result[i].Length; j++)
                            if (result[i][j].Equals(expectedOutput[i][j]))
                            {
                            }
                            else
                            {
                                Assert.Fail("String " + i + "," + j + "did not match between arrays");
                            }
                    }
                    else
                    {
                        Assert.Fail("File arrays are different lengths.");
                    }
                }
            }
            else
            {
                Assert.Fail("File lists are different lengths.");
            }
        }
    }
}
