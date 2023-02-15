using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace WorldCupBet
{


    public partial class WorldCupBetDeployment : WorldCupBetDeploymentBase
    {
        public WorldCupBetDeployment() : base(BYTECODE) { }
        public WorldCupBetDeployment(string byteCode) : base(byteCode) { }
    }

    public class WorldCupBetDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public WorldCupBetDeploymentBase() : base(BYTECODE) { }
        public WorldCupBetDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AddMatchFunction : AddMatchFunctionBase { }

    [Function("addMatch", "uint256")]
    public class AddMatchFunctionBase : FunctionMessage
    {
        [Parameter("string", "_team1", 1)]
        public virtual string Team1 { get; set; }
        [Parameter("string", "_team2", 2)]
        public virtual string Team2 { get; set; }
        [Parameter("uint256", "_time", 3)]
        public virtual BigInteger Time { get; set; }
        [Parameter("string", "_level", 4)]
        public virtual string Level { get; set; }
        [Parameter("uint256", "_team1AwardRate", 5)]
        public virtual BigInteger Team1AwardRate { get; set; }
        [Parameter("uint256", "_team2AwardRate", 6)]
        public virtual BigInteger Team2AwardRate { get; set; }
        [Parameter("uint256", "_drawAwardRate", 7)]
        public virtual BigInteger DrawAwardRate { get; set; }
    }

    public partial class BetFunction : BetFunctionBase { }

    [Function("bet")]
    public class BetFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_matchIndex", 1)]
        public virtual BigInteger MatchIndex { get; set; }
        [Parameter("uint256", "_choice", 2)]
        public virtual BigInteger Choice { get; set; }
        [Parameter("uint256", "_amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class ClaimFunction : ClaimFunctionBase { }

    [Function("claim", "uint256")]
    public class ClaimFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_matchIndex", 1)]
        public virtual BigInteger MatchIndex { get; set; }
    }

    public partial class GetAwardAmountFunction : GetAwardAmountFunctionBase { }

    [Function("getAwardAmount", "uint256")]
    public class GetAwardAmountFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_matchIndex", 1)]
        public virtual BigInteger MatchIndex { get; set; }
        [Parameter("address", "_beter", 2)]
        public virtual string Beter { get; set; }
    }

    public partial class GetBetAmountFunction : GetBetAmountFunctionBase { }

    [Function("getBetAmount", "uint256")]
    public class GetBetAmountFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_matchIndex", 1)]
        public virtual BigInteger MatchIndex { get; set; }
        [Parameter("address", "_beter", 2)]
        public virtual string Beter { get; set; }
    }

    public partial class GetChoiceFunction : GetChoiceFunctionBase { }

    [Function("getChoice", "uint256")]
    public class GetChoiceFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_matchIndex", 1)]
        public virtual BigInteger MatchIndex { get; set; }
        [Parameter("address", "_beter", 2)]
        public virtual string Beter { get; set; }
    }

    public partial class GetChoiceCountsFunction : GetChoiceCountsFunctionBase { }

    [Function("getChoiceCounts", typeof(GetChoiceCountsOutputDTO))]
    public class GetChoiceCountsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_matchIndex", 1)]
        public virtual BigInteger MatchIndex { get; set; }
    }

    public partial class InitializeFunction : InitializeFunctionBase { }

    [Function("initialize")]
    public class InitializeFunctionBase : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "_tokenAddress", 2)]
        public virtual string TokenAddress { get; set; }
    }

    public partial class MatchCountFunction : MatchCountFunctionBase { }

    [Function("matchCount", "uint256")]
    public class MatchCountFunctionBase : FunctionMessage
    {

    }

    public partial class MatchInfosFunction : MatchInfosFunctionBase { }

    [Function("matchInfos", typeof(MatchInfosOutputDTO))]
    public class MatchInfosFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class RecoverTokensFunction : RecoverTokensFunctionBase { }

    [Function("recoverTokens")]
    public class RecoverTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenAmount", 1)]
        public virtual BigInteger TokenAmount { get; set; }
    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class SetMatchResultFunction : SetMatchResultFunctionBase { }

    [Function("setMatchResult")]
    public class SetMatchResultFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_index", 1)]
        public virtual BigInteger Index { get; set; }
        [Parameter("uint256", "_result", 2)]
        public virtual BigInteger Result { get; set; }
    }

    public partial class TokenFunction : TokenFunctionBase { }

    [Function("token", "address")]
    public class TokenFunctionBase : FunctionMessage
    {

    }

    public partial class TotalAwardAmountFunction : TotalAwardAmountFunctionBase { }

    [Function("totalAwardAmount", "uint256")]
    public class TotalAwardAmountFunctionBase : FunctionMessage
    {

    }

    public partial class TotalBetAmountFunction : TotalBetAmountFunctionBase { }

    [Function("totalBetAmount", "uint256")]
    public class TotalBetAmountFunctionBase : FunctionMessage
    {

    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class UpdateMatchFunction : UpdateMatchFunctionBase { }

    [Function("updateMatch")]
    public class UpdateMatchFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_index", 1)]
        public virtual BigInteger Index { get; set; }
        [Parameter("string", "_team1", 2)]
        public virtual string Team1 { get; set; }
        [Parameter("string", "_team2", 3)]
        public virtual string Team2 { get; set; }
        [Parameter("string", "_level", 4)]
        public virtual string Level { get; set; }
        [Parameter("uint256", "_time", 5)]
        public virtual BigInteger Time { get; set; }
        [Parameter("uint256", "_team1AwardRate", 6)]
        public virtual BigInteger Team1AwardRate { get; set; }
        [Parameter("uint256", "_team2AwardRate", 7)]
        public virtual BigInteger Team2AwardRate { get; set; }
        [Parameter("uint256", "_drawAwardRate", 8)]
        public virtual BigInteger DrawAwardRate { get; set; }
    }

    public partial class BetEventDTO : BetEventDTOBase { }

    [Event("Bet")]
    public class BetEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "matchIndex", 1, true )]
        public virtual BigInteger MatchIndex { get; set; }
        [Parameter("address", "sender", 2, true )]
        public virtual string Sender { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "timestamp", 4, false )]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class ClaimEventDTO : ClaimEventDTOBase { }

    [Event("Claim")]
    public class ClaimEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "matchIndex", 1, true )]
        public virtual BigInteger MatchIndex { get; set; }
        [Parameter("address", "sender", 2, true )]
        public virtual string Sender { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "timestamp", 4, false )]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true )]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true )]
        public virtual string NewOwner { get; set; }
    }







    public partial class GetAwardAmountOutputDTO : GetAwardAmountOutputDTOBase { }

    [FunctionOutput]
    public class GetAwardAmountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetBetAmountOutputDTO : GetBetAmountOutputDTOBase { }

    [FunctionOutput]
    public class GetBetAmountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetChoiceOutputDTO : GetChoiceOutputDTOBase { }

    [FunctionOutput]
    public class GetChoiceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetChoiceCountsOutputDTO : GetChoiceCountsOutputDTOBase { }

    [FunctionOutput]
    public class GetChoiceCountsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
        [Parameter("uint256", "", 3)]
        public virtual BigInteger ReturnValue3 { get; set; }
        [Parameter("uint256", "", 4)]
        public virtual BigInteger ReturnValue4 { get; set; }
    }



    public partial class MatchCountOutputDTO : MatchCountOutputDTOBase { }

    [FunctionOutput]
    public class MatchCountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class MatchInfosOutputDTO : MatchInfosOutputDTOBase { }

    [FunctionOutput]
    public class MatchInfosOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "team1", 1)]
        public virtual string Team1 { get; set; }
        [Parameter("string", "team2", 2)]
        public virtual string Team2 { get; set; }
        [Parameter("uint256", "time", 3)]
        public virtual BigInteger Time { get; set; }
        [Parameter("string", "level", 4)]
        public virtual string Level { get; set; }
        [Parameter("uint256", "result", 5)]
        public virtual BigInteger Result { get; set; }
        [Parameter("uint256", "betAmount", 6)]
        public virtual BigInteger BetAmount { get; set; }
        [Parameter("uint256", "awardAmount", 7)]
        public virtual BigInteger AwardAmount { get; set; }
        [Parameter("uint256", "team1AwardRate", 8)]
        public virtual BigInteger Team1AwardRate { get; set; }
        [Parameter("uint256", "team2AwardRate", 9)]
        public virtual BigInteger Team2AwardRate { get; set; }
        [Parameter("uint256", "drawAwardRate", 10)]
        public virtual BigInteger DrawAwardRate { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }







    public partial class TokenOutputDTO : TokenOutputDTOBase { }

    [FunctionOutput]
    public class TokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TotalAwardAmountOutputDTO : TotalAwardAmountOutputDTOBase { }

    [FunctionOutput]
    public class TotalAwardAmountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TotalBetAmountOutputDTO : TotalBetAmountOutputDTOBase { }

    [FunctionOutput]
    public class TotalBetAmountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
