using KerbalSimpit.Core;
using KerbalSimpit.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace KerbalSimpit.Test.Core
{
    [TestClass]
    public class COBSHelperTest
    {
        [TestMethod]
        [DataRow(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, DisplayName = "COBSHelper_Decode_Matches_Encode_Scenario_01")]
        [DataRow(new byte[] { 0, 1, 0, 1, 0, 1, 0, 1 }, DisplayName = "COBSHelper_Decode_Matches_Encode_Scenario_02")]
        [DataRow(new byte[] { 0, 1, 2, 4, 8, 16, 32, 64, 128 }, DisplayName = "COBSHelper_Decode_Matches_Encode_Scenario_03")]
        [DataRow(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, DisplayName = "COBSHelper_Decode_Matches_Encode_Scenario_04")]
        public void COBSHelper_Decode_Matches_Encode(byte[] data)
        {
            SimpitStream input = new SimpitStream();
            SimpitStream output = new SimpitStream();

            input.Write(data);
            Assert.IsTrue(COBSHelper.TryEncodeCOBS(input, output));

            byte[] encoded = output.ReadAll(out int offset, out int count).Skip(offset).Take(count).ToArray();

            input.Clear();
            output.Clear();

            input.Write(encoded);
            Assert.IsTrue(COBSHelper.TryDecodeCOBS(input, output));

            byte[] decoded = output.ReadAll(out offset, out count).Skip(offset).Take(count).ToArray();

            Assert.AreEqual(data.Length, decoded.Length);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], decoded[i]);
            }
        }
    }
}
