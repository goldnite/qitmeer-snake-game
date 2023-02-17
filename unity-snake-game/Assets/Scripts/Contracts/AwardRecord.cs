using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace SnakeGame
{
    public partial class AwardRecord : AwardRecordBase { }

    public class AwardRecordBase 
    {
        [Parameter("address[5]", "awardees", 1)]
        public virtual List<string> Awardees { get; set; }
        [Parameter("uint256[5]", "scores", 2)]
        public virtual List<BigInteger> Scores { get; set; }
        [Parameter("uint256[5]", "amounts", 3)]
        public virtual List<BigInteger> Amounts { get; set; }
        [Parameter("uint256", "timestamp", 4)]
        public virtual BigInteger Timestamp { get; set; }
    }
}
